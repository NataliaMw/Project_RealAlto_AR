using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class FaceMaskManager : MonoBehaviour
{
    public ARFaceManager arFaceManager; // Referencia al ARFaceManager
    public GameObject[] facePrefabs; // Array de prefabs de mascaras
    private Dictionary<TrackableId, List<GameObject>> instantiatedMasks = new Dictionary<TrackableId, List<GameObject>>();

    void Start()
    {
        if (arFaceManager == null)
        {
            arFaceManager = FindObjectOfType<ARFaceManager>(); // Encuentra el ARFaceManager si no esta asignado
        }

        arFaceManager.facesChanged += OnFacesChanged; // Suscribirse al evento facesChanged
    }

    void OnDestroy()
    {
        arFaceManager.facesChanged -= OnFacesChanged; // Desuscribirse del evento facesChanged
    }

    // Metodo para mostrar la mascara correspondiente al indice proporcionado
    public void ShowMask(int index)
    {
        foreach (var face in arFaceManager.trackables)
        {
            UpdateMask(face, index); // Actualiza la mascara de cada cara
        }
    }

    // Metodo que maneja los cambios en las caras detectadas
    void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var face in eventArgs.added)
        {
            AddMasks(face); // Añade mascaras para las nuevas caras detectadas
        }

        foreach (var face in eventArgs.updated)
        {
            UpdateMask(face, -1); // Actualiza las mascaras sin cambiar la mascara actual
        }

        foreach (var face in eventArgs.removed)
        {
            RemoveMasks(face); // Elimina las mascaras para las caras removidas
        }
    }

    // Metodo para añadir mascaras a una cara detectada
    void AddMasks(ARFace face)
    {
        if (instantiatedMasks.ContainsKey(face.trackableId)) return;

        List<GameObject> masks = new List<GameObject>();
        foreach (var prefab in facePrefabs)
        {
            var mask = Instantiate(prefab, face.transform);
            mask.SetActive(false); // Inicialmente desactivado
            masks.Add(mask);
        }

        instantiatedMasks[face.trackableId] = masks;
    }

    // Metodo para actualizar la mascara de una cara
    void UpdateMask(ARFace face, int index)
    {
        if (!instantiatedMasks.ContainsKey(face.trackableId)) return;

        var masks = instantiatedMasks[face.trackableId];
        for (int i = 0; i < masks.Count; i++)
        {
            if (index == -1) break; // No cambiar la mascara actual
            masks[i].SetActive(i == index); // Activar solo la mascara correspondiente al indice
        }
    }

    // Metodo para eliminar las mascaras de una cara
    void RemoveMasks(ARFace face)
    {
        if (instantiatedMasks.TryGetValue(face.trackableId, out List<GameObject> masks))
        {
            foreach (var mask in masks)
            {
                Destroy(mask); // Destruir cada mascara
            }
            instantiatedMasks.Remove(face.trackableId); // Remover la entrada del diccionario
        }
    }
}
