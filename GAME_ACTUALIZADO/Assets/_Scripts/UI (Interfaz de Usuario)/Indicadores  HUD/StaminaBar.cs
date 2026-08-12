using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    // =====================================================
    // VARIABLES PÚBLICAS
    // =====================================================

    public Image fillImage;             // Imagen de la barra
    public float maxStamina = 1f;       // Stamina máxima
    public float currentStamina = 1f;   // Stamina actual
    public float drainSpeed = 0.3f;     // Velocidad al gastar stamina
    public float recoverSpeed = 0.2f;   // Velocidad al recuperar stamina


    // =====================================================
    // UPDATE
    // =====================================================

    private void Update()
    {
        // Actualizar visualmente la barra
        fillImage.fillAmount = currentStamina / maxStamina;
    }


    // =====================================================
    // GASTAR STAMINA
    // =====================================================

    public void UseStamina(float amount)
    {
        // Restar stamina
        currentStamina -= amount * drainSpeed;

        // Evitar que baje de 0
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }


    // =====================================================
    // RECUPERAR STAMINA
    // =====================================================

    public void RecoverStamina(float amount)
    {
        // Recuperar stamina
        currentStamina += amount * recoverSpeed;

        // Evitar que supere el máximo
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }
}