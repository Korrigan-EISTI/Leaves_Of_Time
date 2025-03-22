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

    private void OnCollisionEnter(Collision collision)
    {
        float closestDistance = float.MaxValue; // Initialize with a large value
        Material closestMaterial = null;

        foreach (ContactPoint contact in collision.contacts)
        {
            Renderer renderer = contact.otherCollider.gameObject.GetComponent<Renderer>();
            if (renderer != null && renderer.sharedMaterial != null)
            {
                float distance = (contact.point - transform.position).sqrMagnitude; // Use squared magnitude for efficiency
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMaterial = renderer.sharedMaterial;
                }
            }
        }

        if (closestMaterial != null && closestMaterial.name.ToLower().Contains("water"))
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        transform.position = InitialPosition;
        transform.rotation = InitialRotation;
    }
}
