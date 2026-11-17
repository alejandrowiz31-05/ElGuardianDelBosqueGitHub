
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class CambioAutomatico : MonoBehaviour
{
    // =========================
    // VARIABLES DEL RECORRIDO
    // =========================

    // Guarda la referencia al componente que mueve la cámara por el recorrido.
    public CinemachineSplineDolly dolly;

    // Guarda el nombre de la escena que se cargará al finalizar el recorrido.
    public string nombreEscena;

    // Indica la posición desde donde comienza el recorrido de la cámara.
    public float posicionInicial = 0.1f;

    // Indica la velocidad con la que avanza la cámara por el recorrido.
    public float velocidad = 0.01f;


    // =========================
    // VARIABLES INTERNAS
    // =========================

    // Guarda la posición actual de la cámara dentro del recorrido.
    private float posicion;

    // Indica si la cámara terminó su recorrido.
    private bool terminado;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Restablece el tiempo normal del juego.
        Time.timeScale = 1f;

        // Establece la posición inicial de la cámara.
        posicion = posicionInicial;

        // Coloca la cámara en la posición inicial del recorrido.
        dolly.CameraPosition = posicion;
    }


    // =========================
    // ACTUALIZACIÓN DEL RECORRIDO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba si la cámara ya terminó su recorrido.
        if (terminado)
        {
            // Detiene el método para evitar continuar el movimiento.
            return;
        }

        // Aumenta la posición de la cámara según la velocidad y el tiempo.
        posicion += velocidad * Time.deltaTime;

        // Actualiza la posición de la cámara sin superar el valor máximo de 1.
        dolly.CameraPosition = Mathf.Min(posicion, 1f);

        // Comprueba si la cámara llegó al final del recorrido.
        if (posicion >= 1f)
        {
            // Guarda que el recorrido ha terminado.
            terminado = true;

            // Carga la escena configurada al finalizar el recorrido.
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
