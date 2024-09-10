using UnityEngine;

public class VibrationController : MonoBehaviour
{
    public static void Vibrate(float seconds)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            VibrateAndroid(seconds);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    private static void VibrateAndroid(float seconds)
    {
        using (AndroidJavaObject vibrator = GetVibrator())
        {
            if (vibrator != null)
            {
                long milliseconds = (long)(seconds * 1000);
                vibrator.Call("vibrate", milliseconds);
            }
        }
    }

    private static AndroidJavaObject GetVibrator()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            return context.Call<AndroidJavaObject>("getSystemService", "vibrator");
        }
    }
}
