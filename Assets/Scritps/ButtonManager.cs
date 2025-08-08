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
        // Habilita todos los botones de las máscaras para que sean utilizables.
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        // Habilita el botón para cambiar entre los paneles de máscaras.
        if (masksButton != null)
        {
            masksButton.SetActive(true);
        }

        // Desactiva el panel de mensaje de "desafío" ya que no es necesario.
        if (closePanel != null)
        {
            closePanel.SetActive(false);
        }
    }

    // Función para cerrar el panel
    private void ClosePanel()
    {
        closePanel.SetActive(false);
    }
}