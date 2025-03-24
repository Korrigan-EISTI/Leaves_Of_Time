using UnityEngine;

public class TimeItem : Items
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

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
            timeControl.AddTimeItem();
            Debug.Log("Item de temps collecté ! Total : " + timeControl.timeItemsCount);
        }

        TimeItemsRespawner.RequestRespawn(initialPosition, initialRotation, respawnTime);
    }
}