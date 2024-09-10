using UnityEngine;
using UnityEngine.UI;

public class ObjectData : MonoBehaviour
{
    [Header("Information")]
    public AudioClip audioClip; // Audio clip del objeto
    public string title; // Titulo del objeto
    public string description; // Descripcion del objeto
    [Header("Image")]
    public Sprite objectImage; // image del objeto
    [Header("Booleans")]
    public bool isInteracted = false; // bool para saber si se interactuo con el objeto

    public void Interact()
    {
        isInteracted = !isInteracted;
    }
}
