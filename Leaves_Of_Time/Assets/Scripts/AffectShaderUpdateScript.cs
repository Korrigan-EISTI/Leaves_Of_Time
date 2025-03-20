using UnityEngine;
using System.Collections.Generic;
using Chronos;

public class AssignShaderScript : MonoBehaviour
{
    public Timekeeper timekeeper; // Référence au Timekeeper Chronos

    void Start()
    {
        // Recherche tous les enfants du parent
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child == this.transform) continue; // Ignore le parent lui-même

            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            SkinnedMeshRenderer skinnedRenderer = child.GetComponent<SkinnedMeshRenderer>();

            List<Material> materials = new List<Material>();

            if (meshRenderer != null)
                materials.AddRange(meshRenderer.materials);

            if (skinnedRenderer != null)
                materials.AddRange(skinnedRenderer.materials);

            // Vérifie si un matériau avec "Wind" ou "Water" est présent
            Material targetMaterial = null;
            foreach (Material mat in materials)
            {
                if (mat != null && (mat.name.Contains("Wind") || mat.name.Contains("Water")))
                {
                    targetMaterial = mat; // Prend le premier matériau trouvé correspondant
                    break;
                }
            }

            // Si un matériau "Wind" ou "Water" est détecté, ajoute et configure `UpdateShaderProperty`
            if (targetMaterial != null)
            {
                UpdateShaderProperty updateScript = child.GetComponent<UpdateShaderProperty>();

                if (updateScript == null)
                {
                    updateScript = child.gameObject.AddComponent<UpdateShaderProperty>();
                    Debug.Log($"Script UpdateShaderProperty ajouté à {child.name}");
                }

                // Assigne les références nécessaires
                updateScript.material = targetMaterial;
                updateScript.timekeeper = timekeeper;
            }
        }
    }
}
