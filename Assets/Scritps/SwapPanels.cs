using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapPanels : MonoBehaviour
{
    public RectTransform panel1; // Referencia a la primera lista de mascaras
    public RectTransform panel2; // Referencia a la segunda lista de mascaras
    public Button swapButton; // Referencia al boton para intercambiar las posiciones
    public Sprite alternateSprite; // La otra imagen para alternar
    private Sprite originalSprite; // La imagen original del boton
    public float animationDuration = 0.5f; // Duracion de la animacion en segundos

    private bool isAnimating = false; // Para evitar animaciones multiples simultaneas
    private bool isAlternateSprite = false; // Para rastrear el estado de la imagen del boton

    void Start()
    {
        if (swapButton != null)
        {            
            swapButton.onClick.AddListener(() => StartCoroutine(Swap())); // Suscribirse al evento onClick del boton
            originalSprite = swapButton.transform.GetChild(0).GetComponent<Image>().sprite; // Obtener la imagen original del boton
            
        }
    }

    // Corrutina para intercambiar las posiciones de las dos imagenes con animacion
    private IEnumerator Swap()
    {
        if (isAnimating) yield break; // Salir si ya se esta ejecutando una animacion

        isAnimating = true;

        Vector3 initialPos1 = panel1.anchoredPosition; // Posicion inicial de la primera imagen
        Vector3 initialPos2 = panel2.anchoredPosition; // Posicion inicial de la segunda imagen

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            // Interpolar las posiciones de las imagenes
            panel1.anchoredPosition = Vector3.Lerp(initialPos1, initialPos2, t);
            panel2.anchoredPosition = Vector3.Lerp(initialPos2, initialPos1, t);

            yield return null; // Esperar al siguiente frame
        }

        // Asegurar que las posiciones finales sean exactas
        panel1.anchoredPosition = initialPos2;
        panel2.anchoredPosition = initialPos1;

        // Alternar la imagen del hijo del boton
        changeIcon();

    }

    public void changeIcon()
    {
        var buttonChildImage = swapButton.transform.GetChild(0).GetComponent<Image>();
        buttonChildImage.sprite = isAlternateSprite ? originalSprite : alternateSprite;
        isAlternateSprite = !isAlternateSprite;

        isAnimating = false;
    }
}
