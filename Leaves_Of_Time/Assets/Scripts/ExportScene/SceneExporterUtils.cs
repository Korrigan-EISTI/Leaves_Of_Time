using UnityEngine;
using UnityEditor;
using System.Xml;

public static class SceneExportUtils
{
    public static void ExportSubHierarchy(Transform startTransform, string path)
    {
        if (startTransform == null)
        {
            Debug.LogError("Start Transform is null. Cannot export.");
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();

        // Créer l'élément racine
        XmlElement root = xmlDoc.CreateElement("Scene");
        xmlDoc.AppendChild(root);

        // Exporter la hiérarchie à partir de startTransform
        ExportGameObject(xmlDoc, root, startTransform.gameObject);

        // Sauvegarder le fichier XML
        xmlDoc.Save(path);
        Debug.Log($"Sub-hierarchy exported to {path}");
    }

    private static void ExportGameObject(XmlDocument xmlDoc, XmlElement parentElement, GameObject obj)
    {
        // Créer un élément XML pour ce GameObject
        XmlElement objElement = xmlDoc.CreateElement("GameObject");
        objElement.SetAttribute("name", obj.name);

        // Vérifier si ce GameObject est directement une instance de Prefab
        if (IsRootPrefabInstance(obj))
        {
            string prefabPath = GetPrefabPath(obj);
            if (!string.IsNullOrEmpty(prefabPath))
            {
                objElement.SetAttribute("prefab", prefabPath);
            }
        }

        // Ajouter les informations de transformation
        XmlElement transformElement = xmlDoc.CreateElement("Transform");
        transformElement.SetAttribute("position", $"{obj.transform.position.x};{obj.transform.position.y};{obj.transform.position.z}");
        transformElement.SetAttribute("rotation", $"{obj.transform.rotation.eulerAngles.x};{obj.transform.rotation.eulerAngles.y};{obj.transform.rotation.eulerAngles.z}");
        transformElement.SetAttribute("scale", $"{obj.transform.localScale.x};{obj.transform.localScale.y};{obj.transform.localScale.z}");
        objElement.AppendChild(transformElement);

        // Ajouter cet élément au parent
        parentElement.AppendChild(objElement);

        // Exporter récursivement les enfants
        foreach (Transform child in obj.transform)
        {
            ExportGameObject(xmlDoc, objElement, child.gameObject);
        }
    }

    private static string GetPrefabPath(GameObject obj)
    {
        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
        if (prefab != null)
        {
            return AssetDatabase.GetAssetPath(prefab);
        }
        return null;
    }

    private static bool IsRootPrefabInstance(GameObject obj)
    {
        return PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.Connected &&
               PrefabUtility.GetOutermostPrefabInstanceRoot(obj) == obj;
    }
}
