using UnityEngine;
using UnityEngine.UI;

public class PanelViewerController : MonoBehaviour
{
    public Button closeButton;                         
    [HideInInspector] public ProximityInfoDisplay1 owner;  // Setea quien abre el visor

    void Awake()
    {
        if (closeButton == null)
        {
            var t = transform.Find("BtnCloseViewer");
            if (t != null) closeButton = t.GetComponent<Button>();
        }

        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);
        else
            Debug.LogWarning("[PanelViewerController] Asigna el CloseButton en el Inspector.");
    }

    public void SetOwner(ProximityInfoDisplay1 o) => owner = o;

    void OnCloseClicked()
    {
        if (owner != null)
            owner.HideViewer();      // qui se cierra bien y destruye el clone
        else
            gameObject.SetActive(false); // fallback
    }
}
