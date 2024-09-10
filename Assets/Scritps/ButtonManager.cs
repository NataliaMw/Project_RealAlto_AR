using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button[] buttons; // Array de botones a gestionar
    public GameObject closePanel; // Panel que contiene el botón de cerrar
    public Button closeButton; // Botón dentro del panel para cerrarlo
    public GameObject masksButton; // Botón para mascaras cortadas

    private void Start()
    {
        // Verifica si allChallengesCompleted es true o false
        if (GameStateManager.Instance.allChallengesCompleted)
        {
            // Si todos los desafíos están completos, habilita todos los botones
            foreach (Button button in buttons)
            {
                button.interactable = true; // habilta los botones de mascaras

            }

            masksButton.SetActive(true); // habilita el boton de cambio de mascaras

            // Desactiva el panel de cerrar porque todos los desafíos están completados
            closePanel.SetActive(false);

        }
        else
        {
            
            int completedChallenges = GameStateManager.Instance.ChallengerDoneCount;

            // Verifica y habilita los botones de acuerdo al valor de ChallengerDoneCount
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < completedChallenges)
                {
                    buttons[i].interactable = true;
                }
                else
                {
                    buttons[i].interactable = false;
                }
            }

            //acvtiva la primera mascara
            buttons[0].interactable = true;


            // Activa el panel de cerrar porque aún hay desafíos por completar
            closePanel.SetActive(true);

            // Asigna la función de cerrar al botón de cerrar
            closeButton.onClick.AddListener(ClosePanel);

            // desactivamos el boton de las mascaras adicionales
            masksButton.SetActive(false);
        }
    }

    // Función para cerrar el panel
    private void ClosePanel()
    {
        closePanel.SetActive(false);
    }
}