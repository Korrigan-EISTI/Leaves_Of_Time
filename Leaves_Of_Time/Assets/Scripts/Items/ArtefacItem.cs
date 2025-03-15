using UnityEngine;

public class ArtefacItem : Items
{
    public override void ExecuteAction(GameObject player)
    {
        MoveBehaviour customPlayer = player.GetComponent<MoveBehaviour>();
        if (customPlayer != null)
        {
            customPlayer.CollectArtifact(); // Incrémente le compteur d'artefacts
        }
        else
        {
            Debug.LogError("MoveBehaviour non trouvé sur le joueur !");
        }
    }
}