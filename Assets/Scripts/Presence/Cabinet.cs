using UnityEngine;

public class Cabinet : MonoBehaviour
{
    private Rigidbody rb;
    private bool isKnocked;

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
        if (isKnocked) return;
        isKnocked = true;

        rb.isKinematic = false;

        Vector3 dir = new Vector3(
            Random.Range(-1f,1f),
            .3f,
            Random.Range(-1f,1f)
        );

        rb.maxAngularVelocity = 10f;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5f);
    }
}