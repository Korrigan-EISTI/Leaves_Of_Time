using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReplacer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collider = collision.collider.gameObject;

        if (collider != null)
        {
            PlayerRespawn respawn = collider.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.RespawnPlayer();
            }
        }
    }
}
