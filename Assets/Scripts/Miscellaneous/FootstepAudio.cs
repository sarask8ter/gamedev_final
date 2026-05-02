using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FirstPersonController))]
public class FootstepAudio : MonoBehaviour
{
    private FirstPersonController controller;
    private AudioSource audioSource;
    private CharacterController charController;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<FirstPersonController>();
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller == null) return;

        bool isMoving =
            controller.Grounded &&
            new Vector3(charController.velocity.x, 0, charController.velocity.z).magnitude > 0.1f;

        if (isMoving)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}