using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeItem : Items
{
    public override void ExecuteAction(GameObject player)
    {
        CustomPlayerMovement customPlayer = player.GetComponent<CustomPlayerMovement>();
        if (customPlayer != null)
        {
            customPlayer.CanBreakTime = true;
        }
    }
}
