using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelTransition : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public Button transitionButton;
    public float fadeInDuration = 1f;

    private Image panel1Image;
    private Image panel2Image;

    private void Start()
    {
        // Obtener los componentes Image de los paneles
        panel1Image = panel1.GetComponent<Image>();
        panel2Image = panel2.GetComponent<Image>();

        // Desactivar Panel2 al inicio (para que no se muestre mientras está el Panel1)
        panel2.SetActive(false);

        // Asignar la función de transición al botón
        transitionButton.onClick.AddListener(TransitionToPanel2);
    }

    public void TransitionToPanel2()
    {
        // Desactivar Panel1
        panel1.SetActive(false);

        // Activar Panel2
        panel2.SetActive(true);

        // Iniciar la animación de fade-in en Panel2
        StartCoroutine(FadeIn(panel2Image, fadeInDuration));
    }

    private IEnumerator FadeIn(Image image, float duration)
    {
        float elapsedTime = 0f;
        Color color = image.color;
        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 1f; // Asegurarse de que la transparencia sea 1 al final de la animación
        image.color = color;
    }
}
