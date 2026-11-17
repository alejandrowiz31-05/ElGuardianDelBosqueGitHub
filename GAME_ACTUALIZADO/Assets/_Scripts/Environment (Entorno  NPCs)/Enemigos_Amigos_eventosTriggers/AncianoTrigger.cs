
using UnityEngine;

public class AncianoTrigger : MonoBehaviour
{
    // =========================
    // VARIABLES
    // =========================

    // Guarda el objeto del Anciano que se va a mostrar y ocultar.
    public GameObject anciano;

    // Indica cuántos segundos permanecerá visible el Anciano.
    public float tiempoVisible = 3f;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Oculta al Anciano para que no aparezca al comenzar el juego.
        anciano.SetActive(false);

        // Informa en la consola que el script está activo.
        Debug.Log("AncianoTrigger activo en: " + gameObject.name);
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
            // Muestra al Anciano cuando el Viajero entra en la zona.
            anciano.SetActive(true);

            // Programa la desaparición del Anciano después del tiempo indicado.
            Invoke("OcultarAnciano", tiempoVisible);

            // Informa en la consola que el Anciano apareció.
            Debug.Log("El Anciano apareció");
        }
    }


    // =========================
    // DESAPARICIÓN DEL ANCIANO
    // =========================

    // Oculta al Anciano cuando termina el tiempo programado.
    private void OcultarAnciano()
    {
        // Oculta al Anciano del escenario.
        anciano.SetActive(false);

        // Informa en la consola que el Anciano desapareció.
        Debug.Log("El Anciano desapareció");
    }
}
