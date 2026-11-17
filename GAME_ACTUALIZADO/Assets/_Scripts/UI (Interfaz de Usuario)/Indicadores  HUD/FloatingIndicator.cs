
using UnityEngine;

public class FloatingIndicator : MonoBehaviour
{
    // =========================
    // REFERENCIAS
    // =========================

    [Header("Referencias")]

    // Guarda la referencia al objeto que seguirá el indicador.
    [Tooltip("Objeto al que el indicador seguirá.")]
    public Transform target;


    // =========================
    // VARIABLES DE MOVIMIENTO
    // =========================

    [Header("Movimiento")]

    // Indica la distancia entre el indicador y el objeto que sigue.
    [Tooltip("Desplazamiento desde el objetivo.")]
    public Vector3 offset = new Vector3(0, 2, 0);

    // Indica cuánto sube y baja el indicador durante la flotación.
    [Tooltip("Amplitud del movimiento de flotación.")]
    public float floatAmplitude = 0.2f;

    // Indica la velocidad del movimiento de flotación.
    [Tooltip("Velocidad del movimiento de flotación.")]
    public float floatSpeed = 2f;


    // =========================
    // VARIABLES INTERNAS
    // =========================

    // Guarda la referencia a la cámara principal del juego.
    private Camera mainCamera;

    // Guarda la referencia a la posición y orientación de la cámara.
    private Transform cameraTransform;


    // =========================
    // INICIALIZACIÓN
    // =========================

    // Se ejecuta una vez cuando se inicializa el objeto.
    private void Awake()
    {
        // Busca y guarda la cámara principal del juego.
        mainCamera = Camera.main;

        // Comprueba si se encontró una cámara principal.
        if (mainCamera == null)
        {
            // Muestra una advertencia si no existe una cámara principal.
            Debug.LogWarning(
                $"FloatingIndicator en '{name}': cámara principal no encontrada. El indicador no rotará hacia la cámara."
            );

            // Desactiva el script si no se encuentra la cámara.
            enabled = false;

            // Detiene el método para evitar errores.
            return;
        }

        // Guarda la referencia al Transform de la cámara.
        cameraTransform = mainCamera.transform;
    }


    // =========================
    // ACTUALIZACIÓN DEL INDICADOR
    // =========================

    // Se ejecuta al final de cada fotograma del juego.
    private void LateUpdate()
    {
        // Comprueba que exista el objeto que debe seguir el indicador.
        if (target == null)
        {
            // Detiene el método si no hay un objetivo asignado.
            return;
        }

        // Calcula la posición del indicador respecto al objeto que sigue.
        Vector3 basePosition = target.position + offset;

        // Calcula el movimiento vertical de flotación.
        float floatY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Añade el movimiento de flotación a la posición calculada.
        Vector3 finalPosition = basePosition + new Vector3(0, floatY, 0);

        // Actualiza la posición del indicador.
        transform.position = finalPosition;

        // Orienta el indicador hacia la cámara del jugador.
        RotateTowardCamera();
    }


    // =========================
    // ORIENTACIÓN HACIA LA CÁMARA
    // =========================

    // Gira el indicador hacia la cámara utilizando solamente el eje Y.
    private void RotateTowardCamera()
    {
        // Calcula la dirección desde el indicador hacia la cámara.
        Vector3 directionToCamera =
            cameraTransform.position - transform.position;

        // Evita que el indicador se incline hacia arriba o hacia abajo.
        directionToCamera.y = 0f;

        // Comprueba que exista una dirección válida para realizar el giro.
        if (directionToCamera.sqrMagnitude > 0.001f)
        {
            // Calcula la rotación necesaria para mirar hacia la cámara.
            Quaternion targetRotation =
                Quaternion.LookRotation(directionToCamera);

            // Aplica la rotación calculada al indicador.
            transform.rotation = targetRotation;
        }
    }
}
