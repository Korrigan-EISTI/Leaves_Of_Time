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
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Renderer renderer = collision.gameObject.GetComponent<Renderer>();

        if (renderer != null && renderer.sharedMaterial != null)
        {
            string materialName = renderer.sharedMaterial.name.ToLower();

            if (materialName.Contains("water"))
            {
                RespawnPlayer();
            }
        }
    }

    private void RespawnPlayer()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
