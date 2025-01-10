using UnityEngine;

public class EyesRayCast : MonoBehaviour
{
    // Distancia del raycast en metros
    public float raycastDistance = 2f; 

    private Transform lastHitTransform = null; // Último objeto golpeado por el raycast

    void Update()
    {
        // Crea un rayo desde la posición de la cámara hacia adelante
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Dibuja el rayo en la escena para visualizarlo (solo en modo Scene)
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);

        // Realiza el raycast dentro del rango especificado
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Si el raycast golpea un nuevo objeto
            if (lastHitTransform != hit.transform)
            {
                // Desactiva el estado del último objeto golpeado, si existe
                if (lastHitTransform != null)
                {
                    ToggleObjectState(lastHitTransform, false);
                }

                // Activa el estado del nuevo objeto golpeado
                ToggleObjectState(hit.transform, true);
                lastHitTransform = hit.transform; // Actualiza el último objeto golpeado
            }
        }
        else
        {
            // Si el raycast no golpea ningún objeto
            if (lastHitTransform != null)
            {
                ToggleObjectState(lastHitTransform, false); // Desactiva el estado del último objeto golpeado
                lastHitTransform = null; // Resetea la referencia
            }
        }
    }

    private void ToggleObjectState(Transform target, bool state)
    {
        // Intenta obtener el componente ProximityInfoDisplay del objeto golpeado
        var interactable = target.GetComponent<ProximityInfoDisplay>();
        if (interactable != null)
        {
            // Cambia el valor de la variable isNear en el objeto
            interactable.isNear = state;
        }
    }
}