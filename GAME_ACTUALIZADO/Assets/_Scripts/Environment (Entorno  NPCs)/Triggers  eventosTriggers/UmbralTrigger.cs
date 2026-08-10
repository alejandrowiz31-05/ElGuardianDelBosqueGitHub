using UnityEngine;

public class UmbralTrigger : MonoBehaviour
{
    public GameObject ramasEmpty; // arrastra aquí el Empty de las ramas

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ramasEmpty.SetActive(true); // aparecen
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ramasEmpty.SetActive(false); // se ocultan al salir
        }
    }
}