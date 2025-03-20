using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject environmentPrefab; // Le prefab à instancier

    private GameObject environmentInstance; // Instance créée

    void Start()
    {
        if (environmentPrefab == null)
        {
            Debug.LogError("Le prefab Environment n'est pas assigné !");
            return;
        }

        // Instancier l'environnement à la position (0,0,0)
        environmentInstance = Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity);
        environmentInstance.name = "Environment_Instance"; // Renommer pour éviter les duplicatas

        Debug.Log("Environnement instancié !");
    }
}
