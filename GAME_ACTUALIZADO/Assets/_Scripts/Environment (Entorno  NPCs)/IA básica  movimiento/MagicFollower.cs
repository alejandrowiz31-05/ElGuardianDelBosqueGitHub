using UnityEngine;

public class MagicFollower : MonoBehaviour // Un seguidor mágico que alterna entre seguir al jugador y husmear alrededor de él
{
    [Header("Configuración de Seguimiento")] // Configuración para el comportamiento de seguimiento
    public Transform target; // El jugador al que el seguidor seguirá
    public float followSpeed = 2.0f; // Velocidad de seguimiento
    public float stopDistance = 2.5f; // Qué tan cerca se queda del jugador antes de detenerse
    public float floatingHeight = 1.8f; // Altura a la que flota el seguidor sobre el jugador

    [Header("Comportamiento Curioso (Vida)")] // Configuración para el comportamiento curioso, donde el seguidor husmea alrededor del jugador
    public float curioRadius = 5.0f; // Radio dentro del cual el seguidor elige un punto aleatorio para husmear alrededor del jugador
    public float changeInterval = 7.0f; // Intervalo de tiempo para alternar entre seguir y husmear (en segundos)

    private Vector3 targetPos; // Posición objetivo actual a la que el seguidor se moverá
    private Vector3 curioPos; // Posición aleatoria para husmear alrededor del jugador cuando está en modo curioso
    private bool isCurious = false; // Estado actual del seguidor: si es curioso (husmeando) o no (siguiendo)
    private float timer; // Temporizador para controlar el cambio de estado entre seguir y husmear

    void Start() // Inicializar el temporizador para el cambio de estado
    {
        timer = changeInterval; // Comenzar con el temporizador para alternar entre seguir y husmear
    }

    void Update() // Actualizar el comportamiento del seguidor cada frame
    {
        if (target == null) return; // Si no hay objetivo, no hacer nada

        timer -= Time.deltaTime; // Reducir el temporizador cada frame

        // Controlar el cambio de estado entre seguir y husmear basado en el temporizador
        if (timer <= 0) // Si el temporizador ha llegado a cero, cambiar de estado
        {
            isCurious = !isCurious; // Alternar entre seguir y husmear
            timer = isCurious ? 3.0f : changeInterval; // Si se vuelve curioso, el próximo cambio será más rápido (3 segundos), si vuelve a seguir, el próximo cambio será más lento (changeInterval)

            if (isCurious) // Si el seguidor se vuelve curioso, elegir un nuevo punto aleatorio para husmear alrededor del jugador
            {
                // Generar un nuevo punto aleatorio dentro de una esfera alrededor del jugador para husmear
                Vector3 randomOffset = Random.insideUnitSphere * curioRadius; // Genera un desplazamiento aleatorio dentro de una esfera con el radio definido por curioRadius
                curioPos = target.position + randomOffset; // Calcula la posición objetivo para husmear sumando el desplazamiento aleatorio a la posición del jugador
                curioPos.y = target.position.y + floatingHeight + 1f; // Asegura que el seguidor esté a una altura adecuada para husmear, un poco más alto que la altura de flotación normal
            }
        }

        // Determinar la posición objetivo a la que el seguidor se moverá según su estado actual (curioso o no)
        if (isCurious) // Si el seguidor es curioso, moverse hacia la posición de husmear
        {
            targetPos = curioPos; // La posición objetivo es la posición aleatoria para husmear alrededor del jugador
        }
        else // Si el seguidor no es curioso, seguir al jugador pero mantener una distancia de stopDistance
        {
            targetPos = target.position + (transform.position - target.position).normalized * stopDistance; // Calcula la posición objetivo para seguir al jugador manteniendo una distancia de stopDistance, moviéndose en la dirección desde el jugador hacia el seguidor
            targetPos.y = target.position.y + floatingHeight; // Asegura que el seguidor esté a la altura de flotación definida por floatingHeight sobre el jugador
        }

        // Movimiento suave hacia la posición objetivo
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * (isCurious ? 1.0f : followSpeed)); // Si el seguidor es curioso, usar una velocidad de movimiento más lenta (1.0f) para un movimiento más suave al husmear, si no es curioso, usar la velocidad de seguimiento definida por followSpeed

        // Mirar hacia la posición objetivo para mantener una orientación natural
        transform.LookAt(targetPos + transform.forward); // Hacer que el seguidor mire hacia la posición objetivo, pero solo en el eje Y para evitar rotaciones extrañas

        // Agregar un movimiento de flotación suave para darle vida al seguidor, incluso cuando está husmeando
        transform.position += new Vector3(0, Mathf.Sin(Time.time) * 0.2f * Time.deltaTime, 0); // Agrega un movimiento de flotación suave usando una función seno para un movimiento natural, multiplicado por Time.deltaTime para mantenerlo suave independientemente de la velocidad de fotogramas
    }
}