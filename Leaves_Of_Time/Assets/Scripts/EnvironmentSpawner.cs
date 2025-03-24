using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject environmentPrefab; 
    private GameObject environmentInstance; 

    void Start()
    {
        if (environmentPrefab == null)
        {
            Debug.LogError("Le prefab Environment n'est pas assigné !");
            return;
        }

        environmentInstance = Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
        environmentInstance.name = "Environment_Instance"; 

        Debug.Log("Environnement instancié !");
    }
}
