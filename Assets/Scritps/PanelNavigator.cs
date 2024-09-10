using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelNavigator : MonoBehaviour
{
    public GameObject[] panels; // Los paneles a mostrar
    public RectTransform[] points; // Los puntos asociados a cada panel
    public Button leftButton; // Boton para ir a la izquierda
    public Button rightButton; // Boton para ir a la derecha
    public Vector2 pointScaleIncrease = new Vector2(1.1f, 1.1f); // Escala para aumentar el tamaño de los puntos
    private Vector2 originalPointScale; // La escala original de los puntos

    public Button startButton; // Botón que se activará al ver todos los paneles
    private bool[] panelsVisited; // Array para verificar si cada panel ha sido visitado


    private int currentIndex = 0; // indice del panel actual

    void Start()
    {
        // Configuración inicial
        panelsVisited = new bool[panels.Length];
        UpdatePanels();
        rightButton.onClick.AddListener(OnRightButtonClicked);
        leftButton.onClick.AddListener(OnLeftButtonClicked);

        // Referencia al scale original de los puntos
        originalPointScale = points[0].localScale;

        points[0].localScale = pointScaleIncrease;

        // Asegura que el botón start esté desactivado al inicio
        startButton.gameObject.SetActive(false);
    }

    void OnRightButtonClicked()
    {
        if (currentIndex < panels.Length - 1)
        {
            // Desactivar el panel actual y activar el siguiente
            panels[currentIndex].SetActive(false);
            currentIndex++;
            panels[currentIndex].SetActive(true);
            UpdatePoints();
            CheckIfAllPanelsVisited();
        }
    }

    void OnLeftButtonClicked()
    {
        if (currentIndex > 0)
        {
            // Desactivar el panel actual y activar el anterior
            panels[currentIndex].SetActive(false);
            currentIndex--;
            panels[currentIndex].SetActive(true);
            UpdatePoints();
            CheckIfAllPanelsVisited();
        }
    }

    private void UpdatePanels()
    {
        // Asegura que solo el primer panel esté activo al inicio
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == 0);
        }
        panelsVisited[0] = true; // Marca el primer panel como visitado
    }

    private void UpdatePoints()
    {
        // Actualizar el tamaño de los puntos
        for (int i = 0; i < points.Length; i++)
        {
            points[i].localScale = (i == currentIndex) ? pointScaleIncrease : originalPointScale;
        }
    }

    private void CheckIfAllPanelsVisited()
    {
        // Marca el panel actual como visitado
        panelsVisited[currentIndex] = true;

        // Verifica si todos los paneles han sido visitados
        bool allVisited = true;
        foreach (bool visited in panelsVisited)
        {
            if (!visited)
            {
                allVisited = false;
                break;
            }
        }

        // Si todos los paneles fueron visitados, activa el botón start
        if (allVisited)
        {
            startButton.gameObject.SetActive(true);
        }
    }
}
