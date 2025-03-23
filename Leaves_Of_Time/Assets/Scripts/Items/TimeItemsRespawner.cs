using System.Collections;
using UnityEngine;

public class TimeItemsRespawner : MonoBehaviour
{
    public GameObject timeItemPrefab; // 👉 assigné via l'inspecteur

    private static TimeItemsRespawner instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public static void RequestRespawn(Vector3 position, Quaternion rotation, float delay)
    {
        if (instance != null && instance.timeItemPrefab != null)
        {
            instance.StartCoroutine(instance.Respawn(position, rotation, delay));
        }
        else
        {
            Debug.LogWarning("TimeItemsRespawner or prefab is missing!");
        }
    }

    private IEnumerator Respawn(Vector3 position, Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject newItem = Instantiate(timeItemPrefab, position, rotation);
        newItem.SetActive(true);
    }
}
