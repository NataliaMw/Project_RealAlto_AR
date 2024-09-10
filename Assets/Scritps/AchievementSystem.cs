using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : MonoBehaviour
{
    // Clase para representar un logro con una pregunta y un objeto objetivo
    [System.Serializable]
    public class Achievement
    {
        public string challenge; // Pregunta o desafio del logro
        public GameObject targetObject; // Objeto asociado al logro
    }

    [Header("Lista de Desafios y Objetos")]
    public List<string> challengeTexts = new List<string>(); // Lista de textos de logro
    public List<GameObject> challengeObjects = new List<GameObject>(); // Lista de objetos relacionados con los logro

    [Header("Lista de Elemtos UI")]
    public List<Text> challengeTextUI = new List<Text>(); // Lista de elementos UI para mostrar textos de logro
    public List<Image> challengeImageUI = new List<Image>(); // Lista de elementos UI para mostrar la imagen del objeto
    public List<Image> challengeStatusUI = new List<Image>(); // Lista de elementos UI para mostrar el estado del logro

    public Sprite checkSprite; // Sprite que se muestra cuando el logro esta completado
    private Color checkColor = Color.green;
    private Color nullColor = Color.red;
    public Sprite xSprite; // Sprite que se muestra cuando el logro no esta completado

    [Header("Mensaje Popup")]
    public RectTransform achievementPanel; // Panel que muestra la animacion de logro
    public Vector2 offScreenPosition = new Vector2(0, 0); // Posicion inicial fuera de la pantalla
    public Vector2 onScreenPosition = new Vector2(0, 0); // Posicion final en la pantalla
    public float animationDuration = 0.75f; // Duracion de la animacion
    public float displayDuration = 2.0f; // Tiempo que el panel permanece en pantalla


    public RectTransform completePanel; // Panel que muestra la animacion de Desafio Completado
    private bool[] challengeCompletionStatus; // array de boolenos para verificar si se cumpli cada logro
    private bool achievementShown = false; // Booleano para verificar si el panel de logro ya se ha mostrado
    // public Image ChallengeSprite; // Sprite que se muestra cuando el desafio esta completado
    private bool isAnimating = false; // Flag para verificar si el panel esta en animacion

    public RectTransform rewardPanel; // Panel que muestra la animacion de Desafio Completado


    void Start()
    {
        // Inicializa la UI con los textos de los desafios y establece los sprites predeterminados
        for (int i = 0; i < challengeTexts.Count; i++)
        {
            challengeTextUI[i].text = challengeTexts[i]; // Asigna el texto del desafio al elemento UI correspondiente


            // Obtén el componente ObjectData del objeto
            ObjectData objectData = challengeObjects[i].GetComponent<ObjectData>();

            if (objectData != null && objectData.objectImage != null)
            {
                // Asigna el sprite del objeto al elemento UI correspondiente
                challengeImageUI[i].sprite = objectData.objectImage;

                //cambiamos el color a la imagen del status
                challengeStatusUI[i].color = nullColor;
            }
            else
            {
                // Si el objeto no tiene un sprite asignado, usa un sprite predeterminado, como una equis
                challengeImageUI[i].sprite = xSprite;
                challengeImageUI[i].color = nullColor;
            }
        }

        challengeCompletionStatus = new bool[challengeObjects.Count]; // se le da el tamano del array de acuerdo a la cantidad e desafios
    }

    void Update()
    {
        // Verifica si todos los desafios estan completados
        if (GameStateManager.Instance.allChallengesCompleted && !achievementShown)
        {
            // Muestra el panel de "Desafio Completado" si no esta en animacion
            if (!isAnimating)
            {
                StartCoroutine(ShowAchievementPanelOn(completePanel, 2.5f));

                // ChallengeSprite.color = checkColor;

                achievementShown = true; // Marca que el panel de logro ya se ha mostrado, solo para el panel completo

            }
        }
        else
        {
            // Verifica el estado de todos los logros
            CheckAllAchievements();
        }
    }

    public void CheckAllAchievements()
    {

        bool updated = false; // Flag para verificar si algun desafio ha sido completado

        for (int i = 0; i < challengeObjects.Count; i++)
        {
            if (challengeObjects[i] != null)
            {
                // Obtiene el componente ObjectData del objeto de desafio
                ObjectData objectInteraction = challengeObjects[i].GetComponent<ObjectData>();

                if (objectInteraction != null && objectInteraction.isInteracted)
                {
                    // Si el objeto ha sido interactuado, actualiza la UI
                    if (!challengeCompletionStatus[i])
                    {
                        challengeStatusUI[i].sprite = checkSprite; // Cambia el sprite a "completado"
                        challengeStatusUI[i].color = checkColor; // Cambia el color del sprite
                        challengeCompletionStatus[i] = true; // Marca el desafio como completado

                        updated = true; // Marca que hubo una actualizacion

                        // Muestra la animacion del panel de logro
                        StartCoroutine(ShowAchievementPanelOn(achievementPanel));
                    }
                }
            }
        }

        // Solo verifica si todos los desafios estan completados si hubo una actualizacion
        if (updated)
        {
            CheckChallenges();
            GameStateManager.Instance.ChallengerDoneCount = CountCompletedChallenges();
            CheckSpecificRewards(GameStateManager.Instance.ChallengerDoneCount);
        }
    }

    public void CheckChallenges()
    {
        GameStateManager.Instance.allChallengesCompleted = true; // Asume que todos los desafios estan completados

        // Recorre el array de estado de los desafios
        for (int i = 0; i < challengeCompletionStatus.Length; i++)
        {
            if (!challengeCompletionStatus[i])
            {
                GameStateManager.Instance.allChallengesCompleted = false; // Si hay algun desafio no completado, marca como falso
                break; // Sale del bucle si encuentra un desafio no completado
            }
        }
    }

    IEnumerator ShowAchievementPanelOn(RectTransform panel, float timer = -1f)
    {
        //Si customDisplayDuration es menor que 0, se usa displayDuration
        float durationToUse = timer > 0 ? timer : displayDuration;


        isAnimating = true; // Marca que se esta realizando una animacio

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(offScreenPosition, onScreenPosition, Mathf.SmoothStep(0f, 1f, elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panel.anchoredPosition = onScreenPosition;

        yield return new WaitForSeconds(timer);

        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(onScreenPosition, offScreenPosition, Mathf.SmoothStep(0f, 1f, elapsedTime / animationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panel.anchoredPosition = offScreenPosition;

        isAnimating = false; // Marca que la animacion ha terminado

    }

    // Función para contar cuántos desafíos han sido completados
    private int CountCompletedChallenges()
    {
        int completedCount = 0;
        for (int i = 0; i < challengeCompletionStatus.Length; i++)
        {
            if (challengeCompletionStatus[i])
            {
                completedCount++;
            }
        }
        return completedCount;
    }

     // Función para verificar y mostrar recompensas específicas
    private void CheckSpecificRewards(int completedCount)
    {
        if (completedCount == 1 || completedCount == 3 || completedCount == 5)
        {
            // Muestra el panel de recompensa si se cumple 1, 3 o 5 logros
            StartCoroutine(ShowAchievementPanelOn(rewardPanel));
        }
    }
}
