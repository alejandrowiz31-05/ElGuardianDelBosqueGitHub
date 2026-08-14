using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

public class UmbralTrigger : MonoBehaviour
{
    public AlembicStreamPlayer ramasAlembic;

    public float velocidadAbrir = 1f;
    public float velocidadCerrar = 1f;

    private bool jugadorCerca = false;

    void Update()
    {
        if (ramasAlembic == null)
            return;

        if (jugadorCerca)
        {
            // Avanza la animación
            ramasAlembic.CurrentTime += velocidadAbrir * Time.deltaTime;

            // No permitir que pase del final
            ramasAlembic.CurrentTime = Mathf.Clamp(
                ramasAlembic.CurrentTime,
                0f,
                ramasAlembic.Duration
            );
        }
        else
        {
            // Regresa la animación
            ramasAlembic.CurrentTime -= velocidadCerrar * Time.deltaTime;

            // No permitir que pase del inicio
            ramasAlembic.CurrentTime = Mathf.Clamp(
                ramasAlembic.CurrentTime,
                0f,
                ramasAlembic.Duration
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}