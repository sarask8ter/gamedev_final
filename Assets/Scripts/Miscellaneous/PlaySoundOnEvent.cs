using UnityEngine;

[CreateAssetMenu(fileName = "PlaySoundOnEvent", menuName = "Audio/Play Sound On Event")]
public class PlaySoundOnEvent : EventAction
{
    public AudioClip clip;
    public float volume = 1f;
    public float pitch = 1f;

    public override void OnEventStart()
    {
        if (clip == null) return;

        GameObject temp = new GameObject("TempAudio");
        var audio = temp.AddComponent<AudioSource>();

        audio.clip = clip;
        audio.volume = volume;
        audio.pitch = pitch;
        audio.spatialBlend = 0f;

        temp.AddComponent<OneShotTemporaryAudio>();
    }
}