using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public bool IsSoundEnabled = true;
    public bool IsVibrationEnabled = true;

    private void Awake()
    {
        // Asegurarse de que solo hay una instancia del SettingsManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSoundEnabledAndDisabled()
    {
        IsSoundEnabled = !IsSoundEnabled;
        Debug.Log("Sound Enabled: " + IsSoundEnabled);
    }

    public void SetVibrationEnabledAndDisabled()
    {
        IsVibrationEnabled = !IsVibrationEnabled;
        Debug.Log("Vibration Enabled: " + IsVibrationEnabled);
    }
}
