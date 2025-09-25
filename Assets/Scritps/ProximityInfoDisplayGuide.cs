using UnityEngine;
using UnityEngine.InputSystem;

public class ProximityInfoDisplayGuide : MonoBehaviour
{
    [Header("Proximity Settings")]
    public float activationDistance = 3f;
    public float interactionDistance = 1.5f; // Distancia más cercana para interacción directa
    public bool isNear = false;

    [Header("References")]
    public GameObject cameraContainer;
    private GameObject panel;
    private bool isPanelVisible = false;
    private ObjectData dataStore;
    private DataPanelController dataPanelController;
    private Collider objectCollider;
    private Camera mainCamera;

    [Header("Guide Specific")]
    private GuiaTuristicoController guideController;
    private bool userHasInteracted = false;
    private float lastInteractionTime;
    private float interactionCooldown = 3f; // Evitar spam de interacciones

    void Start()
    {
        // Obtener referencias
        mainCamera = Camera.main;
        objectCollider = GetComponent<Collider>();
        guideController = GetComponent<GuiaTuristicoController>();
        dataStore = GetComponent<ObjectData>();

        if (!objectCollider)
        {
            Debug.LogError("Collider no encontrado en el GuíaTurístico: " + gameObject.name);
        }

        if (!guideController)
        {
            Debug.LogError("GuiaTuristicoController no encontrado en: " + gameObject.name);
        }
    }

    void Update()
    {
        if (!mainCamera) return;

        // Calcular distancia a la cámara
        float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        // Verificar proximidad para activar/desactivar near state
        bool wasNear = isNear;
        isNear = distanceToCamera <= activationDistance;

        // Si el usuario se acerca por primera vez
        if (isNear && !wasNear)
        {
            OnUserApproach();
        }
        // Si el usuario se aleja
        else if (!isNear && wasNear)
        {
            OnUserLeave();
        }

        // Manejar interacciones dentro del rango
        if (isNear && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            HandleTouchInput();
        }

        // Cerrar panel si el usuario se aleja mucho
        if (isPanelVisible && distanceToCamera > activationDistance * 1.5f)
        {
            HidePanel();
        }
    }

    private void HandleTouchInput()
    {
        // Evitar spam de interacciones
        if (Time.time - lastInteractionTime < interactionCooldown)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform == transform)
            {
                float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

                if (distanceToCamera <= interactionDistance)
                {
                    // Interacción directa - mostrar panel informativo
                    OnDirectInteraction();
                }
                else if (distanceToCamera <= activationDistance)
                {
                    // Interacción a distancia - activar comportamiento del guía
                    OnDistantInteraction();
                }

                lastInteractionTime = Time.time;
            }
        }
    }

    private void OnUserApproach()
    {
        Debug.Log("Usuario se acerca al GuíaTurístico");

        if (guideController != null)
        {
            // Hacer que el guía reaccione a la aproximación del usuario
            guideController.SetHappy();
        }
    }

    private void OnUserLeave()
    {
        Debug.Log("Usuario se aleja del GuíaTurístico");

        if (isPanelVisible)
        {
            HidePanel();
        }

        if (guideController != null)
        {
            guideController.OnUserLeft();
        }

        userHasInteracted = false;
    }

    private void OnDirectInteraction()
    {
        Debug.Log("Interacción directa con GuíaTurístico");

        // Mostrar panel con información del guía
        ShowPanel();

        // Activar comportamiento especial del guía
        if (guideController != null)
        {
            guideController.OnUserInteraction();
        }

        // Marcar como interactuado
        if (dataStore != null)
        {
            dataStore.Interact();
        }

        userHasInteracted = true;
    }

    private void OnDistantInteraction()
    {
        Debug.Log("Interacción a distancia con GuíaTurístico");

        // Solo activar comportamiento del guía sin mostrar panel
        if (guideController != null)
        {
            guideController.SetHappy();
        }
    }

    private void ShowPanel()
    {
        if (isPanelVisible) return;

        // Buscar panel si no está asignado
        if (!panel)
        {
            panel = GameObject.Find("Panel Information");
        }

        if (!dataPanelController && panel)
        {
            dataPanelController = panel.GetComponent<DataPanelController>();
        }

        if (dataPanelController && dataStore)
        {
            // Para el guía, no mostrar el botón de explorar objeto (no es un objeto estático)
            if (dataPanelController.lastCaller != null)
            {
                dataPanelController.lastCaller = null;
            }

            dataPanelController.ShowPanel(dataStore.title, dataStore.description, dataStore.audioClip);
            isPanelVisible = true;

            Debug.Log("Panel mostrado para GuíaTurístico");
        }
    }

    private void HidePanel()
    {
        if (!isPanelVisible) return;

        if (dataPanelController)
        {
            dataPanelController.HidePanel();
            isPanelVisible = false;

            Debug.Log("Panel ocultado para GuíaTurístico");
        }
    }

    private void TogglePanel()
    {
        if (isPanelVisible)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }

    // Método público para pausar interacciones (usado por otros scripts)
    public void PauseInteractions()
    {
        enabled = false;

        if (guideController != null)
        {
            guideController.PauseBehavior();
        }
    }

    // Método público para reanudar interacciones
    public void ResumeInteractions()
    {
        enabled = true;

        if (guideController != null)
        {
            guideController.ResumeBehavior();
        }
    }

    // Método para verificar si el usuario está en rango de interacción
    public bool IsUserInRange()
    {
        if (!mainCamera) return false;

        float distance = Vector3.Distance(transform.position, mainCamera.transform.position);
        return distance <= activationDistance;
    }

    // Método para forzar una interacción (usado por otros scripts)
    public void ForceInteraction()
    {
        OnDirectInteraction();
    }
}