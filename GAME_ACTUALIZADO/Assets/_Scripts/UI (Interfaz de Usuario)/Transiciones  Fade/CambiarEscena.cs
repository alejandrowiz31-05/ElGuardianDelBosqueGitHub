
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    // =========================
    // CAMBIAR DE ESCENA
    // =========================

    // Carga una escena del videojuego utilizando su nombre.
    public void CargarEscena(string nombreEscena)
    {
        // Abre la escena que corresponde al nombre recibido.
        SceneManager.LoadScene(nombreEscena);
    }


    // =========================
    // SALIR DEL JUEGO
    // =========================

    // Cierra la aplicación del videojuego.
    public void Salir()
    {
        // Muestra un mensaje en la consola al solicitar la salida.
        Debug.Log("Saliendo del juego");

        // Cierra la aplicación cuando el juego está compilado.
        Application.Quit();
    }
}
