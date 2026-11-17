
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // =========================
    // VARIABLES DEL MENÚ DE PAUSA
    // =========================

    // Guarda la referencia al panel que muestra el menú de pausa.
    public GameObject pausePanel;

    // Indica si el juego se encuentra pausado.
    private bool isPaused = false;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Oculta el menú de pausa al comenzar la partida.
        pausePanel.SetActive(false);
    }


    // =========================
    // ACTUALIZACIÓN DEL JUEGO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba si el jugador presiona la tecla Escape.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Comprueba si el juego ya se encuentra pausado.
            if (isPaused)
            {
                // Continúa la partida si el juego está pausado.
                ResumeGame();
            }
            else
            {
                // Pausa la partida si el juego está en ejecución.
                PauseGame();
            }
        }
    }


    // =========================
    // PAUSAR EL JUEGO
    // =========================

    // Muestra el menú de pausa y detiene el tiempo del juego.
    public void PauseGame()
    {
        // Muestra el panel del menú de pausa.
        pausePanel.SetActive(true);

        // Detiene el tiempo del juego.
        Time.timeScale = 0f;

        // Guarda que el juego se encuentra pausado.
        isPaused = true;
    }


    // =========================
    // CONTINUAR EL JUEGO
    // =========================

    // Oculta el menú de pausa y continúa la partida.
    public void ResumeGame()
    {
        // Oculta el panel del menú de pausa.
        pausePanel.SetActive(false);

        // Restablece el tiempo normal del juego.
        Time.timeScale = 1f;

        // Guarda que el juego ya no se encuentra pausado.
        isPaused = false;
    }


    // =========================
    // REINICIAR EL JUEGO
    // =========================

    // Reinicia la escena que se encuentra activa.
    public void RestartGame()
    {
        // Restablece el tiempo normal antes de reiniciar.
        Time.timeScale = 1f;

        // Vuelve a cargar la escena actual desde el inicio.
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }


    // =========================
    // REGRESAR AL MENÚ PRINCIPAL
    // =========================

    // Regresa al menú principal del videojuego.
    public void QuitGame()
    {
        // Restablece el tiempo normal antes de cambiar de escena.
        Time.timeScale = 1f;

        // Carga la escena del menú principal.
        SceneManager.LoadScene("MainMenu");
    }
}


