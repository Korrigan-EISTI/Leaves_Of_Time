using UnityEngine;
using System.Collections;

public class TimeItem : Items
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isRespawning = false;
    public float respawnTime = 5f;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public override void ExecuteAction(GameObject player)
    {
        EnvironmentTimeControl timeControl = FindObjectOfType<EnvironmentTimeControl>();

        if (timeControl != null)
        {
            timeControl.AddTimeItem(); // Incrémente le compteur et active canBreakTime
            Debug.Log("Item de temps collecté ! Total : " + timeControl.timeItemsCount);
        }

        if (!isRespawning)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        isRespawning = true;
        gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnTime);

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        gameObject.SetActive(true);
        isRespawning = false;
    }
}