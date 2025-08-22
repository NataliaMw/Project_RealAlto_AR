using UnityEngine;
using UnityEngine.InputSystem;

public class ProximityInfoDisplay1 : MonoBehaviour
{
    [Header("Activación")]
    public float activationDistance = 2f;
    public bool isNear = true;

    [Header("Panel de Info (existente)")]
    public GameObject cameraContainer;
    private GameObject panel;
    private bool isPanelVisible = false;
    private ObjectData dataStore;
    private DataPanelController dataPanelController;
    private Collider objectCollider;
    private Camera mainCamera;

    [Header("Viewer 3D")]
    public GameObject panelViewer;         // Panel Viewer (UI) con el RawImage
    public Transform previewPivot;         // Pivot (hijo de PreviewStage)  <<-- Tiene PreviewOrbit
    public Camera previewCamera;           // Cámara que renderiza al RenderTexture
    public GameObject previewPrefab;       // Prefab del modelo para el visor
    public string previewLayerName = "Preview3D";

    [Tooltip("Opcional: si ya tienes agregado el controller al PanelViewer, puedes arrastrarlo aquí. Si lo dejas vacío, lo encuentra automáticamente.")]
    public PanelViewerController viewerController;

    private GameObject previewInstance;
    private int previewLayer;

    void Start()
    {
        mainCamera = Camera.main;

        objectCollider = GetComponent<Collider>();
        if (!objectCollider)
            Debug.LogError("Collider no encontrado en el objeto: " + gameObject.name);

        previewLayer = LayerMask.NameToLayer(previewLayerName);
        if (previewLayer == -1)
            Debug.LogWarning($"La capa '{previewLayerName}' no existe. Crea la Layer y configúrala en la PreviewCamera.");

        dataStore = GetComponent<ObjectData>();
        if (!dataStore)
            Debug.LogWarning($"ObjectData no encontrado en {name}.");

        // Que el visor arranque oculto
        if (panelViewer != null) panelViewer.SetActive(false);
    }

    void Update()
    {
        float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        if (distanceToCamera <= activationDistance && isNear)
        {
            // Tap/Click
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());
                if (RayHitsThis(ray)) TogglePanel();
            }
            else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (RayHitsThis(ray)) TogglePanel();
            }
        }
        else if (isPanelVisible && distanceToCamera > activationDistance)
        {
            TogglePanel(); // se alejó: cierra panel
        }
    }

    private bool RayHitsThis(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.transform == transform)
            {
                if (dataStore != null) dataStore.Interact();
                return true;
            }
        }
        return false;
    }

    private void TogglePanel()
    {
        if (!panel) panel = GameObject.Find("Panel Information");
        if (!dataPanelController && panel) dataPanelController = panel.GetComponent<DataPanelController>();

        if (panel == null || dataPanelController == null)
        {
            Debug.LogError("No se encontró 'Panel Information' o 'DataPanelController'.");
            return;
        }

        if (isPanelVisible)
        {
            dataPanelController.HidePanel();
            isPanelVisible = false;
        }
        else
        {
            // Guardamos quién abrió el panel (para que ClosePanel abra el Viewer de ESTE objeto)
            dataPanelController.lastCaller = this;

            string title = dataStore ? dataStore.title : "";
            string desc = dataStore ? dataStore.description : "";
            var clip = dataStore ? dataStore.audioClip : null;

            dataPanelController.ShowPanel(title, desc, clip);
            isPanelVisible = true;
        }
    }

    // ====== VISOR 3D ======
    public void ShowViewer()
    {
        // Cierra el panel info si seguía abierto
        if (isPanelVisible && dataPanelController != null)
        {
            dataPanelController.HidePanel();
            isPanelVisible = false;
        }

        if (panelViewer == null || previewPivot == null || previewCamera == null || previewPrefab == null)
        {
            Debug.LogError("Faltan referencias para el Viewer 3D en el Inspector.");
            return;
        }

        // Localiza el controller del viewer (si no fue asignado)
        if (viewerController == null)
            viewerController = panelViewer.GetComponent<PanelViewerController>();

        if (viewerController == null)
        {
            Debug.LogError("PanelViewer no tiene PanelViewerController. Añádelo al GameObject PanelViewer.");
            return;
        }

        // Marca el 'owner' para que el botón Cerrar llame al HideViewer correcto
        viewerController.SetOwner(this);

        panelViewer.SetActive(true);

        // Limpia instancia previa
        if (previewInstance != null) Destroy(previewInstance);

        // Instancia y resetea transform
        previewInstance = Instantiate(previewPrefab, previewPivot);



        previewInstance.transform.localPosition = Vector3.zero;
        previewInstance.transform.localRotation = Quaternion.identity;
        previewInstance.transform.localScale = Vector3.one;

        foreach (var anim in previewInstance.GetComponentsInChildren<Animator>(true))
            anim.enabled = false;

        // Ligera inclinación hacia abajo del modelo al iniciar
        previewPivot.localRotation = Quaternion.Euler(15f, 0f, 0f);

        // Pasa a la capa de preview
        if (previewLayer != -1) SetLayerRecursively(previewInstance, previewLayer);

        // Encaje/bounds + encuadre de cámara
        var rends = previewInstance.GetComponentsInChildren<Renderer>(true);
        if (rends.Length > 0)
        {
            // Bounds del modelo (mundo)
            Bounds b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);

            // 1) Centrar el modelo en el Pivot
            Vector3 worldCenter = b.center;
            Vector3 delta = previewPivot.position - worldCenter;
            previewInstance.transform.position += delta;

            // Recalcular bounds tras centrar
            b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);

            // 2) Escalar a tamaño objetivo
            float targetSize = 0.35f; // ajusta si quieres más grande/pequeño
            float maxExtent = Mathf.Max(b.extents.x, Mathf.Max(b.extents.y, b.extents.z));
            if (maxExtent > 1e-5f)
            {
                float scaleFactor = (targetSize * 0.5f) / maxExtent;
                previewInstance.transform.localScale *= scaleFactor;
            }

            // 3) Encajar cámara (que quepa en alto/ancho)
            b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);

            float half = Mathf.Max(b.extents.x, Mathf.Max(b.extents.y, b.extents.z));
            float fovRad = previewCamera.fieldOfView * Mathf.Deg2Rad;
            float dist = half / Mathf.Sin(fovRad * 0.5f);

            Vector3 dir = previewCamera.transform.forward;
            if (dir.sqrMagnitude < 0.0001f) dir = Vector3.forward;

            Vector3 center = b.center;
            previewCamera.transform.position = center - dir.normalized * (dist + 0.2f);
            previewCamera.transform.LookAt(center);
            previewCamera.nearClipPlane = 0.01f;
            previewCamera.farClipPlane = Mathf.Max(20f, (dist + 1.0f) * 4f);

            // 4) Si tienes PreviewOrbit, ajusta distancia interna
            var orbit = previewPivot.GetComponent<PreviewOrbit>();
            if (orbit != null)
            {
                orbit.FitBounds(b, previewCamera.fieldOfView, 1.15f);
            }
        }
    }

    public void HideViewer()
    {
        if (previewInstance != null) Destroy(previewInstance);
        if (panelViewer != null) panelViewer.SetActive(false);

        if (viewerController != null)
            viewerController.SetOwner(null); // limpiar
    }

    private void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform t in go.transform)
            SetLayerRecursively(t.gameObject, layer);
    }
}
