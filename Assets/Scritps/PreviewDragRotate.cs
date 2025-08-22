using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Permite rotar el modelo 3D con **mouse** (clic-izq) o **touch** (arrastre) sobre el RawImage del visor.
/// Colócalo en el Pivot que tengas dentro de “PreviewStage”.
/// </summary>
public class PreviewDragRotate : MonoBehaviour
{
    [Header("Referencias")]
    public Camera previewCamera;     // la cámara que escribe en el RenderTexture
    public RectTransform dragArea;          // el RawImage (RectTransform) del visor

    [Header("Ajustes")]
    public float rotateSpeed = 180f;           // grados/segundo
    public bool invertY = false;          // invierte eje vertical si lo prefieres

    // -------------------- internos --------------------
    bool dragging = false;
    Vector2 lastPos;

    void Awake()
    {
        if (previewCamera == null)
        {
            Debug.LogError("[PreviewDragRotate] Asigna PreviewCamera en el Inspector");
            enabled = false;
        }
        if (dragArea == null)
            Debug.LogWarning("[PreviewDragRotate] dragArea vacío: podrás arrastrar en cualquier parte de la pantalla");
    }

    void Update()
    {
        HandleInput();
    }

    // ===== entrada (mouse + touch) =====
    void HandleInput()
    {
        // ---------- iniciar / terminar arrastre ----------
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            // dedo abajo sobre el visor
            if (touch.press.wasPressedThisFrame && OverArea(touch.position.ReadValue()))
            {
                dragging = true;
                lastPos = touch.position.ReadValue();
            }
            else if (touch.press.wasReleasedThisFrame)
                dragging = false;
        }
        else if (Mouse.current != null)
        {
            // mouse botón izq sobre el visor
            if (Mouse.current.leftButton.wasPressedThisFrame &&
                OverArea(Mouse.current.position.ReadValue()))
            {
                dragging = true;
                lastPos = Mouse.current.position.ReadValue();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
                dragging = false;
        }

        // ---------- rotación mientras arrastras ----------
        if (dragging)
        {
            Vector2 now = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed
                          ? Touchscreen.current.primaryTouch.position.ReadValue()
                          : Mouse.current.position.ReadValue();

            Vector2 delta = now - lastPos;
            lastPos = now;

            float yaw = -delta.x * rotateSpeed * Time.deltaTime;             // giro horizontal
            float pitch = (invertY ? delta.y : -delta.y) * rotateSpeed * Time.deltaTime; // vertical

            transform.Rotate(Vector3.up, yaw, Space.World);  // rota alrededor de Y global
            transform.Rotate(Vector3.right, pitch, Space.Self);   // rota alrededor de X local
        }
    }

    // ---------- comprueba si el puntero/touch está sobre el RawImage ----------
    bool OverArea(Vector2 screenPos)
    {
        if (dragArea == null)          // sin área = siempre permitido
            return true;

        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragArea, screenPos, null, out local);

        return dragArea.rect.Contains(local);
    }
}
