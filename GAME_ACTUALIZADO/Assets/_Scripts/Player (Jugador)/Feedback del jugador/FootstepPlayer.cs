using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    // Tipos de superficie que podemos reconocer
    public enum SurfaceType
    {
        Road,  // Camino
        Grass  // Césped
    }

    [Header("Sonidos por superficie")]
    public AudioClip[] roadClips;   // Sonidos para camino
    public AudioClip[] grassClips;  // Sonidos para césped

    [Header("Ajustes de pasos")]
    public AudioSource audioSource; // Componente que reproduce sonidos

    public PlayerController playerController; // (Opcional) referencia al controlador del jugador
    public float stepInterval = 0.5f; // Tiempo entre pasos en segundos

    // Variables internas
    private float stepTimer = 0f;       // Cuenta el tiempo hasta el siguiente paso
    private bool isMoving = false;      // ¿El jugador se está moviendo?
    private AudioClip[] currentClips;   // Array de sonidos que se usan ahora

    void Start()
    {
        // Al iniciar, asumimos que el jugador está sobre césped
        SetSurface(SurfaceType.Grass);
    }

    void Update()
    {
        // Si el jugador se está moviendo, avanzamos el temporizador
        if (isMoving)
        {
            stepTimer += Time.deltaTime; // Time.deltaTime = tiempo que pasó desde el último frame

            // Cuando el temporizador llega al intervalo, reproducimos un paso
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();   // Reproducir sonido de paso
                stepTimer = 0f;   // Reiniciar el temporizador
            }
        }
    }

    // Este método lo llamas desde PlayerController para decir si el jugador se mueve
    public void SetMoving(bool moving)
    {
        isMoving = moving;
        if (!moving)
            stepTimer = 0f; // Si se detiene, reiniciamos el temporizador para no acumular pasos
    }

    // Cambia qué sonidos usar según la superficie usando if / else
    public void SetSurface(SurfaceType surface)
    {
        if (surface == SurfaceType.Road)
        {
            currentClips = roadClips; // Si está en camino, usar roadClips
        }
        else if (surface == SurfaceType.Grass)
        {
            currentClips = grassClips; // Si está en césped, usar grassClips
        }
        else
        {
            currentClips = null; // Seguridad: si llega algo raro, no hay clips
        }
    }

    // Reproduce un sonido aleatorio del array actual
    void PlayFootstep()
    {
        // Si no hay clips asignados, no hacer nada
        if (currentClips == null || currentClips.Length == 0)
            return;

        // Elegir un índice aleatorio y reproducir ese clip una vez
        int index = Random.Range(0, currentClips.Length);
        audioSource.PlayOneShot(currentClips[index]);
    }
}
