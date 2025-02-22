using UnityEngine;

public class TimeItem : Items
{
    public override void ExecuteAction(GameObject player)
    {
        EnvironmentTimeControl timeControl = FindObjectOfType<EnvironmentTimeControl>();

        if (timeControl != null)
        {
            timeControl.canBreakTime = true;
            Debug.Log("✅ Le joueur peut maintenant arrêter le temps !");
        }
    }
}
