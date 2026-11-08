using UnityEngine;   // Permite usar las herramientas de Unity

// El nombre de la clase debe ser igual al nombre del archivo
public class ManosTrigger : MonoBehaviour
{
    // La lista de manos que este charco va a encender y apagar.
    // "public" hace que aparezca en el Inspector para arrastrar las manos ahí.
    public GameObject[] manos;

    // Start se ejecuta UNA sola vez, cuando das Play.
    private void Start()
    {
        CambiarManos(false);   // Empieza con las manos apagadas (invisibles)
    }

    // Este método enciende o apaga TODAS las manos de la lista.
    // "encendidas" es un sí (true) o un no (false) que tú le pasas.
    private void CambiarManos(bool encendidas)
    {
        // foreach = "para cada mano de la lista, haz lo siguiente"
        foreach (GameObject mano in manos)
        {
            mano.SetActive(encendidas);   // true = aparece, false = desaparece
        }
    }

    // Unity ejecuta esto SOLO cuando algo ENTRA a la zona del trigger.
    // "other" es lo que entró.
    private void OnTriggerEnter(Collider other)
    {
        // Solo reaccionamos si lo que entró tiene la etiqueta "Player" (el Viajero)
        if (other.CompareTag("Player"))
        {
            CambiarManos(true);   // Las manos aparecen
            Debug.Log("El Viajero se acercó al charco");
        }
    }

    // Unity ejecuta esto SOLO cuando algo SALE de la zona del trigger.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CambiarManos(false);   // Las manos desaparecen
            Debug.Log("El Viajero se alejó del charco");
        }
    }
}