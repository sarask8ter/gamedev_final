using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OneShotTemporaryAudio : MonoBehaviour
{
    void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(gameObject, audioSource.clip.length / audioSource.pitch); // Changing pitch will change timing of clip, esp. important if it becomes longer.
    }
}
