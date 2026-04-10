using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed");

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                Debug.Log("Hit: " + hit.collider.name);
                hit.collider.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}