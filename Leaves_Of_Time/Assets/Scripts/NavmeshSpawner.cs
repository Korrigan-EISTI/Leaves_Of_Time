using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{
    [Header("Paramètres de Spawn")]
    [SerializeField] public GameObject prefab;                // Préfabriqué à instancier
    [SerializeField] public float spawnInterval = 5f;        // Temps entre chaque spawn
    [SerializeField] public List<Transform> waypoints;       // Liste des waypoints à suivre

    private void Start()
    {
        if (prefab == null || waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Paramètres manquants sur NavMeshSpawner");
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnAgent();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnAgent()
    {
        GameObject spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity);
        NavMeshAgent agent = spawnedObject.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Le prefab doit contenir un composant NavMeshAgent.");
            Destroy(spawnedObject);
            return;
        }

        AgentNavigator navigator = spawnedObject.AddComponent<AgentNavigator>();
        navigator.Initialize(waypoints);
    }
}

public class AgentNavigator : MonoBehaviour 
{
    private NavMeshAgent agent;
    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    public void Initialize(List<Transform> waypoints)
    {
        this.waypoints = waypoints;
        agent = GetComponent<NavMeshAgent>();

        if (agent == null || waypoints == null || waypoints.Count == 0)
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

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }
}
