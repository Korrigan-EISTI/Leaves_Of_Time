using UnityEngine;
using Chronos;

public class ParticleTimeControl : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private float originalSimulationSpeed = 1f;
    private float oldTimeScale = 0f;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            originalSimulationSpeed = particleSystem.main.simulationSpeed;
        }
    }

    void Update()
    {
        if (Timekeeper.instance != null)
        {
            Clock clock = Timekeeper.instance.Clock("GroupEnvironment");
            if (oldTimeScale != clock.localTimeScale)
            {
                AdjustParticleSpeed(clock.localTimeScale);
                oldTimeScale = clock.localTimeScale;
            }
        }
    }

    private void AdjustParticleSpeed(float timeScale)
    {
        if (particleSystem != null)
        {
            var mainModule = particleSystem.main;
            mainModule.simulationSpeed = originalSimulationSpeed * timeScale;
        }
    }
}
