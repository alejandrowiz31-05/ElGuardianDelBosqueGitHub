
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // =========================
    // CONFIGURACIÓN DEL MOVIMIENTO
    // =========================

    // Velocidad del Viajero cuando corre.
    public float runSpeed = 6f;

    // Fuerza que utiliza el Viajero para saltar.
    public float jumpForce = 5f;

    // Fuerza de gravedad que afecta al Viajero.
    public float gravity = -9.81f;

    // Velocidad del Viajero cuando camina.
    public float walkSpeed = 3f;


    // =========================
    // COMPONENTES
    // =========================

    // Guarda el componente que controla el movimiento del Viajero.
    private CharacterController characterController;

    // Guarda el componente que controla las animaciones del Viajero.
    private Animator animator;

    // Guarda el componente que controla los sonidos de los pasos.
    private FootstepPlayer footstep;

    // Guarda la referencia a la barra de resistencia del Viajero.
    [SerializeField] private StaminaBar staminaBar;


    // =========================
    // CONTROLES Y ESTADO
    // =========================

    // Guarda el movimiento horizontal indicado por el jugador.
    private float horizontal;

    // Guarda el movimiento vertical indicado por el jugador.
    private float vertical;

    // Indica si el Viajero está corriendo.
    public bool isSprinting;

    // Guarda la velocidad vertical utilizada para saltar y caer.
    private float verticalVelocity;

    // Guarda la dirección indicada por las teclas de movimiento.
    private Vector3 inputDireccion;

    // Guarda la dirección hacia la que se moverá el Viajero.
    private Vector3 moveDir;

    // Indica si el jugador está intentando moverse.
    private bool moving;


    // =========================
    // CONFIGURACIÓN DE LA CÁMARA
    // =========================

    [Header("Movimiento cámara")]

    // Guarda la cámara que se utiliza para orientar el movimiento.
    public Transform cameraTransform;

    // Controla qué tan suave gira el Viajero.
    public float turnSmoothTime = 0.12f;

    // Guarda un valor utilizado para suavizar el giro.
    private float turnSmoothVelocity;


    // =========================
    // PREPARACIÓN DE COMPONENTES
    // =========================

    // Se ejecuta cuando Unity carga el objeto que tiene este script.
    private void Awake()
    {
        // Busca el componente Animator en el objeto.
        animator = GetComponent<Animator>();

        // Busca el componente FootstepPlayer en el objeto.
        footstep = GetComponent<FootstepPlayer>();

        // Busca el componente CharacterController en el objeto.
        characterController = GetComponent<CharacterController>();
    }


    // =========================
    // ACTUALIZACIÓN DEL JUGADOR
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Lee las teclas que utiliza el jugador para moverse.
        ObtenerInput();

        // Comprueba si el Viajero está en la animación de levantarse.
        if (BloquearMovimientoSiSeEstaLevantando())
        {
            // Detiene las demás acciones mientras se levanta.
            return;
        }

        // Ejecuta las acciones principales del Viajero.
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
    // LECTURA DE CONTROLES
    // =========================

    // Lee las teclas de movimiento y comprueba si el Viajero quiere caminar.
    private void ObtenerInput()
    {
        // Obtiene el movimiento horizontal.
        horizontal = Input.GetAxis("Horizontal");

        // Obtiene el movimiento vertical.
        vertical = Input.GetAxis("Vertical");

        // Guarda la dirección indicada por el jugador.
        inputDireccion = new Vector3(horizontal, 0f, vertical);

        // Comprueba si el jugador está indicando algún movimiento.
        moving = inputDireccion.sqrMagnitude > 0.001f;
    }


    // =========================
    // MOVIMIENTO DEL VIAJERO
    // =========================

    // Calcula la dirección, gira al Viajero y lo mueve por el escenario.
    private void Mover()
    {
        // Guarda la velocidad que utilizará el Viajero.
        float currentSpeed;

        // Comprueba si el Viajero está corriendo.
        if (isSprinting)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // Reinicia la dirección de movimiento.
        moveDir = Vector3.zero;

        // Comprueba si el jugador está intentando moverse.
        if (moving)
        {
            // Busca la cámara principal si todavía no hay una asignada.
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }

            // Guarda el ángulo horizontal de la cámara.
            float cameraY = 0f;

            // Utiliza la rotación de la cámara si está disponible.
            if (cameraTransform != null)
            {
                cameraY = cameraTransform.eulerAngles.y;
            }

            // Calcula hacia dónde debe mirar el Viajero.
            float targetAngle = Mathf.Atan2(
                inputDireccion.x,
                inputDireccion.z
            ) * Mathf.Rad2Deg + cameraY;

            // Suaviza el giro para evitar cambios bruscos de dirección.
            float smoothAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                turnSmoothTime
            );

            // Gira al Viajero hacia la dirección calculada.
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Calcula la dirección de movimiento según la cámara.
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        // Combina el movimiento horizontal con la velocidad vertical.
        Vector3 finalMove = moveDir.normalized * currentSpeed
                          + Vector3.up * verticalVelocity;

        // Mueve al Viajero teniendo en cuenta el tiempo transcurrido.
        characterController.Move(finalMove * Time.deltaTime);
    }


    // =========================
    // SALTO Y GRAVEDAD
    // =========================

    // Controla el salto del Viajero y actualiza la gravedad.
    private void Saltar()
    {
        // Comprueba si el Viajero está tocando el suelo.
        if (characterController.isGrounded)
        {
            // Comprueba si el Viajero tiene velocidad hacia abajo.
            if (verticalVelocity < 0f)
            {
                // Mantiene una pequeña velocidad hacia abajo.
                verticalVelocity = -2f;
            }

            // Comprueba si el jugador presiona la barra espaciadora.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Activa la animación de salto.
                animator.SetTrigger("Jump");

                // Calcula la velocidad necesaria para realizar el salto.
                verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        // Actualiza la velocidad vertical utilizando la gravedad.
        verticalVelocity += gravity * Time.deltaTime;
    }


    // =========================
    // BLOQUEO AL LEVANTARSE
    // =========================

    // Impide que el Viajero se mueva mientras se está levantando.
    private bool BloquearMovimientoSiSeEstaLevantando()
    {
        // Consulta la animación que está reproduciendo el Viajero.
        var state = animator.GetCurrentAnimatorStateInfo(0);

        // Comprueba si está reproduciendo la animación Standing_Up.
        if (state.IsName("Standing_Up"))
        {
            // Detiene los sonidos de los pasos.
            footstep.SetMoving(false);

            // Mantiene la gravedad mientras el movimiento está bloqueado.
            AplicarGravedadSola();

            // Indica que el movimiento debe permanecer bloqueado.
            return true;
        }

        // Permite continuar si el Viajero no se está levantando.
        return false;
    }


    // =========================
    // GRAVEDAD SIN MOVIMIENTO
    // =========================

    // Mantiene la gravedad mientras el Viajero no puede caminar.
    private void AplicarGravedadSola()
    {
        // Comprueba si está en el suelo y tiene velocidad hacia abajo.
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            // Mantiene una pequeña velocidad hacia abajo.
            verticalVelocity = -2f;
        }

        // Actualiza la velocidad vertical utilizando la gravedad.
        verticalVelocity += gravity * Time.deltaTime;

        // Aplica solamente el movimiento vertical.
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }


    // =========================
    // SONIDOS DE PASOS
    // =========================

    // Informa al sistema de pasos si el Viajero está intentando moverse.
    private void DetectarPasos()
    {
        // Envía el estado de movimiento al script FootstepPlayer.
        footstep.SetMoving(moving);
    }


    // =========================
    // CARRERA DEL VIAJERO
    // =========================

    // Comprueba si el Viajero puede correr con la tecla Shift.
    private void Correr()
    {
        // Comprueba si Shift está presionado y queda resistencia.
        bool isRunning = Input.GetKey(KeyCode.LeftShift)
                         && staminaBar.currentStamina > 0;

        // Guarda si el Viajero está corriendo.
        isSprinting = isRunning;
    }


    // =========================
    // CONTROL DE RESISTENCIA
    // =========================

    // Consume o recupera resistencia según las acciones del Viajero.
    private void ManejarStamina()
    {
        // Comprueba si el Viajero está corriendo y moviéndose.
        if (isSprinting && moving)
        {
            // Consume resistencia mientras corre.
            staminaBar.UseStamina(Time.deltaTime);
        }
        // Comprueba si el jugador dejó de presionar Shift.
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            // Recupera resistencia mientras no presiona Shift.
            staminaBar.RecoverStamina(Time.deltaTime);
        }
    }


    // =========================
    // ANIMACIONES DE MOVIMIENTO
    // =========================

    // Actualiza las animaciones según el movimiento del Viajero.
    private void ActualizarAnimacionesMovimiento()
    {
        // Indica al Animator si el Viajero está corriendo.
        animator.SetBool("IsRunning", isSprinting);

        // Indica al Animator si el Viajero está intentando caminar.
        animator.SetBool("IsWalking", moving);
    }


    // =========================
    // CONTROL DEL ARMA
    // =========================

    // Controla las animaciones para sacar y guardar el arma.
    private void ManejarEspada()
    {
        // Comprueba si el jugador presiona la tecla 1.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Activa la animación para sacar el arma.
            animator.SetTrigger("DrawSword");

            // Indica al Animator que el Viajero está armado.
            animator.SetBool("IsArmed", true);
        }

        // Comprueba si el jugador presiona la tecla 2.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Activa la animación para guardar el arma.
            animator.SetTrigger("SheathSword");

            // Indica al Animator que el Viajero ya no está armado.
            animator.SetBool("IsArmed", false);
        }
    }


    // =========================
    // ANIMACIONES DE ATAQUE
    // =========================

    // Activa las animaciones de ataque cuando el Viajero está armado.
    private void Atacar()
    {
        // Comprueba si presiona el clic izquierdo y está armado.
        if (Input.GetMouseButtonDown(0) && animator.GetBool("IsArmed"))
        {
            // Activa la primera animación de ataque.
            animator.SetTrigger("SlashIn");
        }

        // Comprueba si presiona el clic derecho y está armado.
        if (Input.GetMouseButtonDown(1) && animator.GetBool("IsArmed"))
        {
            // Activa la segunda animación de ataque.
            animator.SetTrigger("SlashOut");
        }
    }
}
