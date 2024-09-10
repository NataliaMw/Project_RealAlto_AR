using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageFadeAndScale : MonoBehaviour
{
    [Header("Image Settings")]
    public Image image; // La imagen que realizará el fade-in y scale
    public Vector3 initialScale = new Vector3(0.5f, 0.5f, 0.5f); // Escala inicial de la imagen
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f); // Escala objetivo durante la animación

    [Header("Audio Settings")]
    public AudioClip audioClip; // El clip de audio que se reproducirá
    private AudioSource audioSource; // Componente AudioSource para reproducir el audio

    [Header("Scene Settings")]
    public string nextSceneName; // El nombre de la escena a la que se cambiará

    private Color initialColor; // Color inicial de la imagen

    void Start()
    {
        // Configura la imagen para que sea completamente transparente al inicio
        initialColor = image.color;
        initialColor.a = 0f;
        image.color = initialColor;

        // Configura la escala inicial de la imagen
        image.transform.localScale = initialScale;

        // Configura el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = 0.8f;

        // Inicia la animación de fade-in y scale
        StartCoroutine(FadeInAndScale());
    }

    private IEnumerator FadeInAndScale()
    {
        float audioDuration = audioClip.length; // Duración del audio
        float elapsedTime = 0f;

        // Asegurarse de que el audio y la animación comiencen exactamente al mismo tiempo
        audioSource.Play();

        while (elapsedTime < audioDuration)
        {
            // Calcula el progreso de la animación
            elapsedTime += Time.deltaTime;

            // Realiza el fade-in de la imagen
            float alpha = Mathf.Clamp01(elapsedTime / audioDuration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Realiza el scale de la imagen desde la escala inicial a la escala objetivo
            image.transform.localScale = Vector3.Lerp(initialScale, targetScale, alpha);

            yield return null;
        }

        // Asegura que la imagen esté completamente visible y a la escala objetivo
        image.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
        image.transform.localScale = targetScale;

        // Cambia de escena después de que termine el audio
        SceneManager.LoadScene(nextSceneName);
    }
}
