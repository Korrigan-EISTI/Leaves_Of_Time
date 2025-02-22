using Chronos;
using UnityEngine;

public class UpdateShaderProperty : MonoBehaviour
{
    public Material material;  // Assure-toi d'assigner le matériau qui utilise le shader graph.
    public Timekeeper timekeeper; // Component that holds the localTimeScale

    void Update()
    {
        if (material != null && Timekeeper.instance != null)
        {
            float newTimeScale = Timekeeper.instance.Clock("GroupEnvironment").localTimeScale;
            material.SetFloat("_TimeScale", newTimeScale);
        }
    }
}
