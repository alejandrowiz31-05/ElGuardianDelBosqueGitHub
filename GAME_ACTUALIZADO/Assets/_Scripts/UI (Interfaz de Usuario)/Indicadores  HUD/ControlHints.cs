
using UnityEngine;

public class ControlHints : MonoBehaviour
{
    // =========================
    // VARIABLES DE LA INTERFAZ
    // =========================

    // Guarda el grupo de interfaz que muestra las indicaciones de los controles.
    public CanvasGroup hintGroup;

    // Indica cuántos segundos permanece visible el mensaje.
    public float displayTime = 5f;


    // =========================
    // VARIABLES INTERNAS
    // =========================

    // Guarda el tiempo restante antes de ocultar el mensaje.
    private float timer;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Establece el tiempo inicial de visualización del mensaje.
        timer = displayTime;

        // Muestra completamente el mensaje al comenzar el juego.
        hintGroup.alpha = 1f;
    }


    // =========================
    // ACTUALIZACIÓN DEL JUEGO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Disminuye el tiempo restante según el tiempo transcurrido.
        timer -= Time.deltaTime;

        // Comprueba si terminó el tiempo de visualización del mensaje.
        if (timer <= 0)
        {
            // Reduce suavemente la transparencia hasta ocultar el mensaje.
            hintGroup.alpha = Mathf.Lerp(
                hintGroup.alpha,
                0f,
                Time.deltaTime * 2f
            );
        }
    }
}

