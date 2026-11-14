using UnityEngine;   // Permite usar las herramientas de Unity

// El nombre de la clase debe ser igual al nombre del archivo
public class PanelVidas : MonoBehaviour
{
    // La lista con los 3 íconos de vida (Life_1, Life_2, Life_3).
    public GameObject[] vidas;

    // Cuántas vidas le quedan al Viajero. "int" es un número entero.
    private int vidasActuales;

    // Start se ejecuta UNA vez, al dar Play.
    private void Start()
    {
        vidasActuales = vidas.Length;   // Length = cuántos elementos tiene la lista (3)
        Debug.Log("Vidas al iniciar: " + vidasActuales);
    }

    // Este método quita UNA vida. Lo vamos a llamar cuando el Viajero reciba daño.
    // "public" permite que otros scripts lo llamen.
    public void PerderVida()
    {
        // Si ya no quedan vidas, no hacemos nada
        if (vidasActuales <= 0)
        {
            return;   // return = "sal del método ya"
        }

        vidasActuales = vidasActuales - 1;      // Resta una vida
        vidas[vidasActuales].SetActive(false);  // Apaga el ícono correspondiente
        Debug.Log("Vidas restantes: " + vidasActuales);

        if (vidasActuales == 0)
        {
            Debug.Log("El Viajero se quedó sin vidas");
        }
    }
}
