
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    // =========================
    // VARIABLES DE LA RESISTENCIA
    // =========================

    // Guarda la imagen que representa la barra de resistencia.
    public Image fillImage;

    // Indica la cantidad máxima de resistencia del jugador.
    public float maxStamina = 1f;

    // Guarda la cantidad actual de resistencia del jugador.
    public float currentStamina = 1f;

    // Indica la velocidad con la que se consume la resistencia.
    public float drainSpeed = 0.3f;

    // Indica la velocidad con la que se recupera la resistencia.
    public float recoverSpeed = 0.2f;


    // =========================
    // ACTUALIZACIÓN DEL JUEGO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Actualiza el nivel visible de la barra según la resistencia actual.
        fillImage.fillAmount = currentStamina / maxStamina;
    }


    // =========================
    // CONSUMO DE RESISTENCIA
    // =========================

    // Disminuye la resistencia del jugador según la cantidad recibida.
    public void UseStamina(float amount)
    {
        // Resta resistencia utilizando la velocidad de consumo.
        currentStamina -= amount * drainSpeed;

        // Mantiene la resistencia entre cero y el valor máximo.
        currentStamina = Mathf.Clamp(
            currentStamina,
            0f,
            maxStamina
        );
    }


    // =========================
    // RECUPERACIÓN DE RESISTENCIA
    // =========================

    // Aumenta la resistencia del jugador según la cantidad recibida.
    public void RecoverStamina(float amount)
    {
        // Suma resistencia utilizando la velocidad de recuperación.
        currentStamina += amount * recoverSpeed;

        // Mantiene la resistencia entre cero y el valor máximo.
        currentStamina = Mathf.Clamp(
            currentStamina,
            0f,
            maxStamina
        );
    }
}
