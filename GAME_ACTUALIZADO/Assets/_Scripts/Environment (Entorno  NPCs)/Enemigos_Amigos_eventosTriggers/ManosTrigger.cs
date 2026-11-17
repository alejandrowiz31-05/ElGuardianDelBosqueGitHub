
using UnityEngine;

public class ManosTrigger : MonoBehaviour
{
    // =========================
    // VARIABLES
    // =========================

    // Guarda las manos que se van a mostrar u ocultar durante el juego.
    public GameObject[] manos;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Oculta las manos para que no aparezcan al comenzar el juego.
        CambiarManos(false);
    }


    // =========================
    // MOSTRAR Y OCULTAR MANOS
    // =========================

    // Activa o desactiva todas las manos según el valor recibido.
    private void CambiarManos(bool encendidas)
    {
        // Recorre una por una las manos guardadas en la lista.
        foreach (GameObject mano in manos)
        {
            // Muestra la mano si recibe true y la oculta si recibe false.
            mano.SetActive(encendidas);
        }
    }


    // =========================
    // ENTRADA DEL JUGADOR
    // =========================

    // Se ejecuta cuando otro objeto entra en la zona de detección.
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba que el objeto que entró sea el jugador.
        if (other.CompareTag("Player"))
        {
            // Muestra las manos cuando el Viajero entra en la zona.
            CambiarManos(true);

            // Informa en la consola que el Viajero se acercó al charco.
            Debug.Log("El Viajero se acercó al charco");
        }
    }


    // =========================
    // SALIDA DEL JUGADOR
    // =========================

    // Se ejecuta cuando otro objeto sale de la zona de detección.
    private void OnTriggerExit(Collider other)
    {
        // Comprueba que el objeto que salió sea el jugador.
        if (other.CompareTag("Player"))
        {
            // Oculta las manos cuando el Viajero sale de la zona.
            CambiarManos(false);

            // Informa en la consola que el Viajero se alejó del charco.
            Debug.Log("El Viajero se alejó del charco");
        }
    }
}
