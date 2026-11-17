
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    // =========================
    // REFERENCIAS
    // =========================

    // Guarda la referencia al personaje que seguirá la cámara.
    public Transform target;


    // =========================
    // VARIABLES DE LA CÁMARA
    // =========================

    // Indica la distancia entre la cámara y el punto de seguimiento.
    public float distance = 4f;

    // Indica la altura del punto de seguimiento respecto al personaje.
    public float height = 2f;

    // Indica la sensibilidad del movimiento del mouse.
    public float sensibilidad = 2f;


    // =========================
    // VARIABLES INTERNAS
    // =========================

    // Guarda la rotación vertical de la cámara.
    private float rotationX = 0f;

    // Guarda la rotación horizontal de la cámara.
    private float rotationY = 0f;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Guarda la rotación horizontal inicial del punto de seguimiento.
        rotationY = transform.eulerAngles.y;

        // Guarda la rotación vertical inicial del punto de seguimiento.
        rotationX = transform.eulerAngles.x;
    }


    // =========================
    // ACTUALIZACIÓN DE LA CÁMARA
    // =========================

    // Se ejecuta al final de cada fotograma del juego.
    private void LateUpdate()
    {
        // Comprueba si existe el personaje que debe seguir la cámara.
        if (target == null)
        {
            // Detiene el método si no hay un personaje asignado.
            return;
        }

        // Obtiene el movimiento horizontal del mouse.
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;

        // Obtiene el movimiento vertical del mouse.
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        // Actualiza la rotación vertical según el movimiento del mouse.
        rotationX -= mouseY;

        // Limita la rotación vertical para evitar que la cámara dé una vuelta completa.
        rotationX = Mathf.Clamp(rotationX, -60f, 60f);

        // Actualiza la rotación horizontal según el movimiento del mouse.
        rotationY += mouseX;

        // Coloca el punto de seguimiento a la altura indicada sobre el personaje.
        transform.position = target.position + Vector3.up * height;

        // Aplica la rotación vertical y horizontal al punto de seguimiento.
        transform.rotation = Quaternion.Euler(
            rotationX,
            rotationY,
            0f
        );

        // Coloca la cámara principal detrás del punto de seguimiento.
        Camera.main.transform.position =
            transform.position - transform.forward * distance;

        // Orienta la cámara principal hacia el punto de seguimiento del personaje.
        Camera.main.transform.LookAt(
            target.position + Vector3.up * height
        );
    }
}


