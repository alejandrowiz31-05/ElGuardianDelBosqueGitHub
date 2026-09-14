using UnityEngine;
 
// Un seguidor mágico que alterna entre seguir al jugador y husmear alrededor de él
public class MagicFollower : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    public Transform target;              // El jugador al que el seguidor seguirá
    public float followSpeed = 2.0f;      // Velocidad de seguimiento
    public float stopDistance = 2.5f;     // Qué tan cerca se queda del jugador antes de detenerse
    public float floatingHeight = 1.8f;   // Altura a la que flota el seguidor sobre el jugador
 
    [Header("Comportamiento Curioso (Vida)")]
    public float curioRadius = 5.0f;      // Radio dentro del cual el seguidor elige un punto aleatorio para husmear
    public float changeInterval = 7.0f;   // Intervalo de tiempo para alternar entre seguir y husmear (en segundos)
 
    // Variables intermedias que varios métodos necesitan compartir en el mismo frame
    private Vector3 targetPos;    // Posición objetivo actual a la que el seguidor se moverá
    private Vector3 curioPos;     // Posición aleatoria para husmear alrededor del jugador
    private bool isCurious = false; // Estado actual: si está husmeando (true) o siguiendo (false)
    private float timer;          // Temporizador para controlar el cambio de estado
 
 
    // =========================
    // INICIO
    // =========================
    void Start()
    {
        timer = changeInterval; // Comienza con el temporizador para alternar entre seguir y husmear
    }
 
 
    // =========================
    // UPDATE — orquesta el frame llamando a cada método
    // =========================
    void Update()
    {
        if (target == null) return; // Si no hay objetivo, no hacer nada
 
        ActualizarTemporizadorDeEstado();
        CalcularPosicionObjetivo();
        MoverHaciaPosicionObjetivo();
        OrientarHaciaObjetivo();
        AplicarFlotacionSuave();
    }
 
 
    // =========================
    // MÉTODO: ActualizarTemporizadorDeEstado
    // Cuenta el tiempo y alterna entre "seguir" y "husmear" cuando el temporizador llega a cero
    // =========================
    private void ActualizarTemporizadorDeEstado()
    {
        timer -= Time.deltaTime;
 
        if (timer <= 0)
        {
            isCurious = !isCurious;
            // Si se vuelve curioso, el próximo cambio será más rápido (3s); si vuelve a seguir, más lento (changeInterval)
            timer = isCurious ? 3.0f : changeInterval;
 
            if (isCurious)
            {
                ElegirPuntoDeHusmeo();
            }
        }
    }
 
 
    // =========================
    // MÉTODO: ElegirPuntoDeHusmeo
    // Genera un punto aleatorio alrededor del jugador para explorar cuando el estado es "curioso"
    // =========================
    private void ElegirPuntoDeHusmeo()
    {
        Vector3 randomOffset = Random.insideUnitSphere * curioRadius;
        curioPos = target.position + randomOffset;
        curioPos.y = target.position.y + floatingHeight + 1f; // un poco más alto que la altura de flotación normal
    }
 
 
    // =========================
    // MÉTODO: CalcularPosicionObjetivo
    // Decide hacia dónde debe moverse el seguidor según su estado actual (curioso o siguiendo)
    // =========================
    private void CalcularPosicionObjetivo()
    {
        if (isCurious)
        {
            targetPos = curioPos;
        }
        else
        {
            targetPos = target.position + (transform.position - target.position).normalized * stopDistance;
            targetPos.y = target.position.y + floatingHeight;
        }
    }
 
 
    // =========================
    // MÉTODO: MoverHaciaPosicionObjetivo
    // Desplaza suavemente al seguidor hacia la posición calculada
    // =========================
    private void MoverHaciaPosicionObjetivo()
    {
        float velocidadActual = isCurious ? 1.0f : followSpeed; // más lento al husmear, para un movimiento suave
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * velocidadActual);
    }
 
 
    // =========================
    // MÉTODO: OrientarHaciaObjetivo
    // Hace que el seguidor mire hacia donde se está moviendo
    // =========================
    private void OrientarHaciaObjetivo()
    {
        transform.LookAt(targetPos + transform.forward);
    }
 
 
    // =========================
    // MÉTODO: AplicarFlotacionSuave
    // Agrega un ligero vaivén vertical para que el seguidor se sienta "vivo"
    // =========================
    private void AplicarFlotacionSuave()
    {
        transform.position += new Vector3(0, Mathf.Sin(Time.time) * 0.2f * Time.deltaTime, 0);
    }
}