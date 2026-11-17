
using UnityEngine;
using TMPro;

public class NotePickup : MonoBehaviour
{
    // =========================
    // VARIABLES DE LA NOTA
    // =========================

    // Permite escribir el contenido de la nota desde el Inspector.
    [TextArea(3, 10)]
    public string noteText;

    // Guarda el panel donde se muestra la nota.
    public GameObject uiPanel;

    // Guarda el texto donde se muestra el contenido de la nota.
    public TextMeshProUGUI uiText;

    // Guarda el mensaje que indica cómo interactuar con la nota.
    public GameObject interactText;


    // =========================
    // VARIABLES INTERNAS
    // =========================

    // Indica si el jugador se encuentra cerca de la nota.
    private bool isPlayerNearby = false;

    // Indica si el jugador está leyendo la nota.
    private bool isReading = false;


    // =========================
    // ACTUALIZACIÓN DEL JUEGO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba si el jugador está cerca y presiona la tecla E.
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Comprueba si la nota se encuentra cerrada.
            if (!isReading)
            {
                // Muestra el contenido de la nota.
                ShowNote();
            }
            else
            {
                // Cierra la nota si ya se encuentra abierta.
                CloseNote();
            }
        }

        // Comprueba si la nota está abierta y se presiona F o Enter.
        if (isReading &&
            (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return)))
        {
            // Cierra la nota y permite continuar el juego.
            CloseNote();
        }
    }


    // =========================
    // MOSTRAR LA NOTA
    // =========================

    // Muestra el contenido de la nota en la pantalla.
    private void ShowNote()
    {
        // Activa el panel donde se muestra la nota.
        uiPanel.SetActive(true);

        // Coloca el contenido de la nota en el texto de la interfaz.
        uiText.text = noteText;

        // Indica que el jugador está leyendo la nota.
        isReading = true;

        // Comprueba que exista el mensaje de interacción.
        if (interactText != null)
        {
            // Oculta el mensaje de interacción mientras se lee la nota.
            interactText.SetActive(false);
        }

        // Pausa el tiempo del juego mientras la nota está abierta.
        Time.timeScale = 0f;
    }


    // =========================
    // CERRAR LA NOTA
    // =========================

    // Oculta la nota y permite continuar el juego.
    private void CloseNote()
    {
        // Oculta el panel donde se muestra la nota.
        uiPanel.SetActive(false);

        // Indica que el jugador ya no está leyendo la nota.
        isReading = false;

        // Restablece el tiempo normal del juego.
        Time.timeScale = 1f;
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
            // Indica que el jugador se encuentra cerca de la nota.
            isPlayerNearby = true;

            // Comprueba que exista el mensaje y que la nota esté cerrada.
            if (interactText != null && !isReading)
            {
                // Muestra el mensaje para interactuar con la nota.
                interactText.SetActive(true);
            }
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
            // Indica que el jugador ya no se encuentra cerca de la nota.
            isPlayerNearby = false;

            // Comprueba que exista el mensaje de interacción.
            if (interactText != null)
            {
                // Oculta el mensaje para interactuar con la nota.
                interactText.SetActive(false);
            }
        }
    }
}
