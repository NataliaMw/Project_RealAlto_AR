using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanel : MonoBehaviour
{
    public RectTransform panel; // Referencia al panel que se movera
    public Button openButton; // Boton para activar el movimiento
    public Button closeButton; // Boton para volver a la posicion inicial

    public Vector2 offScreenPosition; // Posicion inicial fuera de la pantalla
    public Vector2 onScreenPosition; // Posicion objetivo en la pantalla
    public float moveDuration = 0.5f; // Duracion del movimiento

    void Start()
    {
        // Mover el panel fuera de la pantalla al inicio
        panel.anchoredPosition = offScreenPosition;

        // Asignar funciones a los botones
        openButton.onClick.AddListener(MovePanelOnScreen);
        closeButton.onClick.AddListener(MovePanelOffScreen);

        closeButton.gameObject.SetActive(false); // Asegúrate de que el botón 'Back' esté desactivado al inicio
    }

    void MovePanelOnScreen()
    {
        StartCoroutine(MovePanel(panel, onScreenPosition));
        openButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
    }

    void MovePanelOffScreen()
    {
        StartCoroutine(MovePanel(panel, offScreenPosition));
        
    }

    IEnumerator MovePanel(RectTransform panel, Vector2 targetPosition)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPosition;

        if (panel.anchoredPosition == offScreenPosition)
        {
            openButton.gameObject.SetActive(true);
        }
    }
}
