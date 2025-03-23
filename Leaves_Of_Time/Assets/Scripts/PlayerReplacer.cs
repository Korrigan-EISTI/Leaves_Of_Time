using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public Vector3 InitialPosition
    {
        private get { return initialPosition; }
        set { initialPosition = value; }
    }

    public Quaternion InitialRotation
    {
        private get { return initialRotation; }
        set { initialRotation = value; }
    }

    private void Start()
    {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
    }
    public void RespawnPlayer()
    {
        transform.position = InitialPosition;
        transform.rotation = InitialRotation;
    }
}
