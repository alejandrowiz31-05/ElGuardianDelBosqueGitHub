
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    // =========================
    // VARIABLES DE AUDIO
    // =========================

    // Guarda la referencia al mezclador de audio del juego.
    public AudioMixer audioMixer;

    // Guarda la referencia al control deslizante del volumen.
    public Slider volumeSlider;


    // =========================
    // INICIO DEL JUEGO
    // =========================

    // Se ejecuta una vez cuando inicia el juego.
    private void Start()
    {
        // Guarda el volumen actual del mezclador de audio.
        float currentVolume;

        // Comprueba si se puede obtener el volumen configurado.
        if (audioMixer.GetFloat("MasterVolume", out currentVolume))
        {
            // Actualiza el control deslizante con el volumen obtenido.
            volumeSlider.value = currentVolume;
        }
    }


    // =========================
    // CAMBIO DE VOLUMEN
    // =========================

    // Actualiza el volumen cuando cambia el control deslizante.
    public void OnSliderChanged()
    {
        // Aplica al mezclador el volumen seleccionado en el control.
        audioMixer.SetFloat("MasterVolume", volumeSlider.value);
    }


    // =========================
    // AUMENTAR EL VOLUMEN
    // =========================

    // Aumenta el volumen general del juego.
    public void IncreaseVolume()
    {
        // Aumenta el volumen en 2 decibelios sin superar el valor máximo.
        float newValue = Mathf.Clamp(
            volumeSlider.value + 2f,
            -40f,
            0f
        );

        // Actualiza el control deslizante con el nuevo volumen.
        volumeSlider.value = newValue;

        // Aplica el nuevo volumen al mezclador de audio.
        audioMixer.SetFloat("MasterVolume", newValue);
    }


    // =========================
    // DISMINUIR EL VOLUMEN
    // =========================

    // Disminuye el volumen general del juego.
    public void DecreaseVolume()
    {
        // Disminuye el volumen en 2 decibelios sin superar el valor mínimo.
        float newValue = Mathf.Clamp(
            volumeSlider.value - 2f,
            -40f,
            0f
        );

        // Actualiza el control deslizante con el nuevo volumen.
        volumeSlider.value = newValue;

        // Aplica el nuevo volumen al mezclador de audio.
        audioMixer.SetFloat("MasterVolume", newValue);
    }
}

