using UnityEngine;
using System.Xml;
using UnityEditor;

public static class SceneImportUtils
{
    /// <summary>
    /// Importe une sous-hiérarchie depuis un fichier XML et l'ajoute à un Transform donné.
    /// </summary>
    /// <param name="parentTransform">Transform parent où la hiérarchie sera ajoutée.</param>
    /// <param name="path">Chemin du fichier XML à importer.</param>
    public static void ImportSubHierarchy(Transform parentTransform, string path)
    {
        if (parentTransform == null)
        {
            Debug.LogError("Parent Transform is null. Cannot import.");
            return;
        }

        if (!System.IO.File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        // Vérifier que le fichier XML a une structure valide
        XmlElement root = xmlDoc.DocumentElement;
        if (root == null || root.Name != "Scene")
        {
            Debug.LogError("Invalid XML format: Missing root 'Scene' element.");
            return;
        }

        // Parcourir les éléments enfants et recréer la hiérarchie
        foreach (XmlNode node in root.ChildNodes)
        {
            if (node is XmlElement element && element.Name == "GameObject")
            {
                ImportGameObject(element, parentTransform);
            }
        }

        Debug.Log($"Sub-hierarchy imported from {path}");
    }

    /// <summary>
    /// Importe un GameObject et ses enfants à partir d'un élément XML.
    /// </summary>
    private static void ImportGameObject(XmlElement element, Transform parent)
    {
        // Récupérer le nom du GameObject
        string name = element.GetAttribute("name");

        // Vérifier si un prefab est spécifié
        string prefabPath = element.GetAttribute("prefab");
        GameObject newObj;

        if (!string.IsNullOrEmpty(prefabPath))
        {
            // Charger le prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                newObj = Object.Instantiate(prefab, parent);
                newObj.name = name; // Donner le nom du fichier XML
                Debug.Log($"Instantiated prefab: {prefabPath}");
            }
            else
            {
                Debug.LogWarning($"Prefab not found at path: {prefabPath}. Creating empty GameObject.");
                newObj = new GameObject(name);
                newObj.transform.parent = parent;
            }
        }
        else
        {
            // Créer un GameObject vide si aucun prefab n'est défini
            newObj = new GameObject(name);
            newObj.transform.parent = parent;
        }

        // Appliquer les transformations
        foreach (XmlNode child in element.ChildNodes)
        {
            if (child is XmlElement childElement && childElement.Name == "Transform")
            {
                ApplyTransform(newObj, childElement);
            }
        }

        // Parcourir les enfants de cet objet et les recréer
        foreach (XmlNode child in element.ChildNodes)
        {
            if (child is XmlElement childElement && childElement.Name == "GameObject")
            {
                ImportGameObject(childElement, newObj.transform);
            }
        }
    }

    /// <summary>
    /// Applique les transformations d'un élément XML à un GameObject.
    /// </summary>
    private static void ApplyTransform(GameObject obj, XmlElement transformElement)
    {
        // Lire les attributs de transformation
        string position = transformElement.GetAttribute("position");
        string rotation = transformElement.GetAttribute("rotation");
        string scale = transformElement.GetAttribute("scale");

        // Décomposer et appliquer les transformations
        if (!string.IsNullOrEmpty(position))
        {
            obj.transform.position = StringToVector3(position);
        }

        if (!string.IsNullOrEmpty(rotation))
        {
            obj.transform.rotation = Quaternion.Euler(StringToVector3(rotation));
        }

        if (!string.IsNullOrEmpty(scale))
        {
            obj.transform.localScale = StringToVector3(scale);
        }
    }

    /// <summary>
    /// Convertit une chaîne de caractères en Vector3.
    /// </summary>
    private static Vector3 StringToVector3(string str)
    {
        string[] parts = str.Split(';');
        if (parts.Length == 3 &&
            float.TryParse(parts[0], out float x) &&
            float.TryParse(parts[1], out float y) &&
            float.TryParse(parts[2], out float z))
        {
            return new Vector3(x, y, z);
        }

        Debug.LogError($"Invalid Vector3 format: {str}");
        return Vector3.zero;
    }
}
