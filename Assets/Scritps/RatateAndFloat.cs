using UnityEngine;

public class RotateAndFloat : MonoBehaviour
{
    [Header("Velocidad de rotación")]
    public float rotationSpeed = 25f; // Velocidad de rotación en el eje Z
    public float floatSpeed = 0.35f; // Velocidad del efecto de flotación
    public float floatAmplitude = 0.1f; // Amplitud del movimiento de flotación

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position; // Almacena la posición inicial del objeto
    }

    void Update()
    {
        // Rotación en el eje Z
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // Movimiento de flotación (sube y baja)
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

