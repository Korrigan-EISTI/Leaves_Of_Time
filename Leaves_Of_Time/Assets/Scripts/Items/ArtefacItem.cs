using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefacItem : Items
{
    public override void ExecuteAction(GameObject player)
    {
        CustomPlayerMovement customPlayer = player.GetComponent<CustomPlayerMovement>();
        if (customPlayer != null)
        {
            //TODO Add some score 
            return;
        }
    }
}
