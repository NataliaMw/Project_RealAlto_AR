using UnityEngine;
using System.Collections;

public class GuiaTuristicoController : MonoBehaviour
{
    [Header("Animation Control")]
    private Animator animator;
    private bool isTalking = false;
    private bool isHappy = false;

    [Header("Tour Guide Behavior")]
    public float greetingDelay = 2f; // Tiempo antes de saludar
    public float talkingDuration = 5f; // Duración de animación de habla
    public float idleTime = 3f; // Tiempo en idle antes de ser feliz

    [Header("Audio Management")]
    public AudioClip[] tourGreetings; // Múltiples saludos
    public AudioClip[] tourInformation; // Información del tour
    private AudioSource audioSource;

    private Coroutine currentBehaviorCoroutine;
    private ProximityInfoDisplay proximityDisplay;
    private ObjectData objectData;

    void Start()
    {
        // Obtener componentes necesarios
        animator = GetComponent<Animator>();
        proximityDisplay = GetComponent<ProximityInfoDisplay>();
        objectData = GetComponent<ObjectData>();
        audioSource = GetComponent<AudioSource>();

        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar audio source
        audioSource.playOnAwake = false;
        audioSource.volume = 0.8f;

        // Iniciar comportamiento automático del guía
        StartGuideAutomaticBehavior();
    }

    void Update()
    {
        // Actualizar parámetros del animator
        if (animator)
        {
            animator.SetBool("IsTalking", isTalking);
            animator.SetBool("IsHappy", isHappy);
        }
    }

    private void StartGuideAutomaticBehavior()
    {
        if (currentBehaviorCoroutine != null)
        {
            StopCoroutine(currentBehaviorCoroutine);
        }
        currentBehaviorCoroutine = StartCoroutine(GuideAutomaticBehaviorLoop());
    }

    private IEnumerator GuideAutomaticBehaviorLoop()
    {
        while (true)
        {
            // Estado idle inicial
            SetIdle();
            yield return new WaitForSeconds(idleTime);

            // Estado feliz/saludando
            SetHappy();
            yield return new WaitForSeconds(greetingDelay);

            // Estado hablando (información del tour)
            StartTalking();
            yield return new WaitForSeconds(talkingDuration);

            // Volver a idle
            StopTalking();
            yield return new WaitForSeconds(2f);
        }
    }

    public void SetIdle()
    {
        isTalking = false;
        isHappy = false;
    }

    public void SetHappy()
    {
        isTalking = false;
        isHappy = true;

        // Reproducir saludo aleatorio
        if (tourGreetings != null && tourGreetings.Length > 0)
        {
            int randomIndex = Random.Range(0, tourGreetings.Length);
            if (tourGreetings[randomIndex] != null)
            {
                audioSource.clip = tourGreetings[randomIndex];
                audioSource.Play();
            }
        }
    }

    public void StartTalking()
    {
        isTalking = true;
        isHappy = false;

        // Reproducir información del tour
        if (tourInformation != null && tourInformation.Length > 0)
        {
            int randomIndex = Random.Range(0, tourInformation.Length);
            if (tourInformation[randomIndex] != null)
            {
                audioSource.clip = tourInformation[randomIndex];
                audioSource.Play();
            }
        }
    }

    public void StopTalking()
    {
        isTalking = false;
    }

    // Método llamado cuando el usuario interactúa con el guía
    public void OnUserInteraction()
    {
        // Detener comportamiento automático temporalmente
        if (currentBehaviorCoroutine != null)
        {
            StopCoroutine(currentBehaviorCoroutine);
        }

        // Iniciar secuencia de interacción personalizada
        StartCoroutine(InteractionSequence());
    }

    private IEnumerator InteractionSequence()
    {
        // Saludar al usuario
        SetHappy();
        yield return new WaitForSeconds(2f);

        // Hablar con información específica
        StartTalking();
        yield return new WaitForSeconds(talkingDuration + 2f);

        // Volver al comportamiento automático
        StopTalking();
        yield return new WaitForSeconds(1f);

        StartGuideAutomaticBehavior();
    }

    // Método para cuando el usuario se aleja
    public void OnUserLeft()
    {
        // Gesto de despedida
        StartCoroutine(FarewellSequence());
    }

    private IEnumerator FarewellSequence()
    {
        SetHappy();
        yield return new WaitForSeconds(1f);

        // Continuar con comportamiento normal
        StartGuideAutomaticBehavior();
    }

    // Métodos públicos para controlar desde otros scripts
    public void PauseBehavior()
    {
        if (currentBehaviorCoroutine != null)
        {
            StopCoroutine(currentBehaviorCoroutine);
            currentBehaviorCoroutine = null;
        }
        SetIdle();
    }

    public void ResumeBehavior()
    {
        StartGuideAutomaticBehavior();
    }

    void OnDestroy()
    {
        if (currentBehaviorCoroutine != null)
        {
            StopCoroutine(currentBehaviorCoroutine);
        }
    }
}