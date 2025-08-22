using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DataPanelController : MonoBehaviour
{
    // QUIÉN ABRIÓ ESTE PANEL (los objetos con ProximityInfoDisplay1 lo setean)
    public ProximityInfoDisplay1 lastCaller;

    private RectTransform rectTransform;

    private AudioSource audioSource;
    private Text titleText;
    private Text descriptionText;

    public Vector3 offScreenPosition;
    public Vector3 onScreenPosition;
    public float timeToShow = 0.25f;

    public Slider progressBar;
    public Text currentTimeText;
    public Text totalTimeText;

    // >>> NUEVO: referencia de campo al botón Explorar
    public Button explorarButton;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Rutas de hijos (ajústalas si tu jerarquía cambió)
        audioSource = transform.Find("Panel Audio").GetComponent<AudioSource>();
        titleText = transform.Find("Text Title").GetComponent<Text>();
        descriptionText = transform.Find("panel Description/Text Description").GetComponent<Text>();

        Button playButton = transform.Find("Panel Audio/Button Play").GetComponent<Button>();
        Button pauseButton = transform.Find("Panel Audio/Button Pause").GetComponent<Button>();
        Button closeButton = transform.Find("Button Close").GetComponent<Button>();

        // Si no asignaste el botón por Inspector, intenta encontrarlo por nombre
        if (explorarButton == null)
        {
            Transform t = transform.Find("Button Explorar");
            if (t != null) explorarButton = t.GetComponent<Button>();
        }

        playButton.onClick.AddListener(PlayAudio);
        pauseButton.onClick.AddListener(PauseAudio);
        closeButton.onClick.AddListener(ClosePanel);

        // Click en Explorar: cerrar panel y abrir visor del objeto que lo llamó
        if (explorarButton != null)
        {
            explorarButton.onClick.AddListener(() => StartCoroutine(CloseThenOpenViewer()));
            explorarButton.gameObject.SetActive(false); // arranca oculto
        }

        // Barra de progreso
        progressBar.minValue = 0;
        progressBar.maxValue = 1;
        progressBar.onValueChanged.AddListener(OnProgressBarChanged);

        if (audioSource.clip != null)
            totalTimeText.text = FormatTime(audioSource.clip.length);
    }

    void Update()
    {
        if (audioSource.clip != null && audioSource.isPlaying)
        {
            progressBar.value = audioSource.time / audioSource.clip.length;
            currentTimeText.text = FormatTime(audioSource.time);
        }
    }

    private void PlayAudio()
    {
        if (audioSource.clip != null)
            audioSource.Play();
    }

    private void PauseAudio()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    private void ClosePanel()
    {
        // Reset UI
        titleText.text = "";
        descriptionText.text = "";
        if (audioSource.isPlaying)
            audioSource.Stop();

        currentTimeText.text = "00:00";
        totalTimeText.text = audioSource.clip ? FormatTime(audioSource.clip.length) : "00:00";
        progressBar.value = 0;

        HidePanel(); // solo cierra; el visor se abre solo con el botón Explorar
    }

    private IEnumerator CloseThenOpenViewer()
    {
        // animación de salida
        yield return StartCoroutine(MovePanel(offScreenPosition));

        // abre el visor del objeto que abrió este panel
        if (lastCaller != null)
        {
            lastCaller.ShowViewer();
            lastCaller = null; // limpiar
        }
    }

    private void OnProgressBarChanged(float value)
    {
        if (audioSource.clip != null && audioSource.isPlaying)
            audioSource.time = value * audioSource.clip.length;
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

        if (audioClip != null)
        {
            totalTimeText.text = FormatTime(audioClip.length);
            progressBar.value = 0;
        }

        // Mostrar/ocultar el botón Explorar según sea objeto (lastCaller != null) o personaje (null)
        if (explorarButton != null)
            explorarButton.gameObject.SetActive(lastCaller != null);

        StopAllCoroutines();
        StartCoroutine(MovePanel(onScreenPosition));
    }

    public void HidePanel()
    {
        titleText.text = "";
        descriptionText.text = "";
        if (audioSource.isPlaying)
            audioSource.Stop();

        currentTimeText.text = "00:00";
        totalTimeText.text = "00:00";
        progressBar.value = 0;

        // Siempre oculta el botón y limpia el caller para evitar “arrastres”
        if (explorarButton != null)
            explorarButton.gameObject.SetActive(false);
        lastCaller = null;

        StopAllCoroutines();
        StartCoroutine(MovePanel(offScreenPosition));
    }

    private IEnumerator MovePanel(Vector3 targetPosition)
    {
        float t = 0f;
        Vector2 start = rectTransform.anchoredPosition;

        while (t < timeToShow)
        {
            float k = t / timeToShow;
            rectTransform.anchoredPosition = Vector2.Lerp(start, targetPosition, k);
            t += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }
}
