using Chronos;
using UnityEngine;

public class UpdateShaderProperty : MonoBehaviour
{
    public Material material;  
    public Timekeeper timekeeper; 

    void Update()
    {
        if (material != null && Timekeeper.instance != null)
        {
            float newTimeScale = Timekeeper.instance.Clock("GroupEnvironment").localTimeScale;
            material.SetFloat("_TimeScale", newTimeScale);
        }
    }
}
