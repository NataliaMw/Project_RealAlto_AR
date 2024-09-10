using UnityEngine;

public class EyesRayCast : MonoBehaviour
{
    // Distancia del raycast en metros
    public float raycastDistance = 0.6f;

    private Transform lastHitTransform = null;

    void Update()
    {
        // Crea un rayo desde la cámara hacia adelante
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Dibuja el rayo en la escena para poder visualizarlo (solo en modo Scene)
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);

        // Realiza el Raycast
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Si el raycast golpea un objeto
            if (lastHitTransform != hit.transform)
            {
                // Desactiva la variable bool en el último objeto golpeado, si existe
                if (lastHitTransform != null)
                {
                    ToggleObjectState(lastHitTransform, false);
                }

                // Activa la variable bool en el objeto actual golpeado
                ToggleObjectState(hit.transform, true);
                lastHitTransform = hit.transform;
            }
        }
        else
        {
            // Si el raycast no golpea ningún objeto, desactiva la variable bool en el último objeto golpeado
            if (lastHitTransform != null)
            {
                ToggleObjectState(lastHitTransform, false);
                lastHitTransform = null;
            }
        }
    }

    private void ToggleObjectState(Transform target, bool state)
    {
        // Intenta obtener el componente del script en el objeto golpeado
        var interactable = target.GetComponent<ProximityInfoDisplay>();
        if (interactable != null)
        {
            // Cambia el valor de la variable bool en el objeto
            interactable.isNear = state;
        }
    }
}
