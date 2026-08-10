using UnityEngine;

public class MiniMapPlayerIcon : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player == null) return;

        // El icono rota según la dirección del jugador
        transform.rotation = Quaternion.Euler(0f, 0f, -player.eulerAngles.y);
    }
}
