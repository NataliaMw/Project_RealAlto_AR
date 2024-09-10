using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class MaskManager : MonoBehaviour
{
    public GameObject xrOrigin; // El XR Origin que contiene el ARFaceManager
    public GameObject[] facePrefabs; // Arreglo con los diferentes prefabs de mascaras
    private ARFaceManager faceManager;
    
    // Metodos para cambiar la mascara (pueden ser llamados por los botones UI)
    public void ChangeToMask1()
    {
        ChangeMask(0);
    }

    public void ChangeToMask2()
    {
        ChangeMask(1);
    }

    public void ChangeToMask3()
    {
        ChangeMask(2);
    }

    public void ChangeToMask4()
    {
        ChangeMask(3);
    }

    // Metodo para cambiar la máscara
    private void ChangeMask(int maskIndex)
    {
        if (maskIndex < 0 || maskIndex >= facePrefabs.Length)
        {
            Debug.LogError("Mask index out of range.");
            return;
        }

        // Si ya hay un ARFaceManager, elimínalo
        if (faceManager != null)
        {
            Destroy(faceManager);
            faceManager = null;
        }

        // Agrega un nuevo ARFaceManager con el prefab seleccionado
        faceManager = xrOrigin.AddComponent<ARFaceManager>();

        // Forzar una actualización para detectar nuevamente las caras y aplicar el nuevo prefab
        ReapplyMasks();
        
        faceManager.facePrefab = facePrefabs[maskIndex];

        
    }

    private void ReapplyMasks()
    {
        // Copiar las caras actuales en una lista
        var faces = new List<ARFace>(faceManager.trackables.count);

        foreach (var face in faceManager.trackables)
        {
            faces.Add(face);
        }

        // Destruir todas las caras existentes
        foreach (var face in faces)
        {
            Destroy(face.gameObject);
        }

        // Forzar una actualización para detectar nuevamente las caras y aplicar el nuevo prefab
        faceManager.enabled = false;
        faceManager.enabled = true;
    }
}
