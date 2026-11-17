
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // =========================
    // VARIABLES
    // =========================

    // Guarda la daga que aparece en la mano del Viajero.
    public GameObject weaponInHand;

    // Guarda el texto que indica cómo recoger la daga.
    public GameObject pickupText;

    // Guarda la referencia a la daga que está en el suelo.
    private GameObject daggerOnGround;

    // Indica si el Viajero está cerca y puede recoger la daga.
    private bool canPickUp = false;

    // Guarda la copia de la daga que se crea cuando el Viajero la suelta.
    private GameObject droppedWeapon;


    // =========================
    // ENTRADA DEL JUGADOR
    // =========================

    // Se ejecuta cuando otro objeto entra en la zona de detección.
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba que el objeto que entró sea el jugador.
        if (other.CompareTag("Player"))
        {
            // Guarda este objeto como la daga que está en el suelo.
            daggerOnGround = gameObject;

            // Permite recoger la daga mientras el Viajero esté cerca.
            canPickUp = true;

            // Comprueba que el texto esté asignado antes de mostrarlo.
            if (pickupText != null)
            {
                pickupText.SetActive(true);
            }
        }
    }


    // =========================
    // SALIDA DEL JUGADOR
    // =========================

    // Se ejecuta cuando otro objeto sale de la zona de detección.
    private void OnTriggerExit(Collider other)
    {
        // Comprueba que el objeto que salió sea el jugador.
        if (other.CompareTag("Player"))
        {
            // Impide recoger la daga cuando el Viajero se aleja.
            canPickUp = false;

            // Borra la referencia a la daga que estaba cerca.
            daggerOnGround = null;

            // Comprueba que el texto esté asignado antes de ocultarlo.
            if (pickupText != null)
            {
                pickupText.SetActive(false);
            }
        }
    }


    // =========================
    // CONTROLES DEL JUGADOR
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba si el Viajero puede recoger la daga y presiona E.
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            // Recoge la daga.
            PickUpWeapon();
        }

        // Comprueba si la daga está en la mano y el Viajero presiona Q.
        if (weaponInHand.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            // Suelta la daga.
            DropWeapon();
        }
    }


    // =========================
    // RECOGER LA DAGA
    // =========================

    // Activa la daga en la mano y oculta la que estaba en el suelo.
    private void PickUpWeapon()
    {
        // Muestra la daga que está en la mano del Viajero.
        weaponInHand.SetActive(true);

        // Comprueba que exista una referencia a la daga del suelo.
        if (daggerOnGround != null)
        {
            // Oculta la daga que estaba en el suelo.
            daggerOnGround.SetActive(false);
        }

        // Comprueba que el texto esté asignado antes de ocultarlo.
        if (pickupText != null)
        {
            pickupText.SetActive(false);
        }

        // Impide volver a recogerla hasta entrar en otra zona válida.
        canPickUp = false;
    }


    // =========================
    // SOLTAR LA DAGA
    // =========================

    // Oculta la daga de la mano y crea una copia en el suelo.
    private void DropWeapon()
    {
        // Oculta la daga que está en la mano del Viajero.
        weaponInHand.SetActive(false);

        // Crea una copia de la daga un poco más adelante de este objeto.
        droppedWeapon = Instantiate(
            gameObject,
            transform.position + transform.forward * 0.5f,
            Quaternion.identity
        );

        // Activa la copia para que aparezca en el escenario.
        droppedWeapon.SetActive(true);

        // Comprueba si la copia ya tiene un componente Rigidbody.
        if (!droppedWeapon.TryGetComponent<Rigidbody>(out _))
        {
            // Agrega Rigidbody si no existe, para que tenga física.
            droppedWeapon.AddComponent<Rigidbody>();
        }

        // Comprueba si la copia ya tiene un componente Collider.
        if (!droppedWeapon.TryGetComponent<Collider>(out _))
        {
            // Agrega BoxCollider si no existe, para detectar colisiones.
            droppedWeapon.AddComponent<BoxCollider>();
        }
    }
}

