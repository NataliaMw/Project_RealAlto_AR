using UnityEngine;
using UnityEngine.UI;

public class HeartbeatEffect : MonoBehaviour
{
    // Variables públicas para configurar la escala mínima y máxima y la velocidad del latido
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public float speed = 2.0f;

    // Variable para almacenar la escala original
    private Vector3 originalScale;

    void Start()
    {
        // Guardar la escala original del objeto
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Calcular la escala utilizando Mathf.PingPong y Mathf.Lerp
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * speed, 1.0f));
        
        // Aplicar la nueva escala al objeto
        transform.localScale = originalScale * scale;
    }
}

