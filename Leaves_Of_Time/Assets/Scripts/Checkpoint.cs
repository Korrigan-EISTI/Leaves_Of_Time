using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool hasSavePosition = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasSavePosition)
        {
            PlayerRespawn playerRespawn = other.gameObject.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                hasSavePosition = true;
                playerRespawn.InitialPosition = other.transform.position;
                playerRespawn.InitialRotation = other.transform.rotation;
            }
        }
    }
}
