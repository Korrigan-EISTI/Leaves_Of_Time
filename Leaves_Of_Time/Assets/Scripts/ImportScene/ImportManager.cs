using UnityEngine;

public class SceneImportManager : MonoBehaviour
{
    public string importName = "SceneExport.xml";
    public string importPath = "Exports";
    public Transform parentTransform;

    [ContextMenu("Import Sub-Hierarchy")]
    public void ImportSubHierarchy()
    {
        if (parentTransform != null)
        {
            SceneImportUtils.ImportSubHierarchy(parentTransform, importPath + "/" + importName);
        }
        else
        {
            Debug.LogError("Parent Transform is null. Please assign it in the inspector.");
        }
    }
}
