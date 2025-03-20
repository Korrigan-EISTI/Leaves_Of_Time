using UnityEngine;
using System.Collections.Generic;

public class PrefabReplacer : MonoBehaviour
{
    // Liste des noms partiels des prefabs à remplacer
    public string[] oldPrefabNames = { "Tree7A", "Tree7B", "Tree8A", "Tree8B" };

    // Liste des nouveaux prefabs correspondants
    public GameObject[] newPrefabs;

    void Start()
    {
        if (oldPrefabNames.Length != newPrefabs.Length)
        {
            Debug.LogError("Le nombre de noms de prefabs à remplacer doit correspondre au nombre de nouveaux prefabs !");
            return;
        }

        ReplacePrefabs();

        // Supprime le script après exécution
        Destroy(this);
    }

    void ReplacePrefabs()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            for (int i = 0; i < oldPrefabNames.Length; i++)
            {
                if (obj.name.Contains(oldPrefabNames[i])) // Vérifie si le nom contient la chaîne
                {
                    Vector3 position = obj.transform.position;
                    Quaternion rotation = obj.transform.rotation;
                    Vector3 scale = obj.transform.localScale;
                    Transform parent = obj.transform.parent; // Garde la hiérarchie

                    GameObject newObj = Instantiate(newPrefabs[i], position, rotation);
                    newObj.transform.localScale = scale;
                    newObj.transform.parent = parent;

                    DestroyImmediate(obj); // Supprime immédiatement l'ancien objet

                    break; // Évite de tester d'autres noms une fois remplacé
                }
            }
        }
    }
}
