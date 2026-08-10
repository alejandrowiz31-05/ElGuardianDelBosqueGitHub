using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponInHand;      // La daga que se activa en la mano del jugador
    public GameObject pickupText;        // Texto UI que indica "Presiona E para recoger"
    private GameObject daggerOnGround;   // Referencia a la daga que está en el suelo (este objeto)
    private bool canPickUp = false;      // Si el jugador está dentro del trigger y puede recoger

    private GameObject droppedWeapon;    // Referencia a la daga que se crea al soltarla

    private void OnTriggerEnter(Collider other)
    {
        // Cuando algo entra en el trigger
        if (other.CompareTag("Player"))
        {
            daggerOnGround = gameObject; // La daga en el suelo es este GameObject
            canPickUp = true;

            if (pickupText != null)
                pickupText.SetActive(true); // Mostrar texto de recoger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando algo sale del trigger
        if (other.CompareTag("Player"))
        {
            canPickUp = false;
            daggerOnGround = null;

            if (pickupText != null)
                pickupText.SetActive(false); // Ocultar texto de recoger
        }
    }

    private void Update()
    {
        // RECOGER CON E
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUpWeapon();
        }

        // SOLTAR CON Q
        if (weaponInHand.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            DropWeapon();
        }
    }

    void PickUpWeapon()
    {
        // Activar la daga en la mano del jugador
        weaponInHand.SetActive(true);

        // Desactivar la daga que estaba en el suelo (si existe)
        if (daggerOnGround != null)
            daggerOnGround.SetActive(false);

        if (pickupText != null)
            pickupText.SetActive(false);

        canPickUp = false; // Ya no puede recoger hasta volver a entrar al trigger
    }

    void DropWeapon()
    {
        // Desactivar la daga en la mano
        weaponInHand.SetActive(false);

        // Crear una copia del objeto del suelo para que parezca que la soltó
        droppedWeapon = Instantiate(gameObject, transform.position + transform.forward * 0.5f, Quaternion.identity);
        droppedWeapon.SetActive(true);

        // Asegurar que la copia tenga Rigidbody para que caiga con física
        if (!droppedWeapon.TryGetComponent<Rigidbody>(out _))
        {
            droppedWeapon.AddComponent<Rigidbody>();
        }

        // Asegurar que la copia tenga Collider para detectar colisiones
        if (!droppedWeapon.TryGetComponent<Collider>(out _))
        {
            droppedWeapon.AddComponent<BoxCollider>();
        }
    }
}
