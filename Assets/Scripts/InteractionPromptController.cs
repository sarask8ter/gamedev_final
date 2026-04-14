using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    public GameObject pressEUI;
    public Transform player;

    public void HideEUI()
    {
        pressEUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressEUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressEUI.SetActive(false);
        }
    }
}