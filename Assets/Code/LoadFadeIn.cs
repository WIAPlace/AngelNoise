using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInFade : MonoBehaviour
{
    public GameObject loadingScreen;
    public CanvasGroup canvasGroup;

    void Start()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(StartFade());
    }

    IEnumerator StartFade()
    {
        yield return StartCoroutine(FadeLoadingScreen(0, 1)); //The speed of the fade in
        loadingScreen.SetActive(false); //Turns off the fade image

    }

    // The fade in itself
    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
        /*
        if (GameManager.Instance != null)
        {
            GameManager.Instance.input.SetGameplay();
        }
        */
    }
}
