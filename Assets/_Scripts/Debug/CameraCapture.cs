using UnityEngine;
using System.IO;

public class CameraCapture : MonoBehaviour
{
    public Camera targetCamera; // Assign the camera in the inspector
    public int width = 1920;
    public int height = 1080;
    public string saveFolder = "CapturedImages"; // Will be inside Assets/


    [ContextMenu("Capture Screenshot")]
    private void CaptureScreenshotButton()
    {
        CaptureAndSave();
    }

    public void CaptureAndSave()
    {
        // 1. Create RenderTexture
        RenderTexture rt = new RenderTexture(width, height, 24);
        targetCamera.targetTexture = rt;

        // 2. Render camera to RenderTexture
        RenderTexture.active = rt;
        targetCamera.Render();

        // 3. Read pixels into Texture2D
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        // 4. Reset camera
        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // 5. Encode to PNG
        byte[] bytes = screenShot.EncodeToPNG();

        // 6. Create folder inside Assets if it doesn’t exist
        string folderPath = Path.Combine(Application.dataPath, saveFolder);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // 7. Save file with timestamp
        string filePath = Path.Combine(folderPath, $"Capture_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"Saved screenshot to: {filePath}");

        // Optional: Refresh Unity Editor so the new file appears immediately
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
