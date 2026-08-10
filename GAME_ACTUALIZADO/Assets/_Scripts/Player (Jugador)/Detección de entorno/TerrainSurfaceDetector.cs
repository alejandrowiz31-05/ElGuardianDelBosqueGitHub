using UnityEngine;

public class TerrainSurfaceDetector : MonoBehaviour
{
    public Terrain terrain;               // Referencia al Terrain de la escena
    public FootstepPlayer footstep;       // Script que reproduce sonidos de pasos

    public int roadIndex = 0;             // Índice de la capa de textura para "camino"
    public int grassIndex = 1;            // Índice de la capa de textura para "césped"

    public float rayDistance = 1.5f;      // Distancia del rayo hacia abajo

    void Update()
    {
        DetectSurface();                  // Llamar cada frame para detectar la superficie bajo el objeto
    }

    void DetectSurface()
    {
        // Crear un rayo que parte un poco arriba de la posición del objeto y va hacia abajo
        Ray ray = new Ray(transform.position + Vector3.up * 0.2f, Vector3.down);
        RaycastHit hit;

        // Si el rayo golpea algo dentro de rayDistance
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Comprobar si el collider pertenece a un Terrain
            if (hit.collider.GetComponent<Terrain>() != null)
            {
                // Convertir la posición del punto de impacto a coordenadas locales del terrain
                Vector3 terrainPos = hit.point - terrain.transform.position;

                // Calcular la posición en el mapa de splat (alphamap)
                int mapX = (int)((terrainPos.x / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth);
                int mapZ = (int)((terrainPos.z / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight);

                // Obtener los valores de mezcla de texturas en ese punto (1x1)
                float[,,] splat = terrain.terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

                // Leer la influencia de cada capa (road y grass)
                float road = splat[0, 0, roadIndex];
                float grass = splat[0, 0, grassIndex];

                // Comparar cuál textura tiene más peso y asignar el tipo de superficie
                if (road > grass)
                {
                    footstep.SetSurface(FootstepPlayer.SurfaceType.Road);
                }
                else
                {
                    footstep.SetSurface(FootstepPlayer.SurfaceType.Grass);
                }
            }
        }
    }
}
