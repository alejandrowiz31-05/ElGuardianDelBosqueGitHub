
using UnityEngine;

public class TerrainSurfaceDetector : MonoBehaviour
{
    // =========================
    // REFERENCIAS DEL TERRENO
    // =========================

    // Guarda la referencia al terreno de la escena.
    public Terrain terrain;

    // Guarda la referencia al script que reproduce los sonidos de pasos.
    public FootstepPlayer footstep;


    // =========================
    // CONFIGURACIÓN DE SUPERFICIES
    // =========================

    // Indica el índice de la textura que representa el camino.
    public int roadIndex = 0;

    // Indica el índice de la textura que representa el césped.
    public int grassIndex = 1;

    // Indica la distancia máxima para detectar el suelo.
    public float rayDistance = 1.5f;


    // =========================
    // ACTUALIZACIÓN DEL JUEGO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba qué superficie se encuentra debajo del personaje.
        DetectSurface();
    }


    // =========================
    // DETECCIÓN DE LA SUPERFICIE
    // =========================

    // Detecta la textura del terreno que se encuentra bajo el personaje.
    private void DetectSurface()
    {
        // Crea un rayo que comienza un poco arriba del personaje y apunta hacia abajo.
        Ray ray = new Ray(
            transform.position + Vector3.up * 0.2f,
            Vector3.down
        );

        // Guarda la información del punto donde se detecta el suelo.
        RaycastHit hit;

        // Comprueba si el rayo detecta una superficie dentro de la distancia indicada.
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Comprueba si la superficie detectada pertenece a un Terrain.
            if (hit.collider.GetComponent<Terrain>() != null)
            {
                // Calcula la posición del impacto respecto al terreno.
                Vector3 terrainPos = hit.point - terrain.transform.position;

                // Calcula la posición horizontal dentro del mapa de texturas.
                int mapX = (int)(
                    (terrainPos.x / terrain.terrainData.size.x)
                    * terrain.terrainData.alphamapWidth
                );

                // Calcula la posición vertical dentro del mapa de texturas.
                int mapZ = (int)(
                    (terrainPos.z / terrain.terrainData.size.z)
                    * terrain.terrainData.alphamapHeight
                );

                // Obtiene la información de las texturas en el punto detectado.
                float[,,] splat = terrain.terrainData.GetAlphamaps(
                    mapX,
                    mapZ,
                    1,
                    1
                );

                // Obtiene la influencia de la textura del camino.
                float road = splat[0, 0, roadIndex];

                // Obtiene la influencia de la textura del césped.
                float grass = splat[0, 0, grassIndex];

                // Comprueba si la textura del camino tiene mayor influencia.
                if (road > grass)
                {
                    // Selecciona los sonidos de pasos sobre el camino.
                    footstep.SetSurface(FootstepPlayer.SurfaceType.Road);
                }
                else
                {
                    // Selecciona los sonidos de pasos sobre el césped.
                    footstep.SetSurface(FootstepPlayer.SurfaceType.Grass);
                }
            }
        }
    }
}

