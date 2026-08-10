using UnityEngine;
using UnityEngine.AI;

// Controlador simple para comportamiento de deambular (wander) de un animal.
// Mejoras realizadas: caching de componentes, reducción de llamadas repetidas a Animator
// y seguridad ante referencias nulas. Conserva la lógica original.
public class AnimalController : MonoBehaviour
{
    // Componentes cacheados
    Animator animator;
    NavMeshAgent agent;

    [Header("Movimiento autónomo")]
    public float walkSpeed = 1.5f;
    public float wanderRadius = 10f;
    public float idleTime = 3f;

    // Temporizadores
    float idleTimer;
    float specialTimer;

    // Estado simple para evitar setear parámetros del Animator cada frame
    enum AnimState { Idle, Walking }
    AnimState currentState = AnimState.Idle;

    void Awake()
    {
        // Cachear referencias al inicio para evitar GetComponent repetidos
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Validaciones: si faltan componentes, desactivar para no lanzar NullReference
        if (agent == null)
        {
            Debug.LogWarning("AnimalController: falta NavMeshAgent en " + name + ". Desactivando script.");
            enabled = false;
            return;
        }

        if (animator == null)
        {
            Debug.LogWarning("AnimalController: falta Animator en " + name + ". Las animaciones no se actualizarán.");
        }

        // Inicializar valores
        idleTimer = idleTime;
        specialTimer = Random.Range(5f, 10f);

        // Asignar velocidad sólo si es diferente (reduce escrituras innecesarias)
        if (!Mathf.Approximately(agent.speed, walkSpeed)) agent.speed = walkSpeed;

        // Empezar a deambular
        WanderToNewPoint();
        SetAnimState(AnimState.Walking);
    }

    void Update()
    {
        // Si el agente está calculando ruta o no tiene destino, esperar
        if (agent.pathPending) return;

        // Si hemos llegado al destino (o no hay uno válido)
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            idleTimer -= Time.deltaTime;
            specialTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {
                WanderToNewPoint();
                idleTimer = idleTime;
                specialTimer = Random.Range(5f, 10f);
                SetAnimState(AnimState.Walking);
            }
            else
            {
                // Mantener idle
                SetAnimState(AnimState.Idle);

                // Animaciones especiales automáticas - se disparan sólo cuando está idle
                if (specialTimer <= 0f)
                {
                    PlayRandomSpecial();
                    specialTimer = Random.Range(5f, 10f);
                }
            }
        }
        else
        {
            // Si el agente se está moviendo, marcar walking
            SetAnimState(AnimState.Walking);
        }
    }

    // Cambia el estado del Animator sólo cuando sea necesario
    void SetAnimState(AnimState newState)
    {
        if (animator == null) return;
        if (currentState == newState) return;

        currentState = newState;

        // Simplificar: sólo dos estados manejados aquí. Mantener los parámetros consistentes.
        bool isIdle = newState == AnimState.Idle;
        bool isWalking = newState == AnimState.Walking;

        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", false); // conservado por compatibilidad
    }

    void WanderToNewPoint()
    {
        // Generar un punto aleatorio dentro de un radio y muestrear en la NavMesh
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        bool found = NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas);

        Vector3 finalPosition = found ? hit.position : randomDirection;

        // Actualizar velocidad sólo si es necesario
        if (!Mathf.Approximately(agent.speed, walkSpeed)) agent.speed = walkSpeed;

        agent.SetDestination(finalPosition);
    }

    void PlayRandomSpecial()
    {
        if (animator == null) return;

        float roll = Random.value; // número entre 0 y 1

        // Probabilidades: Sit 50%, Attack 30%, Jump 20%
        if (roll < 0.5f) animator.SetTrigger("Sit");
        else if (roll < 0.8f) animator.SetTrigger("Attack");
        else animator.SetTrigger("Jump");
    }
}
