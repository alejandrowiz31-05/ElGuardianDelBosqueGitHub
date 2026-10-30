using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class CambioAutomatico : MonoBehaviour
{
    public CinemachineSplineDolly dolly;
    public string nombreEscena;
    public float posicionInicial = 0.1f;
    public float velocidad = 0.01f;

    private float posicion;
    private bool terminado;

    void Start()
    {
        Time.timeScale = 1f;
        posicion = posicionInicial;
        dolly.CameraPosition = posicion;
    }

    void Update()
    {
        if (terminado) return;

        posicion += velocidad * Time.deltaTime;
        dolly.CameraPosition = Mathf.Min(posicion, 1f);

        if (posicion >= 1f)
        {
            terminado = true;
            SceneManager.LoadScene(nombreEscena);
        }
    }
}