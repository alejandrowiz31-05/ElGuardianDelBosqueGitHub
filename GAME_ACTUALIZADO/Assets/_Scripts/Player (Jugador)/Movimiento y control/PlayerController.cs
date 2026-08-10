using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ---------------- VARIABLES PÚBLICAS ----------------
    public float runSpeed = 6f;        // Velocidad al correr
    public bool useInput = false;      // true = usa Input.GetAxisRaw, false = Input.GetAxis
    public bool isSprinting;           // bandera para saber si corre (útil para sonidos)

    // ---------------- VARIABLES PRIVADAS ----------------
    private Rigidbody rb;              // componente Rigidbody para física
    private Animator animator;         // controla animaciones
    private FootstepPlayer footstep;   // controla sonidos de pasos
    private float horizontal;          // input horizontal
    private float vertical;            // input vertical

    // Variables para mejorar el salto
    private float lastGroundedTime;    // última vez que estuvo en el suelo (coyote time)
    private bool isJumping;            // si está en animación de salto

    // ---------------- MÉTODOS ----------------

    public bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift); // true si se mantiene Shift
    }

    void Start()
    {
        footstep = GetComponent<FootstepPlayer>(); // busca el script de pasos en el mismo GameObject
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();           // obtiene Rigidbody
        animator = GetComponent<Animator>();      // obtiene Animator
    }

    void Update()
    {
        // 1. BLOQUEAR MOVIMIENTO SI ESTÁ LEVANTÁNDOSE
        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Standing_Up"))
        {
            footstep.SetMoving(false); // no reproducir pasos
            return;                    // salir: no procesamos movimiento
        }

        // 2. DETECTAR MOVIMIENTO
        bool moving = (horizontal != 0 || vertical != 0);
        footstep.SetMoving(moving); // activar/desactivar sonido de pasos

        // 3. CORRER
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        isSprinting = isRunning;

        // 4. ANIMACIONES DE CAMINAR Y CORRER
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsWalking", moving);

        // 5. SALTO (con coyote time)
        if (!isJumping && Time.time - lastGroundedTime <= 0.12f && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");  // activar animación de salto
            isJumping = true;             // marcar que está saltando
        }

        // 6. COMBATE (animaciones de espada/daga)
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
        if (Input.GetMouseButtonDown(0) && animator.GetBool("IsArmed"))
        {
            animator.SetTrigger("SlashIn");
        }
        if (Input.GetMouseButtonDown(1) && animator.GetBool("IsArmed"))
        {
            animator.SetTrigger("SlashOut");
        }
    }

    private void FixedUpdate()
    {
        // 7. MOVIMIENTO DEL JUGADOR
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        rb.AddForce(direction);
    }
}