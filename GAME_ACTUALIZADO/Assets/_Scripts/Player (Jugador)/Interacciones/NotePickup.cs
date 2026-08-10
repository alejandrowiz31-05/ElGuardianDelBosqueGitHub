using UnityEngine;
using TMPro;

public class NotePickup : MonoBehaviour
{
    [TextArea(3, 10)]
    public string noteText;
    public GameObject uiPanel;
    public TextMeshProUGUI uiText;
    public GameObject interactText;

    private bool isPlayerNearby = false;
    private bool isReading = false;

    void Update()
    {
        // Abrir con E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isReading)
            {
                ShowNote();
            }
            else
            {
                CloseNote();
            }
        }

        // Cerrar con F o Enter
        if (isReading && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return)))
        {
            CloseNote();
        }
    }

    private void ShowNote()
    {
        uiPanel.SetActive(true);
        uiText.text = noteText;
        isReading = true;

        if (interactText != null)
            interactText.SetActive(false);

        Time.timeScale = 0f;
    }

    private void CloseNote()
    {
        uiPanel.SetActive(false);
        isReading = false;
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (interactText != null && !isReading)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (interactText != null)
                interactText.SetActive(false);
        }
    }
}
