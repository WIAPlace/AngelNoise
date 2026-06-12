using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnStart : MonoBehaviour
{
    public AudioSource[] audioSources;
    public float fadeDuration = 2.0f; // Time in seconds to reach full volume
    private float[] targetVolume; // Max volume to reach

    void OnEnable()
    {
        targetVolume = new float[audioSources.Length];
        for(int i = 0; i<audioSources.Length;i++)
        {
            if(audioSources[i]!=null && audioSources[i].isActiveAndEnabled){
                targetVolume[i] = audioSources[i].volume;

                // Ensure volume starts at 0 and play the audio
                audioSources[i].volume = 0;
                audioSources[i].Play();
            }
        }
        // Start the fading process
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        //Debug.Log("Fading in");
        float currentTime = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            // Linearly interpolate volume from 0 to target over the duration
            for(int i = 0; i<audioSources.Length;i++)
            {
                if(audioSources[i]!=null && audioSources[i].isActiveAndEnabled){
                    audioSources[i].volume = Mathf.Lerp(0, targetVolume[i], currentTime / fadeDuration);
                }
            }
            yield return null;
        }
        for(int i = 0; i<audioSources.Length;i++)
        {   
            if(audioSources[i]!=null && audioSources[i].isActiveAndEnabled){
                audioSources[i].volume = targetVolume[i];
            }
        }
    }
}
