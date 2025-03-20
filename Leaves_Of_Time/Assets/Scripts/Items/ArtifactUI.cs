using UnityEngine;
using TMPro;
using System.Collections;

public class ArtifactUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI artifactText;
    [SerializeField] private MoveBehaviour player;
    private int lastArtifactCount = 0;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<MoveBehaviour>();
        }

        if (artifactText == null)
        {
            Debug.LogError("ArtifactText non assigné dans ArtifactUI !", this);
        }
        if (player == null)
        {
            Debug.LogError("MoveBehaviour non trouvé dans ArtifactUI !", this);
        }

        UpdateUI();
    }

    void Update()
    {
        if (player != null && player.ArtifactsCollected != lastArtifactCount)
        {
            lastArtifactCount = player.ArtifactsCollected;
            StartCoroutine(AnimateText()); // Lance l'animation quand un artefact est collecté
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (artifactText != null && player != null)
        {
            artifactText.text = $"{player.ArtifactsCollected}";
           /* Debug.Log($"UI mise à jour : {player.ArtifactsCollected}/7");*/
        }
        else
        {
            Debug.LogWarning("Problème avec artifactText ou player dans ArtifactUI");
        }
    }

    IEnumerator AnimateText()
    {
        float scaleUp = 1.2f; // Taille maximale pendant l'animation
        float scaleDown = 1f; // Taille de base
        float duration = 0.2f; // Durée de l'animation

        // Grossir
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, scaleUp, elapsed / duration);
            artifactText.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        // Rétrécir
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(scaleUp, scaleDown, elapsed / duration);
            artifactText.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
    }
}