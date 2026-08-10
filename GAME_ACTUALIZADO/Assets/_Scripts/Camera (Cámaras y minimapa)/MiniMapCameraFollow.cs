using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    public Transform target; // El jugador

    void LateUpdate()
    {
        if (target == null) return;

        // Seguir la posición del jugador
        Vector3 newPos = target.position;
        newPos.y = transform.position.y; // Mantener altura fija
        transform.position = newPos;

        // Girar con el jugador
        transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
    }
}
