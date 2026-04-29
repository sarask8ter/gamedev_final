using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Light[] lightSources;
    private bool isOn = true;
    private bool isFlickering;

    public bool IsInteractable => true;

    public void Interact(PlayerInteractor player)
    {
        ToggleLight();
    }

    public void ToggleLight()
    {
        isOn = !isOn;
        foreach (var lightSource in lightSources) lightSource.enabled = isOn;
    }

    public void Flicker(float duration)
    {
        if (isFlickering) return;
        StartCoroutine(FlickerRoutine(duration));
    }

    private IEnumerator FlickerRoutine(float duration)
    {
        isFlickering = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            foreach (var lightSource in lightSources) lightSource.enabled = !lightSource.enabled;

            float delay = Random.Range(0.05f, 0.2f);
            yield return new WaitForSeconds(delay);

            elapsed += delay;
        }

        foreach (var lightSource in lightSources) lightSource.enabled = true;
        isFlickering = false;
    }
}