using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool allChallengesCompleted = false; // Booleano global para verificar si todos los desafíos están completados

    public bool tutorialCompletedThisSession = false; // Variable estática para el estado del tutorial

     public int ChallengerDoneCount = 0; // Variable estática para el estado del tutorial


    public void Awake()
    {
        // Asegura que solo exista una instancia de GameStateManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que este objeto se destruya al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

