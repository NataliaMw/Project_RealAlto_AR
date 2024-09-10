using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    // Variable pública para asignar el nombre de la escena desde el Inspector
    public string sceneName;

    // Método que se llama cuando se presiona el botón
    public void ChangeScene()
    {
        // Cambia a la escena especificada en la variable 'sceneName'
        SceneManager.LoadScene(sceneName);
    }

    public void OnARModeButtonPressed()
    {
        // Verifica si el jugador ha completado el tutorial
        if (GameStateManager.Instance.tutorialCompletedThisSession)
        {
            // Si el tutorial ya se completó en esta sesión, ir al recorrido
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            // Si el tutorial no se ha completado, ir al tutorial
            SceneManager.LoadScene("Loading Screen");
        }
    }

    public void OnTutorialCompletePressed()
    {
        // guarda si el jugador ha completado el tutorial
        GameStateManager.Instance.tutorialCompletedThisSession = true;

        //cambia la scene
        SceneManager.LoadScene(sceneName);
    }


}
