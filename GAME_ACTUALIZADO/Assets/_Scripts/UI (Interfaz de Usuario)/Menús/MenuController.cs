using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public int numeroEscena;

    public void Jugar()
    {
        SceneManager.LoadScene(numeroEscena);
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Salir del juego"); // Para ver algo en el editor
    }
}
