using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
 
public class UmbralTrigger : MonoBehaviour
{
    // =========================
    // ATRIBUTOS DE ANIMACIÓN
    // =========================
    public AlembicStreamPlayer ramasAlembic;   // Referencia al Alembic que contiene la animación de las Raíces del Umbral
    public float velocidadAbrir = 1f;          // Velocidad a la que avanza la animación cuando el jugador se acerca
    public float velocidadCerrar = 1f;         // Velocidad a la que retrocede la animación cuando el jugador se aleja
 
    // =========================
    // ESTADO
    // =========================
    private bool jugadorCerca = false;         // Indica si el jugador está dentro del área de activación de las raíces
 
 
    // =========================
    // UPDATE — orquesta el avance o retroceso de la animación cada frame
    // =========================
    private void Update()
    {
        if (ramasAlembic == null)
            return;
 
        if (jugadorCerca)
            AvanzarAnimacion();
        else
            RetrocederAnimacion();
    }
 
 
    // =========================
    // MÉTODO: AvanzarAnimacion
    // Hace avanzar la animación de las raíces mientras el jugador permanece cerca
    // =========================
    private void AvanzarAnimacion()
    {
        ramasAlembic.CurrentTime += velocidadAbrir * Time.deltaTime;
 
        // No permitir que la animación pase del final
        ramasAlembic.CurrentTime = Mathf.Clamp(
            ramasAlembic.CurrentTime,
            0f,
            ramasAlembic.Duration
        );
    }
 
 
    // =========================
    // MÉTODO: RetrocederAnimacion
    // Hace retroceder la animación de las raíces cuando el jugador se aleja
    // =========================
    private void RetrocederAnimacion()
    {
        ramasAlembic.CurrentTime -= velocidadCerrar * Time.deltaTime;
 
        // No permitir que la animación pase del inicio
        ramasAlembic.CurrentTime = Mathf.Clamp(
            ramasAlembic.CurrentTime,
            0f,
            ramasAlembic.Duration
        );
    }
 
 
    // =========================
    // MÉTODO: OnTriggerEnter
    // Detecta cuándo el jugador entra en el área de las raíces
    // =========================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
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
            jugadorCerca = false;
        }
    }
}