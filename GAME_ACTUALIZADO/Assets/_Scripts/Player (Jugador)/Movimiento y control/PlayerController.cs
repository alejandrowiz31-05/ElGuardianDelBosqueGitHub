using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // =========================
    // ATRIBUTOS DE MOVIMIENTO
    // =========================
    public float runSpeed = 6f;        // Velocidad al correr
    public float jumpForce = 5f;       // Fuerza del salto
    public float gravity = -9.81f;     // Gravedad
    public float walkSpeed = 3f;       // Velocidad al caminar

    // =========================
    // COMPONENTES
    // =========================
    private CharacterController characterController;
    private Animator animator;
    private FootstepPlayer footstep;

    [SerializeField] private StaminaBar staminaBar;

    // =========================
    // INPUT DE MOVIMIENTO
    // =========================
    private float horizontal;
    private float vertical;

    // =========================
    // ESTADO DEL JUGADOR
    // =========================
    public bool isSprinting;
    private bool isJumping;
    private float verticalVelocity;

    [Header("Movimiento cámara")]
    public Transform cameraTransform;
    public float turnSmoothTime = 0.12f;
    private float turnSmoothVelocity;

    // Variables intermedias que varios métodos necesitan compartir en el mismo frame
    private Vector3 inputDireccion;
    private Vector3 moveDir;
    private bool moving;


    // =========================
    // INICIO
    // =========================
    private void Awake()
    {
        animator = GetComponent<Animator>();
        footstep = GetComponent<FootstepPlayer>();
        characterController = GetComponent<CharacterController>();
    }


    // =========================
    // UPDATE — orquesta el frame llamando a cada método
    // =========================
    private void Update()
    {
        ObtenerInput();

        // Si el personaje está en animación de levantarse, se bloquea el movimiento
        if (BloquearMovimientoSiSeEstaLevantando())
            return;

        Mover();
        Saltar();
        DetectarPasos();
        Correr();
        ManejarStamina();
        ActualizarAnimacionesMovimiento();
        ManejarEspada();
        Atacar();
    }


    // =========================
    // MÉTODO: ObtenerInput
    // Lee las teclas de movimiento presionadas por el jugador
    // =========================
    private void ObtenerInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        inputDireccion = new Vector3(horizontal, 0f, vertical);
        moving = inputDireccion.sqrMagnitude > 0.001f;
    }


    // =========================
    // MÉTODO: Mover
    // Calcula dirección relativa a la cámara, gira al personaje
    // y lo desplaza en el mundo
    // =========================
    private void Mover()
    {
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;
        moveDir = Vector3.zero;

        if (moving)
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            float cameraY = (cameraTransform != null) ? cameraTransform.eulerAngles.y : 0f;
            float targetAngle = Mathf.Atan2(inputDireccion.x, inputDireccion.z) * Mathf.Rad2Deg + cameraY;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        Vector3 finalMove = moveDir.normalized * currentSpeed + Vector3.up * verticalVelocity;
        characterController.Move(finalMove * Time.deltaTime);
    }


    // =========================
    // MÉTODO: Saltar
    // Aplica gravedad cada frame y ejecuta el salto cuando corresponde
    // =========================
    private void Saltar()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                // Pequeña fuerza hacia abajo para mantener al CharacterController "pegado" al suelo
                verticalVelocity = -2f;
            }

            isJumping = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
                isJumping = true;
                verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        verticalVelocity += gravity * Time.deltaTime;
    }


    // =========================
    // MÉTODO: BloquearMovimientoSiSeEstaLevantando
    // Evita que el jugador se mueva mientras la animación "Standing_Up" está activa
    // =========================
    private bool BloquearMovimientoSiSeEstaLevantando()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Standing_Up"))
        {
            footstep.SetMoving(false);
            AplicarGravedadSola();
            return true;
        }

        return false;
    }


    // =========================
    // MÉTODO: AplicarGravedadSola
    // Mantiene la gravedad activa mientras el movimiento está bloqueado
    // =========================
    private void AplicarGravedadSola()
    {
        if (characterController.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }


    // =========================
    // MÉTODO: DetectarPasos
    // Avisa al FootstepPlayer si el personaje se está moviendo o no
    // =========================
    private void DetectarPasos()
    {
        footstep.SetMoving(moving);
    }


    // =========================
    // MÉTODO: Correr
    // Determina si el jugador está corriendo según la tecla e stamina disponible
    // =========================
    private void Correr()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift)
                         && staminaBar.currentStamina > 0;

        isSprinting = isRunning;
    }


    // =========================
    // MÉTODO: ManejarStamina
    // Consume o recupera stamina según si el jugador corre o no
    // =========================
    private void ManejarStamina()
    {
        if (isSprinting && moving)
        {
            staminaBar.UseStamina(Time.deltaTime);
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            staminaBar.RecoverStamina(Time.deltaTime);
        }
    }


    // =========================
    // MÉTODO: ActualizarAnimacionesMovimiento
    // Sincroniza el Animator con el estado actual de movimiento
    // =========================
    private void ActualizarAnimacionesMovimiento()
    {
        animator.SetBool("IsRunning", isSprinting);
        animator.SetBool("IsWalking", moving);
    }


    // =========================
    // MÉTODO: ManejarEspada
    // Saca o guarda la espada/daga según la tecla presionada
    // =========================
    private void ManejarEspada()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("DrawSword");
            animator.SetBool("IsArmed", true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("SheathSword");
            animator.SetBool("IsArmed", false);
        }
    }


    // =========================
    // MÉTODO: Atacar
    // Ejecuta las animaciones de ataque si el personaje está armado
    // =========================
    private void Atacar()
    {
        if (Input.GetMouseButtonDown(0) && animator.GetBool("IsArmed"))
        {
            animator.SetTrigger("SlashIn");
        }

        if (Input.GetMouseButtonDown(1) && animator.GetBool("IsArmed"))
        {
            animator.SetTrigger("SlashOut");
        }
    }
}