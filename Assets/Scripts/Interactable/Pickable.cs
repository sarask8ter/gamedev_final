using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickable : MonoBehaviour, IInteractable
{
    private string id = "box";
    public string Id => id;

    private bool isIteractable = true;
    public bool IsInteractable => isIteractable;

    private Collider coll;

    void Awake()
    {
        coll = GetComponent<Collider>();
    }

    public void Interact(PlayerInteractor player)
    {
        transform.position = player.HoldingPoint.position;
        transform.rotation = Quaternion.identity;
        coll.enabled = false;

        isIteractable = false;
    }

    public void Drop(Transform dropPoint)
    {
        transform.position = dropPoint.position;
        transform.rotation = dropPoint.rotation;
        coll.enabled = true;
    }
}
