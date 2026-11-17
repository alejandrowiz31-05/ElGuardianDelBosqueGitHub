
using UnityEngine;

public class DanioAlViajero : MonoBehaviour
{
    // =========================
    // VARIABLES
    // =========================

    // Guarda la referencia al panel que controla las vidas del Viajero.
    public PanelVidas panelVidas;


    // =========================
    // ENTRADA DEL JUGADOR
    // =========================

    // Se ejecuta cuando otro objeto entra en la zona de detección.
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba que el objeto que entró sea el jugador.
        if (other.CompareTag("Player"))
        {
            // Le indica al sistema de vidas que debe quitar un corazón.
            panelVidas.PerderVida();

            // Informa en la consola qué objeto le causó daño al Viajero.
            Debug.Log("El Viajero recibió daño de: " + gameObject.name);
        }
    }
}
