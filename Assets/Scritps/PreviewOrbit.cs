using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PreviewOrbit : MonoBehaviour
{
    [Header("Referencias")]
    public Camera previewCamera;          
    public RectTransform dragArea;         

    [Header("Controles")]
    public float rotateSpeed = 150f;
    public float zoomSpeed = 3f;           
    public float pinchZoomSpeed = 0.005f; 
    public float minDistance = 0.2f;
    public float maxDistance = 10f;

    [Header("Orientación inicial")]
    public float initialTiltY = 12f;       
    public float initialTiltX = 30f;      

    private float distance = 2f;
    private Vector3 lastMousePos;
    private bool dragging = false;

    
    private int activePointerId = -1;
    private Vector2 lastPointerPos;
    private float lastPinchDist = -1f;

    void Start()
    {
        if (previewCamera == null)
        {
            Debug.LogError("[PreviewOrbit] Asigna PreviewCamera.");
            enabled = false;
            return;
        }

        distance = Vector3.Distance(previewCamera.transform.position, transform.position);

        // Inclinacion inicial (ligeramente hacia abajo + yaw)
        transform.rotation = Quaternion.Euler(initialTiltX, initialTiltY, 0f);

        previewCamera.transform.LookAt(transform);
    }

    void Update()
    {
        HandleMouse();
        HandleTouch();

        // Mantener cámara mirando al pivot a la distancia actual
        Vector3 dir = (previewCamera.transform.position - transform.position).normalized;
        if (dir.sqrMagnitude < 0.001f) dir = -previewCamera.transform.forward;
        previewCamera.transform.position = transform.position + dir * distance;
        previewCamera.transform.LookAt(transform);
    }

    void HandleMouse()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Inicia/termina drag con mouse (izquierdo) sólo si está sobre el área (si se asigno)
        if (Mouse.current.leftButton.wasPressedThisFrame && IsOverDragArea(mousePos))
        {
            dragging = true;
            lastMousePos = mousePos;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            dragging = false;
        }

        // Rotacion con arrastre
        if (dragging)
        {
            Vector2 now = Mouse.current.position.ReadValue();
            Vector2 delta = now - (Vector2)lastMousePos;
            lastMousePos = now;

            transform.Rotate(Vector3.up, -delta.x * rotateSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, delta.y * rotateSpeed * Time.deltaTime, Space.Self);
        }

        // Zoom con rueda (solo si el cursor está sobre el rea asignada)
        float scroll = Mouse.current.scroll.ReadValue().y / 120f;
        if (Mathf.Abs(scroll) > 0.01f && IsOverDragArea(mousePos))
        {
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);
        }
    }

    void HandleTouch()
    {
        if (Touchscreen.current == null) return;

        // contar dedos activos y tomar hasta dos
        TouchControl t0 = null, t1 = null;
        int active = 0;
        foreach (var t in Touchscreen.current.touches)
        {
            if (t.press.isPressed)
            {
                if (active == 0) t0 = t;
                else if (active == 1) t1 = t;
                active++;
            }
        }

        if (active == 1 && t0 != null)
        {
            Vector2 p = t0.position.ReadValue();

            if (t0.press.wasPressedThisFrame && IsOverDragArea(p))
            {
                dragging = true;
                activePointerId = t0.touchId.ReadValue();
                lastPointerPos = p;
            }

            if (dragging && activePointerId == t0.touchId.ReadValue())
            {
                Vector2 now = t0.position.ReadValue();
                Vector2 delta = now - lastPointerPos;
                lastPointerPos = now;

                transform.Rotate(Vector3.up, -delta.x * rotateSpeed * Time.deltaTime, Space.World);
                transform.Rotate(Vector3.right, delta.y * rotateSpeed * Time.deltaTime, Space.Self);
            }

            if (t0.press.wasReleasedThisFrame && activePointerId == t0.touchId.ReadValue())
            {
                dragging = false;
                activePointerId = -1;
            }

            lastPinchDist = -1f; // con 1 dedo no hay pinch
        }
        else if (active >= 2 && t0 != null && t1 != null)
        {
            Vector2 p0 = t0.position.ReadValue();
            Vector2 p1 = t1.position.ReadValue();

            // interactuamos solo si al menos un dedo está sobre el adeea
            if (!IsOverDragArea(p0) && !IsOverDragArea(p1))
                return;

            float d = Vector2.Distance(p0, p1);
            if (lastPinchDist < 0f) lastPinchDist = d;
            float delta = d - lastPinchDist; // + separan → zoom IN
            lastPinchDist = d;

            distance = Mathf.Clamp(distance - delta * pinchZoomSpeed, minDistance, maxDistance);

            dragging = false; // con pinch no rotamos
        }
        else
        {
            lastPinchDist = -1f;
        }
    }

    bool IsOverDragArea(Vector2 screenPos)
    {
        if (dragArea == null) return true; // si no asignas, permite en toda la pantalla
        // Para Canvas Overlay, la camera puede ser null; también funciona.
        return RectTransformUtility.RectangleContainsScreenPoint(dragArea, screenPos, null);
    }

    // Encaje cuando instancias el modelo (llamala desde ShowViewer con los bounds del clone)
    public void FitBounds(Bounds b, float fov = 40f, float padding = 1.2f)
    {
        if (previewCamera == null) return;

        if (b.size == Vector3.zero)
            b = new Bounds(transform.position, Vector3.one * 0.1f);

        float halfFovRad = Mathf.Deg2Rad * (fov * 0.5f);
        float radiusY = b.extents.y;
        float radiusX = b.extents.x / Mathf.Max(0.0001f, previewCamera.aspect);
        float radius = Mathf.Max(radiusY, radiusX) * padding;

        float dist = radius / Mathf.Tan(halfFovRad);
        distance = Mathf.Clamp(dist, minDistance, maxDistance);

        previewCamera.transform.position = transform.position - previewCamera.transform.forward * distance;
        previewCamera.transform.LookAt(transform);
    }
}
