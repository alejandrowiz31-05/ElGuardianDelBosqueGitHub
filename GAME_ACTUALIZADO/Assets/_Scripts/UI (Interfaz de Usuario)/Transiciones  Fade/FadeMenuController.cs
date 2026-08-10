using System.Collections;              // Necesario para IEnumerator y StartCoroutine
using UnityEngine;
using UnityEngine.SceneManagement;
public class FadeMenuController : MonoBehaviour
{
    public Animator animator;       // Arrastras aquí el Animator del PanelMenu
    public string sceneToLoad;      // Nombre de la escena a cargar
    public void PlayFadeOut()
    {
        StartCoroutine(FadeAndLoad());
    }
    IEnumerator FadeAndLoad()
    {
        Debug.Log("① Botón presionado: " + Time.realtimeSinceStartup);

        animator.CrossFade("FadeOut", 0f);
        // Reproduce la animación FadeOut
        Debug.Log("② CrossFade llamado: " + Time.realtimeSinceStartup);

        AsyncOperation carga = SceneManager.LoadSceneAsync(sceneToLoad); // empieza a cargar en segundo plano
        carga.allowSceneActivation = false; // pero no la muestres todavía
        Debug.Log("③ LoadSceneAsync iniciado: " + Time.realtimeSinceStartup);

        float tiempoTranscurrido = 0f;

        // Espera hasta que se cumplan AMBAS cosas: pasó al menos 1 segundo (para ver el fade)
        // Y la escena ya está realmente lista para mostrarse (progress llega a 0.9)
        while (tiempoTranscurrido < 1f || carga.progress < 0.9f)
        {
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
        Debug.Log("④ Escena lista y tiempo mínimo cumplido: " + Time.realtimeSinceStartup);

        carga.allowSceneActivation = true; // ahora sí, muestra la escena ya cargada
        Debug.Log("⑤ Escena activada: " + Time.realtimeSinceStartup);
    }
}