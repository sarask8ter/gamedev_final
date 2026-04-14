using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public GameObject pressEUI;
    public float interactRange = 2.5f;
    public Transform player;

    private bool playerInRange;

    void Update()
    {
        // Don't show prompt during dialogue
        if (PlayerStateManager.State == PlayerState.Dialogue)
        {
            pressEUI.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        playerInRange = distance <= interactRange;

        pressEUI.SetActive(playerInRange);

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            BeginDialogue();
        }
    }

    void BeginDialogue()
    {
        pressEUI.SetActive(false);

        Debug.Log("Dialogue started!");

        GetComponent<Neighbor>()?.StartDialogue();
    }
}