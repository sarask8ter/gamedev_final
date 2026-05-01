using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    void Start()
    {
        var audio = GetComponent<AudioSource>();
        audio.loop = true;
        audio.spatialBlend = 0f;
        audio.Play();
    }
}