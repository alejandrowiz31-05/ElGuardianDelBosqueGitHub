using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour // Clase para manejar la barra de estamina del jugador
{
    public Image fillImage; // Imagen de la barra que se llena/vacía
    public float maxStamina = 1f; // Estamina máxima (1 = 100%)
    public float currentStamina = 1f; // Estamina actual (inicia llena)
    public float drainSpeed = 1.5f; // Qué tan rápido se gasta la estamina
    public float recoverSpeed = 0.5f; // Qué tan rápido se recupera la estamina

    void Update() // Se ejecuta cada frame
    {
        // Actualiza la parte visual de la barra según el porcentaje actual
        fillImage.fillAmount = currentStamina / maxStamina;
    }

    // Método para gastar estamina (llamado desde PlayerController cuando corres)
    public void UseStamina(float amount)
    {
        currentStamina -= amount * drainSpeed; // Resta estamina multiplicada por la velocidad de gasto
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Limita entre 0 y maxStamina
    }

    // Método para recuperar estamina (llamado cuando no estás corriendo)
    public void RecoverStamina(float amount)
    {
        currentStamina += amount * recoverSpeed; // Suma estamina multiplicada por la velocidad de recuperación
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Limita entre 0 y maxStamina
    }
}
