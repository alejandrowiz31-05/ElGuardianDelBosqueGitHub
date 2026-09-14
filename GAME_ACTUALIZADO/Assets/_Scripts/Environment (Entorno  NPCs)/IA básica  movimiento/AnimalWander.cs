using UnityEngine;
using UnityEngine.AI;
 
// Controlador simple para comportamiento de deambular (wander) de un animal.
// Conserva la lógica original; solo se agrupa el bloque de decisión de Update()
// en un método propio para mantener la misma organización del resto del proyecto.
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
 
 
    // =========================
    // INICIO
    // =========================
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
 
    void Start()
    {
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
 
        idleTimer = idleTime;
        specialTimer = Random.Range(5f, 10f);
 
        if (!Mathf.Approximately(agent.speed, walkSpeed)) agent.speed = walkSpeed;
 
        WanderToNewPoint();
        SetAnimState(AnimState.Walking);
    }
 
 
    // =========================
    // UPDATE — orquesta el frame
    // =========================
    void Update()
    {
        if (agent.pathPending) return;
 
        ManejarComportamiento();
    }
 
 
    // =========================
    // MÉTODO: ManejarComportamiento
    // Decide si Miska debe caminar hacia un nuevo punto, quedarse en idle,
    // o reproducir una animación especial mientras espera
    // =========================
    private void ManejarComportamiento()
    {
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
                SetAnimState(AnimState.Idle);
 
                if (specialTimer <= 0f)
                {
                    PlayRandomSpecial();
                    specialTimer = Random.Range(5f, 10f);
                }
            }
        }
        else
        {
            SetAnimState(AnimState.Walking);
        }
    }
 
 
    // =========================
    // MÉTODO: SetAnimState
    // Cambia el estado del Animator sólo cuando es necesario
    // =========================
    void SetAnimState(AnimState newState)
    {
        if (animator == null) return;
        if (currentState == newState) return;
 
        currentState = newState;
 
        bool isIdle = newState == AnimState.Idle;
        bool isWalking = newState == AnimState.Walking;
 
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", false); // conservado por compatibilidad
    }
 
 
    // =========================
    // MÉTODO: WanderToNewPoint
    // Elige un punto aleatorio dentro del radio de deambulación y mueve a Miska hacia allí
    // =========================
    void WanderToNewPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
 
        NavMeshHit hit;
        bool found = NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas);
 
        Vector3 finalPosition = found ? hit.position : randomDirection;
 
        if (!Mathf.Approximately(agent.speed, walkSpeed)) agent.speed = walkSpeed;
 
        agent.SetDestination(finalPosition);
    }
 
 
    // =========================
    // MÉTODO: PlayRandomSpecial
    // Reproduce una animación especial aleatoria mientras Miska está en idle
    // (Sit 50%, Attack 30%, Jump 20%)
    // =========================
    void PlayRandomSpecial()
    {
        if (animator == null) return;
 
        float roll = Random.value;
 
        if (roll < 0.5f) animator.SetTrigger("Sit");
        else if (roll < 0.8f) animator.SetTrigger("Attack");
        else animator.SetTrigger("Jump");
    }
}