using UnityEngine;   // Permite usar las herramientas de Unity

// El nombre de la clase debe ser igual al nombre del archivo
public class AncianoTrigger : MonoBehaviour
{
    // El Anciano que va a aparecer y desaparecer.
    // "public" hace que aparezca en el Inspector para arrastrarlo ahí.
    public GameObject anciano;

    // Cuántos segundos se queda visible antes de desaparecer.
    // También se puede cambiar desde el Inspector.
    public float tiempoVisible = 3f;

    // Start se ejecuta UNA sola vez, cuando das Play.
    private void Start()
    {
        anciano.SetActive(false);   // Empieza oculto
        Debug.Log("AncianoTrigger activo en: " + gameObject.name);
    }

    // Unity ejecuta esto SOLO cuando algo ENTRA a la zona del trigger.
    private void OnTriggerEnter(Collider other)
    { 
        // Solo reaccionamos si lo que entró es el Viajero
        if (other.CompareTag("Player"))
        {
            anciano.SetActive(true);                      // El Anciano aparece
            Invoke("OcultarAnciano", tiempoVisible);      // Programa: "en X segundos, ejecuta OcultarAnciano"
            Debug.Log("El Anciano apareció");
        }
    }

    // Este método lo inventamos nosotros. Solo apaga al Anciano.
    private void OcultarAnciano()
    {
        anciano.SetActive(false);   // El Anciano desaparece
        Debug.Log("El Anciano desapareció");
    }
}