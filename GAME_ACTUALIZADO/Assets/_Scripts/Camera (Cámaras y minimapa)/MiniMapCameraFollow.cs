
using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    // =========================
    // REFERENCIAS
    // =========================

    // Guarda la referencia al personaje que seguirá la cámara del minimapa.
    public Transform target;


    // =========================
    // SEGUIMIENTO DEL PERSONAJE
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

        // Obtiene la posición actual del personaje.
        Vector3 newPos = target.position;

        // Mantiene la altura actual de la cámara del minimapa.
        newPos.y = transform.position.y;

        // Actualiza la posición de la cámara para seguir al personaje.
        transform.position = newPos;

        // Gira la cámara según la orientación horizontal del personaje.
        transform.rotation = Quaternion.Euler(
            90f,
            target.eulerAngles.y,
            0f
        );
    }
}

