using UnityEngine;

public class UmbralTrigger : MonoBehaviour
{
    // =========================
    // VARIABLES
    // =========================

    // Guarda las raíces que se van a mostrar u ocultar durante el juego.
    public GameObject[] raices;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Muestra en la consola el nombre del objeto que tiene este script.
        Debug.Log("UmbralTrigger está activo en: " + gameObject.name);

        // Oculta las raíces para que no aparezcan al comenzar el juego.
        CambiarRaices(false);
    }


    // =========================
    // MOSTRAR Y OCULTAR RAÍCES
    // =========================

    // Activa o desactiva todas las raíces según el valor recibido.
    private void CambiarRaices(bool encendidas)
    {
        // Recorre una por una las raíces guardadas en la lista.
        foreach (GameObject raiz in raices)
        {
            // Muestra la raíz si recibe true y la oculta si recibe false.
            raiz.SetActive(encendidas);
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
            // Muestra las raíces cuando el Viajero entra en la zona.
            CambiarRaices(true);

            // Informa en la consola que el Viajero entró en la zona.
            Debug.Log("El Viajero ENTRÓ a la zona de las raíces");
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
            // Oculta las raíces cuando el Viajero sale de la zona.
            CambiarRaices(false);

            // Informa en la consola que el Viajero salió de la zona.
            Debug.Log("El Viajero SALIÓ de la zona de las raíces");
        }
    }
}