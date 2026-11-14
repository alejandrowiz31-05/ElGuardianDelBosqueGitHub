using UnityEngine;   // Permite usar las herramientas de Unity

// El nombre de la clase debe ser igual al nombre del archivo
public class DanioAlViajero : MonoBehaviour
{
    // Aquí arrastras el LivesPanel (el que tiene el script PanelVidas).
    // El tipo es "PanelVidas": así este script puede llamar sus métodos.
    public PanelVidas panelVidas;

    // Unity ejecuta esto SOLO cuando algo ENTRA a esta zona.
    private void OnTriggerEnter(Collider other)
    {
        // Solo reaccionamos si lo que entró es el Viajero
        if (other.CompareTag("Player"))
        {
            panelVidas.PerderVida();   // Llama al método que escribiste en PanelVidas
            Debug.Log("El Viajero recibió daño de: " + gameObject.name);
        }
    }
}