using UnityEngine;
/// 
/// Author: Weston Tollette
/// Created: 3/19/26
/// Purpose: scriptable object for sound effects.
/// 
/// Edited:
/// Edited By:
/// Edit Purpose:
///
[CreateAssetMenu(menuName = "Audio/SoundEffect")]
public class SoundEffectSO : ScriptableObject {
    public AudioClip[] clips;
    public float volume = 1f;
    public float pitchVariant = 0.1f;

    // play the audio source
    public void Play(AudioSource source) {
        source.pitch = 1f + Random.Range(-pitchVariant, pitchVariant);
        source.PlayOneShot(clips[Random.Range(0, clips.Length)], volume);
    }

    // overload for if we need to know what audio source is being played
    public void Play(AudioSource source, int index) {
        source.pitch = 1f + Random.Range(-pitchVariant, pitchVariant);
        source.PlayOneShot(clips[index], volume);
    }
}
