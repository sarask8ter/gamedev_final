using UnityEngine;

public class Cabinet : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void KnockOver()
    {
        rb.isKinematic = false;
        rb.AddForce(Vector3.right * 5f, ForceMode.Impulse);
    }
}