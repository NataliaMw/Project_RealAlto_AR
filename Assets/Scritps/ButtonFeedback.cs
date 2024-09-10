using UnityEngine;
using UnityEngine.UI;

public class ButtonFeedback : MonoBehaviour
{
    public AudioClip buttonSound; // Sonido para reproducir al presionar el boton
    private AudioSource audioSource;
    public float vibrationDuration = 0.1f; // Duracion de la vibracion en segundos


    void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Crear un nuevo GameObject para AudioSource si no existe
            GameObject audioSourceObject = new GameObject("AudioSource");
            audioSourceObject.transform.SetParent(transform);
            audioSource = audioSourceObject.AddComponent<AudioSource>();
        }

        // Obtener el componente Button y añadir el listener
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("No Button component found!");
        }
    }

    // Método llamado al presionar el boton
    private void OnButtonClick()
    {
        PlaySoundAndVibrate();
        float waitTime = 1.0f; // Tiempo de espera en segundos
        float timer = 0.0f;

        while (timer < waitTime)
        {
            // Actualizar el temporizador
            timer += Time.deltaTime;
        }
    }

    // Metodo para reproducir el sonido y activar la vibracion
    public void PlaySoundAndVibrate()
    {
         // Reproducir el sonido si esta habilitado
        if (SettingsManager.Instance.IsSoundEnabled && buttonSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonSound);
        }

        // Activar la vibracion con la duracion especificada si esta habilitada
        if (SettingsManager.Instance.IsVibrationEnabled)
        {
            VibrationController.Vibrate(vibrationDuration);
        }
    }
}

