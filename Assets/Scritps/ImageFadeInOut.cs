using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFadeInOut : MonoBehaviour
{
    public Image[] images; // Las imágenes a animar
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    private void Start()
    {
        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }
        StartCoroutine(AnimateImages());
    }

    private IEnumerator AnimateImages()
    {
        while (true)
        {
            foreach (Image image in images)
            {
                image.gameObject.SetActive(true);
                yield return StartCoroutine(FadeIn(image));
                yield return new WaitForSeconds(1f); // Espera un segundo antes de desvanecerse
                yield return StartCoroutine(FadeOut(image));
                image.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0f;
        Color color = image.color;
        color.a = 0f;
        image.color = color;
        while (elapsedTime < fadeInDuration)
        {
            color.a = Mathf.Clamp01(elapsedTime / fadeInDuration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 1f;
        image.color = color;
    }

    private IEnumerator FadeOut(Image image)
    {
        float elapsedTime = 0f;
        Color color = image.color;
        while (elapsedTime < fadeOutDuration)
        {
            color.a = Mathf.Clamp01(1f - (elapsedTime / fadeOutDuration));
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 0f;
        image.color = color;
    }
}
