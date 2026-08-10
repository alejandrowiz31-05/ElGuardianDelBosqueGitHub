using UnityEngine;

public class FootstepDust : MonoBehaviour // Un script para generar polvo al pisar el suelo con los pies, usando raycasts para detectar el contacto con el suelo
{
    public Transform footL; // Referencia al pie izquierdo
    public Transform footR; // Referencia al pie derecho
    public GameObject dustPrefab; // Prefab del efecto de polvo que se instanciará al pisar el suelo
    public LayerMask groundLayer; // Capa que representa el suelo, para que el raycast solo detecte colisiones con el suelo

    private bool leftFootDown = false; // Estado del pie izquierdo (si está tocando el suelo o no)
    private bool rightFootDown = false; // Estado del pie derecho (si está tocando el suelo o no)
    
    // --- NUEVA VARIABLE PARA CONTROLAR EL TIEMPO ---
    public float destroyTime = 2f; // Tiempo en segundos después del cual el efecto de polvo se destruirá automáticamente

    void Update() // En cada frame, verificamos el estado de ambos pies para generar el efecto de polvo cuando toquen el suelo
    {
        CheckFoot(footL, ref leftFootDown); // Verificar el pie izquierdo
        CheckFoot(footR, ref rightFootDown); // Verificar el pie derecho
    }

    void CheckFoot(Transform foot, ref bool footState) // Método para verificar si un pie está tocando el suelo usando un raycast, y generar el efecto de polvo si es así
    {
        if (foot == null) return; // Si no hay referencia al pie, no hacer nada

        Ray ray = new Ray(foot.position, Vector3.up * 0.5f); // Crear un rayo que se origina en la posición del pie y apunta hacia abajo (hacia el suelo)
        RaycastHit hit; // Variable para almacenar la información del impacto del raycast

        // Dibujamos el rayo para que lo veas en la pestaña Scene
        Debug.DrawRay(foot.position, Vector3.down * 0.2f, Color.red); // Cambié la dirección a Vector3.down para que el rayo apunte hacia abajo, y la distancia a 0.2f para que sea más preciso al detectar el suelo

        // Cambié la distancia a 0.2f porque 1.2f es MUCHO (detectaría el suelo antes de pisar)
        if (Physics.Raycast(foot.position, Vector3.down, out hit, 0.2f, groundLayer)) // Si el raycast detecta una colisión con el suelo dentro de la distancia especificada
        {
            if (!footState) // Si el pie no estaba tocando el suelo en el frame anterior, entonces generamos el efecto de polvo
            {
                // Aparece el humo
                GameObject tempDust = Instantiate(dustPrefab, hit.point + Vector3.up * 0.05f, Quaternion.identity); // Instanciamos el prefab del polvo en la posición del impacto del raycast, con un pequeño desplazamiento hacia arriba para que no se vea cortado por el suelo

                // --- CAMBIO CLAVE: Destruir el objeto después de 2 segundos ---
                Destroy(tempDust, destroyTime); // Esto asegura que el efecto de polvo se destruya automáticamente después de un tiempo, evitando que se acumulen muchos objetos en la escena y afecten el rendimiento

                footState = true; // Actualizamos el estado del pie para que no se genere el efecto de polvo en cada frame mientras el pie sigue tocando el suelo, sino solo cuando el pie toque el suelo por primera vez (cuando cambia de no tocar a tocar)
            }
        }
        else // Si el raycast no detecta el suelo, significa que el pie no está tocando el suelo, así que actualizamos el estado del pie a false para que se pueda generar el efecto de polvo la próxima vez que toque el suelo
        {
            footState = false; // El pie no está tocando el suelo, así que actualizamos el estado a false para que se pueda generar el efecto de polvo la próxima vez que toque el suelo
        }
    }
}