using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    [SerializeField] private FirstPersonController controller;

    private AudioSource audioSource;
    private CharacterController charController;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        charController = controller.GetComponent<CharacterController>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
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