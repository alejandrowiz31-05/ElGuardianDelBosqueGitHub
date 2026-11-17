using UnityEngine;

public class PanelVidas : MonoBehaviour
{
    // =========================
    // VARIABLES
    // =========================

    // Guarda los corazones que representan las vidas del Viajero.
    public GameObject[] vidas;

    // Guarda la cantidad de vidas que le quedan al Viajero.
    private int vidasActuales;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Cuenta los corazones de la lista para establecer las vidas iniciales.
        vidasActuales = vidas.Length;

        // Informa en la consola cuántas vidas tiene el Viajero al comenzar.
        Debug.Log("Vidas al iniciar: " + vidasActuales);
    }


    // =========================
    // PÉRDIDA DE VIDAS
    // =========================

    // Quita una vida cuando otro script llama a este método.
    public void PerderVida()
    {
        // Comprueba si el Viajero ya se quedó sin vidas.
        if (vidasActuales <= 0)
        {
            // Sale del método para evitar que las vidas sigan disminuyendo.
            return;
        }

        // Resta una vida de la cantidad actual.
        vidasActuales = vidasActuales - 1;

        // Oculta el corazón correspondiente a la vida que se perdió.
        vidas[vidasActuales].SetActive(false);

        // Informa en la consola cuántas vidas le quedan al Viajero.
        Debug.Log("Vidas restantes: " + vidasActuales);

        // Comprueba si el Viajero perdió su última vida.
        if (vidasActuales == 0)
        {
            // Informa en la consola que el Viajero se quedó sin vidas.
            Debug.Log("El Viajero se quedó sin vidas");
        }
    }
}
