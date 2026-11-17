
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    // =========================
    // TIPOS DE SUPERFICIE
    // =========================

    // Define los tipos de superficie que puede reconocer el sistema.
    public enum SurfaceType
    {
        Road,   // Camino.
        Grass   // Césped.
    }


    // =========================
    // VARIABLES DE SONIDO
    // =========================

    [Header("Sonidos por superficie")]

    // Guarda los sonidos de pasos que se utilizan sobre el camino.
    public AudioClip[] roadClips;

    // Guarda los sonidos de pasos que se utilizan sobre el césped.
    public AudioClip[] grassClips;


    // =========================
    // VARIABLES DE CONFIGURACIÓN
    // =========================

    [Header("Ajustes de pasos")]

    // Guarda el componente encargado de reproducir los sonidos.
    public AudioSource audioSource;

    // Guarda una referencia opcional al controlador del jugador.
    public PlayerController playerController;

    // Indica cuántos segundos deben pasar entre un sonido de paso y otro.
    public float stepInterval = 0.5f;


    // =========================
    // VARIABLES INTERNAS
    // =========================

    // Guarda el tiempo transcurrido desde el último paso.
    private float stepTimer = 0f;

    // Indica si el jugador se encuentra en movimiento.
    private bool isMoving = false;

    // Guarda los sonidos correspondientes a la superficie actual.
    private AudioClip[] currentClips;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Establece el césped como superficie inicial.
        SetSurface(SurfaceType.Grass);
    }


    // =========================
    // ACTUALIZACIÓN DEL JUEGO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba si el jugador se encuentra en movimiento.
        if (isMoving)
        {
            // Aumenta el temporizador según el tiempo transcurrido.
            stepTimer += Time.deltaTime;

            // Comprueba si llegó el momento de reproducir otro paso.
            if (stepTimer >= stepInterval)
            {
                // Reproduce un sonido de paso.
                PlayFootstep();

                // Reinicia el temporizador para contar el siguiente paso.
                stepTimer = 0f;
            }
        }
    }


    // =========================
    // ESTADO DEL MOVIMIENTO
    // =========================

    // Recibe desde PlayerController si el jugador está en movimiento.
    public void SetMoving(bool moving)
    {
        // Actualiza el estado de movimiento del jugador.
        isMoving = moving;

        // Comprueba si el jugador dejó de moverse.
        if (!moving)
        {
            // Reinicia el temporizador para evitar acumular pasos.
            stepTimer = 0f;
        }
    }


    // =========================
    // CAMBIO DE SUPERFICIE
    // =========================

    // Selecciona los sonidos correspondientes al tipo de superficie.
    public void SetSurface(SurfaceType surface)
    {
        // Comprueba si la superficie seleccionada es un camino.
        if (surface == SurfaceType.Road)
        {
            // Utiliza los sonidos de pasos sobre el camino.
            currentClips = roadClips;
        }
        // Comprueba si la superficie seleccionada es césped.
        else if (surface == SurfaceType.Grass)
        {
            // Utiliza los sonidos de pasos sobre el césped.
            currentClips = grassClips;
        }
        else
        {
            // Evita utilizar sonidos si la superficie no es reconocida.
            currentClips = null;
        }
    }


    // =========================
    // REPRODUCCIÓN DE PASOS
    // =========================

    // Reproduce un sonido aleatorio de la superficie seleccionada.
    private void PlayFootstep()
    {
        // Comprueba que existan sonidos disponibles para reproducir.
        if (currentClips == null || currentClips.Length == 0)
        {
            // Detiene el método si no hay sonidos disponibles.
            return;
        }

        // Selecciona una posición aleatoria dentro de la lista de sonidos.
        int index = Random.Range(0, currentClips.Length);

        // Reproduce una vez el sonido seleccionado.
        audioSource.PlayOneShot(currentClips[index]);
    }
}

