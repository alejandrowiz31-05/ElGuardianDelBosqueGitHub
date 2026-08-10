using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;            // El jugador
    public float distance = 4f;         // Qué tan lejos está la cámara
    public float height = 2f;           // Altura del pivot
    public float sensibilidad = 2f;     // Sensibilidad del mouse

    private float rotationX = 0f;       // Rotación vertical
    private float rotationY = 0f;       // Rotación horizontal

    void Start()
    {
        // Guardar rotación inicial del pivot
        rotationY = transform.eulerAngles.y;
        rotationX = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Obtener entrada de la cámara
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        // 2. Acumular esos movimientos en mis dos variables de rotación
        rotationX -= mouseY;                          // arriba/abajo
        rotationX = Mathf.Clamp(rotationX, -60f, 60f); // limitar para no dar la vuelta completa
        rotationY += mouseX;                          // izquierda/derecha (gira libre)

        // 3. Posicionar este objeto (el "pivot") a la altura de la cabeza del jugador
        transform.position = target.position + Vector3.up * height;

        // 4. Aplicar la rotación completa (horizontal + vertical) al pivot
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        // 5. Poner la cámara detrás del pivot, a la distancia indicada
        Camera.main.transform.position = transform.position - transform.forward * distance;

        // 6. Hacer que la cámara siempre mire hacia el jugador
        Camera.main.transform.LookAt(target.position + Vector3.up * height);
    }
}


