using UnityEngine;

public class Cabinet : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // public void KnockOver()
    // {
    //     rb.isKinematic = false;
    //     rb.AddForce(Vector3.right * 5f, ForceMode.Impulse);
    // }

    public void KnockOver()
    {
        rb.isKinematic = false;

        Vector3 dir =
        new Vector3(
            Random.Range(-1f,1f),
            .3f,
            Random.Range(-1f,1f)
        );

        rb.AddForce(
            dir.normalized * 6f,
            ForceMode.Impulse
        );

        rb.AddTorque(
        Random.insideUnitSphere * 8f,
        ForceMode.Impulse
        );
    }
}