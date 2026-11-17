
using UnityEngine;

public class MagicFollower : MonoBehaviour
{
    // =========================
    // VARIABLES DE SEGUIMIENTO
    // =========================

    [Header("Configuración de Seguimiento")]

    // Guarda la referencia al objeto que seguirá la Esfera Dorada.
    public Transform target;

    // Indica la velocidad con la que la esfera sigue al jugador.
    public float followSpeed = 2.0f;

    // Indica la distancia que la esfera mantiene respecto al jugador.
    public float stopDistance = 2.5f;

    // Indica la altura a la que flota la esfera respecto al jugador.
    public float floatingHeight = 1.8f;


    // =========================
    // VARIABLES DE EXPLORACIÓN
    // =========================

    [Header("Comportamiento Curioso (Vida)")]

    // Indica el radio en el que la esfera puede elegir un punto para explorar.
    public float curioRadius = 5.0f;

    // Indica cuántos segundos permanece siguiendo antes de cambiar de estado.
    public float changeInterval = 7.0f;


    // =========================
    // VARIABLES INTERNAS
    // =========================

    // Guarda la posición hacia la que debe moverse la esfera.
    private Vector3 targetPos;

    // Guarda la posición elegida para explorar alrededor del jugador.
    private Vector3 curioPos;

    // Indica si la esfera está explorando o siguiendo al jugador.
    private bool isCurious = false;

    // Guarda el tiempo restante antes de cambiar de estado.
    private float timer;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Establece el tiempo inicial antes de cambiar de estado.
        timer = changeInterval;
    }


    // =========================
    // ACTUALIZACIÓN DEL JUEGO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba que exista un objetivo antes de continuar.
        if (target == null)
        {
            return;
        }

        // Actualiza el tiempo para cambiar entre seguir y explorar.
        ActualizarTemporizadorDeEstado();

        // Calcula la posición hacia la que debe moverse la esfera.
        CalcularPosicionObjetivo();

        // Mueve la esfera hacia la posición calculada.
        MoverHaciaPosicionObjetivo();

        // Orienta la esfera hacia su objetivo de movimiento.
        OrientarHaciaObjetivo();

        // Aplica un movimiento suave de flotación.
        AplicarFlotacionSuave();
    }


    // =========================
    // CAMBIO DE COMPORTAMIENTO
    // =========================

    // Controla el tiempo para alternar entre seguir y explorar.
    private void ActualizarTemporizadorDeEstado()
    {
        // Disminuye el tiempo restante según el tiempo transcurrido.
        timer -= Time.deltaTime;

        // Comprueba si terminó el tiempo del estado actual.
        if (timer <= 0)
        {
            // Cambia entre el estado de exploración y el de seguimiento.
            isCurious = !isCurious;

            // Establece un tiempo más corto cuando la esfera está explorando.
            if (isCurious)
            {
                timer = 3.0f;
            }
            else
            {
                // Recupera el tiempo configurado cuando vuelve a seguir.
                timer = changeInterval;
            }

            // Comprueba si la esfera debe elegir un punto para explorar.
            if (isCurious)
            {
                // Selecciona una nueva posición alrededor del jugador.
                ElegirPuntoDeHusmeo();
            }
        }
    }


    // =========================
    // ELECCIÓN DEL PUNTO DE EXPLORACIÓN
    // =========================

    // Elige una posición aleatoria para explorar alrededor del jugador.
    private void ElegirPuntoDeHusmeo()
    {
        // Genera un desplazamiento aleatorio dentro del radio configurado.
        Vector3 randomOffset = Random.insideUnitSphere * curioRadius;

        // Calcula una posición de exploración alrededor del jugador.
        curioPos = target.position + randomOffset;

        // Coloca el punto de exploración un poco más alto que la flotación normal.
        curioPos.y = target.position.y + floatingHeight + 1f;
    }


    // =========================
    // CÁLCULO DE LA POSICIÓN
    // =========================

    // Calcula hacia dónde debe moverse la esfera según su estado.
    private void CalcularPosicionObjetivo()
    {
        // Comprueba si la esfera se encuentra explorando.
        if (isCurious)
        {
            // Utiliza la posición elegida para explorar.
            targetPos = curioPos;
        }
        else
        {
            // Calcula una posición cercana al jugador respetando la distancia.
            targetPos = target.position
                + (transform.position - target.position).normalized * stopDistance;

            // Ajusta la altura de seguimiento respecto al jugador.
            targetPos.y = target.position.y + floatingHeight;
        }
    }


    // =========================
    // MOVIMIENTO DE LA ESFERA
    // =========================

    // Mueve suavemente la esfera hacia la posición calculada.
    private void MoverHaciaPosicionObjetivo()
    {
        // Guarda la velocidad que utilizará la esfera.
        float velocidadActual;

        // Comprueba si la esfera se encuentra explorando.
        if (isCurious)
        {
            // Utiliza una velocidad más lenta durante la exploración.
            velocidadActual = 1.0f;
        }
        else
        {
            // Utiliza la velocidad configurada durante el seguimiento.
            velocidadActual = followSpeed;
        }

        // Desplaza suavemente la esfera hacia su posición objetivo.
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * velocidadActual
        );
    }


    // =========================
    // ORIENTACIÓN DE LA ESFERA
    // =========================

    // Orienta la esfera hacia la dirección de su movimiento.
    private void OrientarHaciaObjetivo()
    {
        // Gira la esfera hacia el punto calculado delante de su orientación.
        transform.LookAt(targetPos + transform.forward);
    }


    // =========================
    // FLOTACIÓN DE LA ESFERA
    // =========================

    // Aplica un movimiento vertical suave para simular flotación.
    private void AplicarFlotacionSuave()
    {
        // Añade un pequeño desplazamiento vertical basado en el tiempo.
        transform.position += new Vector3(
            0,
            Mathf.Sin(Time.time) * 0.2f * Time.deltaTime,
            0
        );
    }
}
