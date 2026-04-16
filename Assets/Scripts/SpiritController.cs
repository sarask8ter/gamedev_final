using UnityEngine;

public class SpiritController : MonoBehaviour
{
    public LightSwitch[] lights;
    public DoorPivot[] doors;
    public Cabinet[] cabinets;

    void Start()
    {
        InvokeRepeating("DoRandomEvent", 5f, 10f);
    }

    // Currently does random events every 10 seconds invoked at the start; we can change based on the storyline how we want the spirit events be triggered later on
    void DoRandomEvent()
    {
        int rand = Random.Range(0, 4);
        Debug.Log("Spirit event: " + rand);

        switch (rand)
        {
            case 0:
                Debug.Log("Flicker lights");
                FlickerLights();
                break;

            case 1:
                Debug.Log("Slam door");
                SlamDoor();
                break;

            case 2:
                Debug.Log("Knock cabinet");
                KnockCabinet();
                break;

            case 3:
                Debug.Log("Shake object");
                ShakeObject();
                break;
        }
    }

    void FlickerLights()
    {
        foreach (var light in lights)
        {
            light.Flicker(2f);
        }
    }

    void SlamDoor()
    {
        doors[Random.Range(0, doors.Length)].Slam();
    }

    void KnockCabinet()
    {
        cabinets[Random.Range(0, cabinets.Length)].KnockOver();
    }

    void ShakeObject()
    {
        // simple shake example
        Transform obj = cabinets[0].transform;

        StartCoroutine(Shake(obj));
    }

    System.Collections.IEnumerator Shake(Transform obj)
    {
        Vector3 original = obj.position;

        for (int i = 0; i < 20; i++)
        {
            obj.position = original + Random.insideUnitSphere * 0.1f;
            yield return new WaitForSeconds(0.02f);
        }

        obj.position = original;
    }
}