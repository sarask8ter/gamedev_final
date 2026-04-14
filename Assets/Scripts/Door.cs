using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(0, openAngle, 0);
    }

    public bool IsInteractable => true;

    public void Interact(PlayerInteractor player)
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor());
        Debug.Log("Door rotating. isOpen = " + isOpen);
    }

    
    public void Slam()
    {
        isOpen = false;
        StopAllCoroutines();
        transform.rotation = closedRot;
    }
    System.Collections.IEnumerator RotateDoor()
    {
        Debug.Log("Rotating door");
        Quaternion target = isOpen ? openRot : closedRot;

        while (Quaternion.Angle(transform.rotation, target) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                target,
                Time.deltaTime * speed
            );
            yield return null;
        }

        transform.rotation = target;
    }
}