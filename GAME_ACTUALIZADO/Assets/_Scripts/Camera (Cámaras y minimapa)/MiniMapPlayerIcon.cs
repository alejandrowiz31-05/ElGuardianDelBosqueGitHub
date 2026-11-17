
using UnityEngine;

public class MiniMapPlayerIcon : MonoBehaviour
{
    // =========================
    // REFERENCIAS
    // =========================

    // Guarda la referencia al personaje que representa el ícono del minimapa.
    public Transform player;


    // =========================
    // ACTUALIZACIÓN DEL ÍCONO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba si existe el personaje asignado.
        if (player == null)
        {
            // Detiene el método si no hay un personaje asignado.
            return;
        }

        // Gira el ícono según la orientación horizontal del personaje.
        transform.rotation = Quaternion.Euler(
            0f,
            0f,
            -player.eulerAngles.y
        );
    }
}

