using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 6f;        // Velocidad al correr
    public float jumpForce = 5f;      // Fuerza del salto
    public float gravity = -9.81f;      // Gravedad
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
    // UPDATE
    // =========================

    private void Update()
    {
        // -------------------------
        // OBTENER INPUT
        // -------------------------

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");



        // =========================
        // MOVIMIENTO (RELATIVO A LA CÁMARA) Y GIRO SUAVE
        // =========================

        // Entrada en XZ
        Vector3 input = new Vector3(horizontal, 0f, vertical);

        // Velocidad actual según sprint
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // Dirección de movimiento inicial
        Vector3 moveDir = Vector3.zero;

        // Si hay entrada significativa, calcular dirección relativa a la cámara
        if (input.sqrMagnitude > 0.001f)
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            float cameraY = (cameraTransform != null) ? cameraTransform.eulerAngles.y : 0f;
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cameraY;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        // -------------------------
        // GRAVEDAD Y SALTO
        // -------------------------

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                // Pequeña fuerza hacia abajo para mantener al CharacterController "pegado" al suelo
                verticalVelocity = -2f;
            }

            // Reset estado de salto al estar en suelo
            isJumping = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
                isJumping = true;
                verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        // Aplicar gravedad cada frame
        verticalVelocity += gravity * Time.deltaTime;

        // Mover en dirección horizontal + componente vertical
        Vector3 finalMove = moveDir.normalized * currentSpeed + Vector3.up * verticalVelocity;
        characterController.Move(finalMove * Time.deltaTime);


        // -------------------------
        // BLOQUEAR MOVIMIENTO
        // AL LEVANTARSE
        // -------------------------

        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Standing_Up"))
        {
            footstep.SetMoving(false);
            return;
        }


        // -------------------------
        // DETECTAR MOVIMIENTO
        // -------------------------

        bool moving = input.sqrMagnitude > 0.001f;

        footstep.SetMoving(moving);


        // -------------------------
        // CORRER
        // -------------------------

        bool isRunning = Input.GetKey(KeyCode.LeftShift)
                         && staminaBar.currentStamina > 0;

        isSprinting = isRunning;


        // -------------------------
        // STAMINA
        // -------------------------

        if (isSprinting && moving)
        {
            staminaBar.UseStamina(Time.deltaTime);
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            staminaBar.RecoverStamina(Time.deltaTime);
        }


        // -------------------------
        // ANIMACIONES
        // -------------------------

        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsWalking", moving);





        // -------------------------
        // ESPADA
        // -------------------------

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


        // -------------------------
        // ATAQUES
        // -------------------------

        if (Input.GetMouseButtonDown(0) && animator.GetBool("IsArmed"))
        {
            animator.SetTrigger("SlashIn");
        }

        if (Input.GetMouseButtonDown(1) && animator.GetBool("IsArmed"))
        {
            animator.SetTrigger("SlashOut");
        }
    }


  


    // =========================
    // ATERRIZAJE
    // =========================

     // AQUÍ VAMOS A DETECTAR
        // CUANDO EL JUGADOR TOCA EL SUELO
    
}