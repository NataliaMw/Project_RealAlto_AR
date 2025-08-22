using UnityEngine;
using UnityEngine.InputSystem;

///  Pon este componente en el PIVOT (GameObject 3-D, fuera del Canvas)
public class PreviewStepRotate : MonoBehaviour
{
    [Tooltip("Cuántos grados gira cada toque")]
    public float stepDegrees = 45f;

    [Tooltip("RawImage que recibe los toques")]
    public RectTransform tapArea;   // arrastra ModelView (RawImage)

    void Update()
    {
        // --- ¿hubo TAP / clic? ---------------------------------------------
        bool tapped =
            (Touchscreen.current != null &&
             Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            ||
            (Mouse.current != null &&
             Mouse.current.leftButton.wasPressedThisFrame);
        if (!tapped) return;
        // --------------------------------------------------------------------

        // Posición del toque / ratón  (⟵ la línea corregida)
        Vector2 pos = (Touchscreen.current != null)
            ? Touchscreen.current.primaryTouch.position.ReadValue()
            : Mouse.current.position.ReadValue();

        // ¿Está sobre el RawImage?
        if (!RectTransformUtility.RectangleContainsScreenPoint(tapArea, pos, null))
            return;

        // Lado derecho ↔ izquierdo del visor
        bool rightSide = pos.x > tapArea.position.x;
        float dir = rightSide ? -1f : +1f;   // invierte si quieres lo contrario

        // Gira el modelo
        transform.Rotate(Vector3.up, dir * stepDegrees, Space.World);
    }
}
