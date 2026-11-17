
using UnityEngine;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    // =========================
    // VARIABLES
    // =========================

    // Guarda el componente que controla las animaciones de Miska.
    private Animator animator;

    // Guarda el componente que permite a Miska moverse por el escenario.
    private NavMeshAgent agent;


    // =========================
    // CONFIGURACIÓN DEL MOVIMIENTO
    // =========================

    [Header("Movimiento autónomo")]

    // Velocidad con la que Miska camina por el bosque.
    public float walkSpeed = 1.5f;

    // Distancia máxima utilizada para buscar nuevos destinos.
    public float wanderRadius = 10f;

    // Tiempo de espera antes de buscar otro destino.
    public float idleTime = 3f;


    // =========================
    // TEMPORIZADORES Y ESTADO
    // =========================

    // Cuenta el tiempo de espera entre los recorridos de Miska.
    private float idleTimer;

    // Cuenta el tiempo para reproducir una animación especial.
    private float specialTimer;

    // Define los dos estados principales de animación de Miska.
    private enum AnimState
    {
        Idle,
        Walking
    }

    // Guarda el estado de animación actual.
    private AnimState currentState = AnimState.Idle;


    // =========================
    // PREPARACIÓN DE COMPONENTES
    // =========================

    // Se ejecuta cuando Unity carga el objeto que tiene este script.
    private void Awake()
    {
        // Busca el componente Animator en el objeto.
        animator = GetComponent<Animator>();

        // Busca el componente NavMeshAgent en el objeto.
        agent = GetComponent<NavMeshAgent>();
    }


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Comprueba que Miska tenga el componente de navegación.
        if (agent == null)
        {
            // Muestra una advertencia si falta el componente.
            Debug.LogWarning("AnimalController: falta NavMeshAgent en " + name + ". Desactivando script.");

            // Desactiva este script para evitar errores de movimiento.
            enabled = false;
            return;
        }

        // Comprueba que Miska tenga el componente de animaciones.
        if (animator == null)
        {
            // Muestra una advertencia si falta el Animator.
            Debug.LogWarning("AnimalController: falta Animator en " + name + ". Las animaciones no se actualizarán.");
        }

        // Establece el tiempo inicial de espera.
        idleTimer = idleTime;

        // Elige un tiempo aleatorio para las animaciones especiales.
        specialTimer = Random.Range(5f, 10f);

        // Actualiza la velocidad si es diferente a la configurada.
        if (!Mathf.Approximately(agent.speed, walkSpeed))
        {
            agent.speed = walkSpeed;
        }

        // Busca el primer destino para Miska.
        WanderToNewPoint();

        // Solicita la animación de caminar.
        SetAnimState(AnimState.Walking);
    }


    // =========================
    // ACTUALIZACIÓN DEL MOVIMIENTO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Espera mientras Unity calcula la ruta de navegación.
        if (agent.pathPending)
        {
            return;
        }

        // Revisa si Miska debe caminar o quedarse esperando.
        ManejarComportamiento();
    }


    // =========================
    // COMPORTAMIENTO DE MISKA
    // =========================

    // Controla cuándo Miska camina, espera o realiza una animación especial.
    private void ManejarComportamiento()
    {
        // Comprueba si Miska llegó a su destino o está cerca de él.
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Reduce los temporizadores según el tiempo transcurrido.
            idleTimer -= Time.deltaTime;
            specialTimer -= Time.deltaTime;

            // Comprueba si terminó el tiempo de espera.
            if (idleTimer <= 0f)
            {
                // Busca un nuevo destino para seguir caminando.
                WanderToNewPoint();

                // Reinicia los tiempos de espera y animaciones.
                idleTimer = idleTime;
                specialTimer = Random.Range(5f, 10f);

                // Solicita la animación de caminar.
                SetAnimState(AnimState.Walking);
            }
            else
            {
                // Solicita la animación de reposo.
                SetAnimState(AnimState.Idle);

                // Comprueba si llegó el momento de una animación especial.
                if (specialTimer <= 0f)
                {
                    // Reproduce una animación especial al azar.
                    PlayRandomSpecial();

                    // Reinicia el tiempo para la siguiente animación.
                    specialTimer = Random.Range(5f, 10f);
                }
            }
        }
        else
        {
            // Mantiene la animación de caminar mientras se desplaza.
            SetAnimState(AnimState.Walking);
        }
    }


    // =========================
    // CONTROL DE ANIMACIONES
    // =========================

    // Cambia las animaciones principales cuando el estado es diferente.
    private void SetAnimState(AnimState newState)
    {
        // Sale del método si Miska no tiene Animator.
        if (animator == null)
        {
            return;
        }

        // Evita repetir el cambio si ya está en ese estado.
        if (currentState == newState)
        {
            return;
        }

        // Guarda el nuevo estado de animación.
        currentState = newState;

        // Comprueba si Miska debe estar en reposo o caminando.
        bool isIdle = newState == AnimState.Idle;
        bool isWalking = newState == AnimState.Walking;

        // Actualiza los parámetros del Animator.
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalking", isWalking);

        // Mantiene desactivada la animación de correr.
        animator.SetBool("isRunning", false);
    }


    // =========================
    // BÚSQUEDA DE NUEVOS DESTINOS
    // =========================

    // Busca un punto aleatorio para que Miska camine por el bosque.
    private void WanderToNewPoint()
    {
        // Genera una dirección aleatoria dentro del radio configurado.
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;

        // Calcula la posición tomando como referencia a Miska.
        randomDirection += transform.position;

        // Guarda la información del punto encontrado en el NavMesh.
        NavMeshHit hit;

        // Busca un punto válido dentro de la zona de navegación.
        bool found = NavMesh.SamplePosition(
            randomDirection,
            out hit,
            wanderRadius,
            NavMesh.AllAreas
        );

        // Guarda la posición que Miska intentará alcanzar.
        Vector3 finalPosition;

        // Comprueba si Unity encontró un punto válido en el NavMesh.
        if (found)
        {
            // Utiliza el punto encontrado en la zona de navegación.
            finalPosition = hit.position;
        }
        else
        {
            // Conserva la posición aleatoria si no encuentra un punto válido.
            finalPosition = randomDirection;
        }

        // Actualiza la velocidad si es diferente a la configurada.
        if (!Mathf.Approximately(agent.speed, walkSpeed))
        {
            agent.speed = walkSpeed;
        }

        // Le indica a Miska hacia dónde debe caminar.
        agent.SetDestination(finalPosition);
    }


    // =========================
    // ANIMACIONES ESPECIALES
    // =========================

    // Reproduce una animación aleatoria cuando Miska está esperando.
    private void PlayRandomSpecial()
    {
        // Sale del método si Miska no tiene Animator.
        if (animator == null)
        {
            return;
        }

        // Genera un número aleatorio entre 0 y 1.
        float roll = Random.value;

        // Tiene un 50 % de probabilidad de sentarse.
        if (roll < 0.5f)
        {
            animator.SetTrigger("Sit");
        }
        // Tiene un 30 % de probabilidad de hacer la animación Attack.
        else if (roll < 0.8f)
        {
            animator.SetTrigger("Attack");
        }
        // Tiene un 20 % de probabilidad de saltar.
        else
        {
            animator.SetTrigger("Jump");
        }
    }
}
