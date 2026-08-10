using UnityEngine;

/// <summary>
/// Indicador flotante que sigue a un objetivo y mira hacia la cámara (billboard).
/// Optimizado para rendimiento: cacheo de componentes, validaciones robustas.
/// </summary>
public class FloatingIndicator : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Objeto al que el indicador seguirá.")]
    public Transform target;

    [Header("Movimiento")]
    [Tooltip("Desplazamiento desde el objetivo.")]
    public Vector3 offset = new Vector3(0, 2, 0);

    [Tooltip("Amplitud del movimiento de flotación.")]
    public float floatAmplitude = 0.2f;

    [Tooltip("Velocidad del movimiento de flotación.")]
    public float floatSpeed = 2f;

    // Componentes cacheados
    private Camera mainCamera;
    private Transform cameraTransform;

    void Awake()
    {
        // Cachear referencias al inicializar (evita búsquedas repetidas)
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning($"FloatingIndicator en '{name}': cámara principal no encontrada. El indicador no rotará hacia la cámara.");
            enabled = false;
            return;
        }

        cameraTransform = mainCamera.transform;
    }

    void LateUpdate()
    {
        // Validar que el objetivo exista
        if (target == null)
        {
            return;
        }

        // Calcular posición base con desplazamiento
        Vector3 basePosition = target.position + offset;

        // Aplicar flotación suave mediante función seno
        float floatY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        Vector3 finalPosition = basePosition + new Vector3(0, floatY, 0);

        // Actualizar posición del indicador
        transform.position = finalPosition;

        // Billboard: rotación solo en eje Y hacia la cámara (evita inclinaciones)
        RotateTowardCamera();
    }

    /// <summary>
    /// Rota el indicador para mirar hacia la cámara, solo sobre el eje Y.
    /// </summary>
    private void RotateTowardCamera()
    {
        // Calcular dirección hacia la cámara
        Vector3 directionToCamera = cameraTransform.position - transform.position;
        directionToCamera.y = 0f; // Ignorar eje Y para evitar inclinación

        // Validar que el vector no sea casi cero (evita errores en LookRotation)
        if (directionToCamera.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = targetRotation;
        }
    }
}