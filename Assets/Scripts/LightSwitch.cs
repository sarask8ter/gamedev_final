using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Light lightSource;
    private bool isOn = true;

    public void Interact()
    {
        ToggleLight();
    }

    public void ToggleLight()
    {
        isOn = !isOn;
        lightSource.enabled = isOn;
    }

    public void Flicker(float duration)
    {
        StartCoroutine(FlickerRoutine(duration));
    }

    private System.Collections.IEnumerator FlickerRoutine(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            lightSource.enabled = !lightSource.enabled;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            time += Time.deltaTime;
        }

        lightSource.enabled = true;
    }
}