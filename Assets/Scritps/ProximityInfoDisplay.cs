using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class ProximityInfoDisplay : MonoBehaviour
{
    // Distancia de activación en metros
    public float activationDistance = 2f;

    // Determina si el objeto está cerca
    public bool isNear = false;

    public GameObject cameraContainer; // Contenedor de la cámara principal

    private GameObject panel; // Panel que presenta la información
    private bool isPanelVisible = false; // Boolean para saber si el panel está activo
    private ObjectData dataStore; // Información del objeto para ser usada
    private DataPanelController dataPanelController; // Controlador del panel que presenta la data
    private Collider objectCollider; // Collider del objeto para detectar el toque

    private Camera mainCamera; // Cámara principal para calcular la proximidad

    void Start()
    {
        // Obtiene la referencia de la cámara principal
        mainCamera = Camera.main;

        // Obtiene el collider del objeto
        objectCollider = GetComponent<Collider>();
        if (!objectCollider)
        {
            Debug.LogError("Collider no encontrado en el objeto: " + gameObject.name);
        }
    }

    void Update()
    {
        // Calcula la distancia entre la cámara y el objeto
        float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        // Verifica si el objeto está dentro del rango de activación
        if (distanceToCamera <= activationDistance && isNear)
        {
            // Verifica si la pantalla es tocada
            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                // Crea un rayo desde la posición del toque en la pantalla
                Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());
                RaycastHit hit;

                // Realiza un raycast para verificar si el objeto fue golpeado
                if (Physics.Raycast(ray, out hit, 100))
                {
                    // Verifica si el objeto golpeado es este objeto
                    if (hit.transform == transform)
                    {
                        // Llama a la función para visualizar el panel
                        TogglePanel();

                        // Cambia el estado del booleano para determinar que se interactuó con el objeto
                        ObjectData objectInteraction = hit.transform.GetComponent<ObjectData>();
                        if (objectInteraction != null)
                        {
                            objectInteraction.Interact();
                        }
                    }
                }
            }
        }
        else if (isPanelVisible && distanceToCamera > activationDistance)
        {
            // Cierra el panel si el objeto está fuera del rango
            TogglePanel();
        }
    }

    private void TogglePanel()
    {
        // Busca el panel de información si no se ha asignado
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
        if (isPanelVisible)
        {
            dataPanelController.HidePanel(); // Oculta el panel
            isPanelVisible = false;
        }
        else
        {
            dataPanelController.ShowPanel(dataStore.title, dataStore.description, dataStore.audioClip); // Muestra el panel
            isPanelVisible = true;
        }
    }

    private void ToggleColliderState(bool state)
    {
        // Cambia el estado del collider basado en el parámetro state
        objectCollider.enabled = state;
    }
}