using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DataPanelController : MonoBehaviour
{
    private RectTransform rectTransform;

    private AudioSource audioSource;
    private Text titleText;
    private Text descriptionText;

    public Vector3 offScreenPosition;
    public Vector3 onScreenPosition;
    public float timeToShow;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
        // Encontrar los elementos dentro del panel
        audioSource = transform.Find("Panel Audio").GetComponent<AudioSource>();
        titleText = transform.Find("Text Title").GetComponent<Text>();
        descriptionText = transform.Find("panel Description/Text Description").GetComponent<Text>();

        // Agregar listeners a los botones
        Button playButton = transform.Find("Panel Audio/Button Play").GetComponent<Button>();
        Button pauseButton = transform.Find("Panel Audio/Button Pause").GetComponent<Button>();
        Button stopButton = transform.Find("Panel Audio/Button Stop").GetComponent<Button>();
        Button closeButton = transform.Find("Button Close").GetComponent<Button>();

        playButton.onClick.AddListener(PlayAudio);
        pauseButton.onClick.AddListener(PauseAudio);
        stopButton.onClick.AddListener(StopAudio);
        closeButton.onClick.AddListener(closePanel);
    }

    // Metodo para reproducir el audio
    private void PlayAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    // Método para pausar el audio
    private void PauseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // Método para detener el audio y limpiar los datos
    private void StopAudio()
    {
        // detener el Audio
        audioSource.Stop();
         
    }

    private void closePanel()
    {
        // Limpiar los textos y texto
        titleText.text = "";
        descriptionText.text = "";
        audioSource.clip = null;

        StartCoroutine(MovePanel(offScreenPosition));

    }

    public void ShowPanel(string title, string description, AudioClip audioClip)
    {
        titleText.text = title;
        descriptionText.text = description;
        audioSource.clip = audioClip;

        StartCoroutine(MovePanel(onScreenPosition));
    }

    public void HidePanel()
    {      

        // Limpiar los textos y texto
        titleText.text = "";
        descriptionText.text = "";
        audioSource.clip = null;

        StartCoroutine(MovePanel(offScreenPosition));
    }

    private IEnumerator MovePanel(Vector3 targetPosition)
    {
        float time = 0;
        Vector2 startPosition = rectTransform.anchoredPosition;

        while (time < timeToShow)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time);
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        
    }
}
