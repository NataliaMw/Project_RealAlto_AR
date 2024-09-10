using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;

public class CaptureImage : MonoBehaviour
{
    public Camera arCamera;  // Camara que se usara para capturar la imagen
    public AudioSource audioSource; // Componente AudioSource para reproducir el sonido
    public AudioClip captureSound; // Clip de sonido para la captura
    public Image whiteScreenEffect; // Imagen blanca para el efecto de pantalla blanca

    void Start()
    {
        // Asegurarse de que la camara esté asignada
        if (arCamera == null)
        {
            arCamera = Camera.main;
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); // Encontrar el AudioSource si no esta asignado
        }
        if (whiteScreenEffect != null)
        {
            whiteScreenEffect.gameObject.SetActive(false); // Asegurarse de que el efecto blanco esté desactivado al inicio
        }

    }

    // Metodo publico que puede ser llamado para capturar una imagen
    public void Capture()
    {
        // Inicia una corrutina para capturar la imagen
        StartCoroutine(CapturePhotoCoroutine());
    }

    // Corrutina que realiza la captura de la imagen
    private IEnumerator CapturePhotoCoroutine()
    {
        
        // Reproducir el sonido de captura
        if (audioSource != null && captureSound != null)
        {
            audioSource.PlayOneShot(captureSound);
        }

        // Activar el efecto de pantalla blanca
        if (whiteScreenEffect != null)
        {
            whiteScreenEffect.gameObject.SetActive(true);

            // Esperar un breve período para mostrar el efecto blanco
            yield return new WaitForSeconds(0.075f);

            // Desactivar el efecto de pantalla blanca
            whiteScreenEffect.gameObject.SetActive(false);
        }

        // Espera hasta el final del frame para asegurar que la captura sea precisa
        yield return new WaitForEndOfFrame();

        // Crear un RenderTexture para la camara
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        arCamera.targetTexture = renderTexture;
        
        // Crear una textura 2D con las mismas dimensiones que la pantalla
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        
        // Renderizar la vista de la camara en el RenderTexture
        arCamera.Render();
        
        // Activar el RenderTexture y leer los pixeles en la textura 2D
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        
        // Desactivar el RenderTexture y limpiar
        arCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Codificar la textura en formato PNG
        byte[] bytes = screenShot.EncodeToPNG();

        // Generar el nombre del archivo con la fecha y hora actual
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"photo_{timestamp}.png";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        
        // Guardar la imagen en el directorio persistente de la aplicacion
        File.WriteAllBytes(filePath, bytes);

        // Llamar al metodo para guardar la imagen en la galeria
        SaveToGallery(filePath, fileName);

        // Mensaje de depuracion para indicar que la foto se ha guardado
        Debug.Log($"Photo saved at: {filePath}");
    }

    // Metodo para guardar la imagen en la galeria del dispositivo
    private void SaveToGallery(string filePath, string fileName)
    {
        // Crear el directorio para las imagenes en la galeria si no existe
        string galleryPath = Path.Combine(Application.persistentDataPath, "..", "Pictures", "Real Alto");
        if (!Directory.Exists(galleryPath))
        {
            Directory.CreateDirectory(galleryPath);
        }

        // Copiar la imagen al directorio de la galeria
        string destinationPath = Path.Combine(galleryPath, fileName);
        File.Copy(filePath, destinationPath, true);

        // Mensaje de depuracion para indicar que la foto se ha copiado a la galeria
        Debug.Log($"Photo copied to gallery at: {destinationPath}");
    }
}
