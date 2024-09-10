using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageEffects : MonoBehaviour
{
    public Image[] images; // Las imágenes a animar
    public RectTransform parentRectTransform; // El contenedor principal de las imágenes
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float scaleIncreaseDuration = 1f;
    public Vector2 initialScale = Vector2.one; // Escala inicial
    public Vector2 finalScale = Vector2.one * 1.5f; // Escala final
    public Vector2 minPosition;
    public Vector2 maxPosition;

    private void Start()
    {
        parentRectTransform = GetComponent<RectTransform>();

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
                StartCoroutine(AnimateImage(image));
                yield return new WaitForSeconds(1f); // Delay entre imágenes para escalonar el efecto
            }
        }
    }

    private IEnumerator AnimateImage(Image image)
    {
        while (true)
        {
            // Calcular minPosition y maxPosition basados en el tamaño del contenedor padre
            Vector2 minPosition = new Vector2(-parentRectTransform.rect.width / 2, -parentRectTransform.rect.height / 2);
            Vector2 maxPosition = new Vector2(parentRectTransform.rect.width / 2, parentRectTransform.rect.height / 2);

            // Posicionar la imagen en una posición aleatoria dentro del contenedor
            Vector2 randomPosition = new Vector2(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y)
            );
            image.rectTransform.anchoredPosition = randomPosition;

            // Iniciar el fade-in y aumento de escala
            yield return StartCoroutine(FadeIn(image));
            yield return StartCoroutine(IncreaseScale(image));

            // Mantener la imagen visible por un momento
            yield return new WaitForSeconds(1f);

            // Iniciar el fade-out
            yield return StartCoroutine(FadeOut(image));

            // Desactivar la imagen después del fade-out
            image.gameObject.SetActive(false);

            // Esperar un momento antes de reiniciar el ciclo
            yield return new WaitForSeconds(1f);
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

    private IEnumerator IncreaseScale(Image image)
    {
        float elapsedTime = 0f;
        Vector3 initialScale3D = new Vector3(initialScale.x, initialScale.y, 1);
        Vector3 finalScale3D = new Vector3(finalScale.x, finalScale.y, 1);
        while (elapsedTime < scaleIncreaseDuration)
        {
            image.rectTransform.localScale = Vector3.Lerp(initialScale3D, finalScale3D, elapsedTime / scaleIncreaseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.rectTransform.localScale = finalScale3D;
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
