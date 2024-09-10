using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class ProximityInfoDisplay : MonoBehaviour
{
    // Distancia de activacion en metros
    public bool isNear = false;

    public GameObject cameraContainer;

    private GameObject panel; // Panel que presenta la informacion
    private bool isPanelVisible = false; // Boolean para saber si el panel esta activo
    private ObjectData dataStore; // Informacion de objeto para ser usada
    private DataPanelController dataPanelController; // Controlador del panel que presenta la data
    private Collider objectCollider; // Collider del objeto para detectar el toque

    void Start()
    {
        // Obtiene el collider del objeto
        objectCollider = GetComponent<Collider>();
        if (!objectCollider)
        {
            Debug.LogError("Collider no encontrado en el objeto: " + gameObject.name);
        }
    }

    void Update()
    {
        // Verifica si el objeto esta dentro del rango de activacion
        if (isNear)
        {
            // Verifica si la pantalla es tocada
            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {

                // Crea un rayo desde la posicion del toque en la pantalla
                Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());                
                
                RaycastHit hit;

                // Realiza un raycast para verificar si el objeto fue golpeado
                if (Physics.Raycast(ray, out hit, 100))
                {
                    // Verifica si el objeto golpeado es este objeto
                    if (hit.transform == transform)
                    {
                        // Llama a la funcion para visualizar el panel
                        TogglePanel();

                        // Cambia el estado del booleano para determinar que se interactuo con el objeto
                        ObjectData objectInteraction = hit.transform.GetComponent<ObjectData>();
                        if (objectInteraction != null)
                        {
                            objectInteraction.Interact();
                        }
                    }
                }
            }
        }
        // else if (isPanelVisible)
        // {
        //     // Si el panel esta visible pero el objeto esta fuera del rango, reactivar el collider
        //     ToggleColliderState(true);
        // }
    }

    private void TogglePanel()
    {
        // Busca el panel de informacion si no se ha asignado
        if (!panel)
        {
            panel = GameObject.Find("Panel Information");
        }

        // Busca el componente ObjectData si no se ha asignado
        if (!dataStore)
        {
            dataStore = GetComponent<ObjectData>();
        }

        // Busca el controlador del panel si no se ha asignado
        if (!dataPanelController)
        {
            dataPanelController = panel.GetComponent<DataPanelController>();
        }

        // Alterna la visibilidad del panel
        if (isPanelVisible && !isNear)
        {
            dataPanelController.HidePanel(); // Oculta el panel
            isPanelVisible = false;
        }
        else
        {
            dataPanelController.ShowPanel(dataStore.title, dataStore.description, dataStore.audioClip); // Muestra el panel
            isPanelVisible = true;
            // ToggleColliderState(false); // Desactiva el collider
        }
    }

    private void ToggleColliderState(bool state)
    {
        // Cambia el estado del collider basado en el parametro state
        objectCollider.enabled = state;
    }
}
