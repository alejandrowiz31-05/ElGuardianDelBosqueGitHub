using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;   // Arrastras tu MasterMixer
    public Slider volumeSlider;     // Arrastras tu SliderVolume

    void Start()
    {
        // Inicializa el slider con el valor actual del mixer
        float currentVolume;
        if (audioMixer.GetFloat("MasterVolume", out currentVolume))
        {
            volumeSlider.value = currentVolume;
        }
    }

    public void OnSliderChanged()
    {
        // Cambia el volumen cuando mueves la hoja del slider
        audioMixer.SetFloat("MasterVolume", volumeSlider.value);
    }

    public void IncreaseVolume()
    {
        // Sube el volumen en pasos de 2 dB
        float newValue = Mathf.Clamp(volumeSlider.value + 2f, -40f, 0f);
        volumeSlider.value = newValue;
        audioMixer.SetFloat("MasterVolume", newValue);
    }

    public void DecreaseVolume()
    {
        // Baja el volumen en pasos de 2 dB
        float newValue = Mathf.Clamp(volumeSlider.value - 2f, -40f, 0f);
        volumeSlider.value = newValue;
        audioMixer.SetFloat("MasterVolume", newValue);
    }
}
