using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseApp : MonoBehaviour
{
    public GameObject actionPanel; // El panel con los botones de accion
    public Button showPanelButton; // El boton que mostrará el panel de accion
    public Button continueButton; // El boton para continuar en la aplicacion
    public Button quitButton; // El boton para cerrar la aplicacion

    private void Start()
    {
        // Asegurarse de que el panel de accion esté desactivado al inicio
        actionPanel.SetActive(false);

        // Asignar las funciones a los botones
        showPanelButton.onClick.AddListener(ShowActionPanel);
        continueButton.onClick.AddListener(ContinueApp);
        quitButton.onClick.AddListener(QuitApp);
    }

    private void ShowActionPanel()
    {
        // Mostrar el panel de accion
        actionPanel.SetActive(true);
    }

    private void ContinueApp()
    {
        // Continuar en la aplicacion (esconder el panel de accion)
        actionPanel.SetActive(false);
    }

    private void QuitApp()
    {
        // Cerrar la aplicacion
        Application.Quit();
    }
}
