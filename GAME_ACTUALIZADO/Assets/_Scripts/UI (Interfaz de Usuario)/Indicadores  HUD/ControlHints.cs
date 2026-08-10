using UnityEngine;

public class ControlHints : MonoBehaviour // Este script muestra un mensaje de control al inicio del juego y lo desvanece después de un tiempo
{
    public CanvasGroup hintGroup; // El grupo de UI que contiene el mensaje de control
    public float displayTime = 5f; // Cuánto tiempo se muestra el mensaje antes de empezar a desvanecerlo
    private float timer; // Temporizador para controlar el tiempo que se muestra el mensaje

    void Start() // Al iniciar, se muestra el mensaje y se establece el temporizador
    {
        timer = displayTime; // Establece el temporizador con el tiempo de visualización definido
        hintGroup.alpha = 1f; // Asegura que el mensaje esté completamente visible al inicio
    }

    void Update() // Cada frame, se cuenta el tiempo y se desvanece el mensaje si el tiempo se ha acabado
    {
        timer -= Time.deltaTime; // Resta el tiempo que ha pasado al temporizador

        if (timer <= 0) // Si el tiempo se ha acabado, empieza a desvanecer el mensaje
        {
            hintGroup.alpha = Mathf.Lerp(hintGroup.alpha, 0f, Time.deltaTime * 2f); // Desvanece el mensaje suavemente hacia 0 (completamente transparente) a una velocidad de 2 unidades por segundo
        }
    }
}
