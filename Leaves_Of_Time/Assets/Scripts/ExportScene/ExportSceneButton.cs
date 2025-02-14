using UnityEngine;
using UnityEngine.UI;

public class ExportSceneButton : MonoBehaviour
{
    public SceneExportManager sceneExportManager;

    public void OnExportButtonClick()
    {
        if (sceneExportManager != null)
        {
            sceneExportManager.ExportScene();
        }
        else
        {
            Debug.LogError("SceneExportManager is not assigned!");
        }
    }
}
