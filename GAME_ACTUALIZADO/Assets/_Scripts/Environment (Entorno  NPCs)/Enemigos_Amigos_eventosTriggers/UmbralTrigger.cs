using UnityEngine;

 
public class UmbralTrigger : MonoBehaviour
{

    // =========================
    // ESTADO
    // =========================

    public GameObject[] raices;   // Aquí van los 4 modelos de raíces
    


    private void Start()
    {
        Debug.Log("UmbralTrigger está activo en: " + gameObject.name);
        CambiarRaices(false);
    }

    private void CambiarRaices(bool encendidas)
    {
        foreach (GameObject raiz in raices)
        {
            raiz.SetActive(encendidas);
        }
    }

    private void Update()
    {
      
    }
 
 
    
 
    // =========================
    // MÉTODO: OnTriggerEnter
    // Detecta cuándo el jugador entra en el área de las raíces
    // =========================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CambiarRaices(true);
            Debug.Log("El Viajero ENTRÓ a la zona de las raíces");
        }
    }
 
 
    // =========================
    // MÉTODO: OnTriggerExit
    // Detecta cuándo el jugador sale del área de las raíces
    // =========================
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CambiarRaices(false);
            Debug.Log("El Viajero SALIÓ de la zona de las raíces");
        }
    }
}