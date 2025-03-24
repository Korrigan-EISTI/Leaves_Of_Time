using System.Collections;
using System.Collections.Generic;
using Chronos;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{
    [Header("Paramètres de Spawn")]
    [SerializeField] public GameObject prefab;
    [SerializeField] public float spawnInterval = 5f;
    [SerializeField] public List<Transform> waypoints;
    [SerializeField] public List<float> speeds;
    [SerializeField] public List<float> xRotations;

    private void Start()
    {
        if (prefab == null || waypoints == null || waypoints.Count == 0 || speeds == null || xRotations == null || speeds.Count != waypoints.Count - 1 || xRotations.Count != waypoints.Count - 1)
        {
            Debug.LogError("Paramètres manquants ou incorrects sur NavMeshSpawner");
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        // Récupère ou crée la clock Chronos associée au groupe "GlobalEnvironment"
        Clock clock = Timekeeper.instance.Clock("GroupEnvironment");

        while (true)
        {
            SpawnAgent();

            float startTime = clock.time;

            // Attend exactement spawnInterval secondes selon la clock Chronos
            while (clock.time < startTime + spawnInterval)
            {
                yield return null;
            }
        }
    }


    private void SpawnAgent()
    {
        GameObject spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity);
        spawnedObject.SetActive(true);
        NavMeshAgent agent = spawnedObject.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Le prefab doit contenir un composant NavMeshAgent.");
            Destroy(spawnedObject);
            return;
        }

        AgentNavigator navigator = spawnedObject.AddComponent<AgentNavigator>();
        navigator.Initialize(waypoints, speeds, xRotations);
    }
}

public class AgentNavigator : MonoBehaviour
{
    private NavMeshAgent agent;
    private List<Transform> waypoints;
    private List<float> speeds;
    private List<float> xRotations;
    private int currentWaypointIndex = 0;

    public void Initialize(List<Transform> waypoints, List<float> speeds, List<float> xRotations)
    {
        this.waypoints = waypoints;
        this.speeds = speeds;
        this.xRotations = xRotations;
        agent = GetComponent<NavMeshAgent>();

        if (agent == null || waypoints == null || waypoints.Count == 0 || speeds == null || xRotations == null)
        {
            Debug.LogError("Problème d'initialisation de l'agent.");
            Destroy(this);
            return;
        }

        MoveToNextWaypoint();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        if (currentWaypointIndex >= waypoints.Count - 1)
        {
            Destroy(gameObject);
        }
        else
        {
            agent.SetDestination(waypoints[currentWaypointIndex + 1].position);
            agent.speed = speeds[currentWaypointIndex];

            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(xRotations[currentWaypointIndex], currentRotation.y, currentRotation.z);

            currentWaypointIndex++;
        }
    }
}
