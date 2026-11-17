
using UnityEngine;

public class FootstepDust : MonoBehaviour
{
    // =========================
    // VARIABLES DE LOS PIES
    // =========================

    // Guarda la referencia al pie izquierdo del personaje.
    public Transform footL;

    // Guarda la referencia al pie derecho del personaje.
    public Transform footR;


    // =========================
    // VARIABLES DEL EFECTO
    // =========================

    // Guarda el prefab del polvo que aparece cuando un pie toca el suelo.
    public GameObject dustPrefab;

    // Indica qué capa de Unity se considera suelo.
    public LayerMask groundLayer;

    // Indica cuántos segundos permanece el efecto antes de destruirse.
    public float destroyTime = 2f;


    // =========================
    // VARIABLES INTERNAS
    // =========================

    // Indica si el pie izquierdo estaba tocando el suelo.
    private bool leftFootDown = false;

    // Indica si el pie derecho estaba tocando el suelo.
    private bool rightFootDown = false;


    // =========================
    // ACTUALIZACIÓN DEL JUEGO
    // =========================

    // Se ejecuta una vez por cada fotograma del juego.
    private void Update()
    {
        // Comprueba si el pie izquierdo está tocando el suelo.
        CheckFoot(footL, ref leftFootDown);

        // Comprueba si el pie derecho está tocando el suelo.
        CheckFoot(footR, ref rightFootDown);
    }


    // =========================
    // DETECCIÓN DEL SUELO
    // =========================

    // Comprueba si un pie toca el suelo y genera polvo al entrar en contacto.
    private void CheckFoot(Transform foot, ref bool footState)
    {
        // Comprueba que exista una referencia al pie.
        if (foot == null)
        {
            // Detiene el método si no hay un pie asignado.
            return;
        }

        // Guarda la información del punto donde se detecta el suelo.
        RaycastHit hit;

        // Dibuja un rayo rojo hacia abajo para visualizar la detección en Scene.
        Debug.DrawRay(
            foot.position,
            Vector3.down * 0.2f,
            Color.red
        );

        // Comprueba si hay suelo debajo del pie a una distancia de 0.2 unidades.
        if (Physics.Raycast(
            foot.position,
            Vector3.down,
            out hit,
            0.2f,
            groundLayer))
        {
            // Comprueba si el pie acaba de entrar en contacto con el suelo.
            if (!footState)
            {
                // Crea el efecto de polvo en el punto donde el pie toca el suelo.
                GameObject tempDust = Instantiate(
                    dustPrefab,
                    hit.point + Vector3.up * 0.05f,
                    Quaternion.identity
                );

                // Destruye el efecto después del tiempo configurado.
                Destroy(tempDust, destroyTime);

                // Indica que el pie está tocando el suelo.
                footState = true;
            }
        }
        else
        {
            // Indica que el pie dejó de tocar el suelo.
            footState = false;
        }
    }
}
