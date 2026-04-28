using UnityEngine;

public class ColliderTriggerEnd : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerEnd.End();
        }
    }
}