using UnityEngine;

public class ItemAudio : MonoBehaviour
{
    public static ItemAudio Instance;

    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private AudioClip dropClip;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayPickup(Vector3 position)
    {
        PlayClip(pickupClip, position);
    }

    public void PlayDrop(Vector3 position)
    {
        PlayClip(dropClip, position);
    }

    private void PlayClip(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject obj = new GameObject("TempAudio");
        obj.transform.position = position;

        var source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 1f;
        source.Play();

        Destroy(obj, clip.length);
    }
}