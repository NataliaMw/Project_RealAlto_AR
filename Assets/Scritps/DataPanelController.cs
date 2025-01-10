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

    public Slider progressBar; // Barra de progreso del audio
    public Text currentTimeText; // Texto para mostrar el tiempo actual
    public Text totalTimeText; // Texto para mostrar la duración total

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
        Button closeButton = transform.Find("Button Close").GetComponent<Button>();

        playButton.onClick.AddListener(PlayAudio);
        pauseButton.onClick.AddListener(PauseAudio);
        closeButton.onClick.AddListener(ClosePanel);

        // Configurar la barra de progreso
        progressBar.minValue = 0;
        progressBar.maxValue = 1;
        progressBar.onValueChanged.AddListener(OnProgressBarChanged);

        // Mostrar la duración total del audio si existe un clip
        if (audioSource.clip != null)
        {
            totalTimeText.text = FormatTime(audioSource.clip.length);
        }
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            // Actualizar la barra de progreso
            progressBar.value = audioSource.time / audioSource.clip.length;

            // Actualizar el tiempo actual
            currentTimeText.text = FormatTime(audioSource.time);
        }
    }

    private void PlayAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    private void PauseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    private void ClosePanel()
    {
        // Limpiar los textos y datos
        titleText.text = "";
        descriptionText.text = "";
        audioSource.clip = null;
        currentTimeText.text = "00:00";
        totalTimeText.text = "00:00";
        progressBar.value = 0;

        StartCoroutine(MovePanel(offScreenPosition));
    }

    private void OnProgressBarChanged(float value)
    {
        // Cambiar el tiempo del audio al mover el slider
        if (audioSource.clip != null)
        {
            audioSource.time = value * audioSource.clip.length;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowPanel(string title, string description, AudioClip audioClip)
    {
        titleText.text = title;
        descriptionText.text = description;
        audioSource.clip = audioClip;

        // Actualizar la duración total del audio
        if (audioClip != null)
        {
            totalTimeText.text = FormatTime(audioClip.length);
            progressBar.value = 0; // Reiniciar la barra de progreso
        }

        StartCoroutine(MovePanel(onScreenPosition));
    }

    public void HidePanel()
    {      
        // Limpiar los textos y datos
        titleText.text = "";
        descriptionText.text = "";
        audioSource.clip = null;
        currentTimeText.text = "00:00";
        totalTimeText.text = "00:00";
        progressBar.value = 0;

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