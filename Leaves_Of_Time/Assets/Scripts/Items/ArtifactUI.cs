using UnityEngine;
using TMPro;
using System.Collections;

public class ArtifactUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI artifactText;
    [SerializeField] private MoveBehaviour player;
    private int lastArtifactCount = 0;
    private Vector3 initialScale; // Pour stocker le scale initial

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
        else
        {
            initialScale = artifactText.transform.localScale; // Stocke le scale initial
            Debug.Log("Scale initial de artifactText : " + initialScale);
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
            Debug.Log($"UI mise à jour : {player.ArtifactsCollected}/7");
        }
        else
        {
            Debug.LogWarning("Problème avec artifactText ou player dans ArtifactUI");
        }
    }

    IEnumerator AnimateText()
    {
        Debug.Log("Animation du texte déclenchée pour ArtifactUI !");
        float scaleUpMultiplier = 1.2f; // Grossit de 20% par rapport au scale initial
        float duration = 0.2f; // Durée de l'animation

        // Calcule le scale maximum en fonction du scale initial
        Vector3 scaleUp = initialScale * scaleUpMultiplier;

        // Grossir
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            artifactText.transform.localScale = Vector3.Lerp(initialScale, scaleUp, t);
            yield return null;
        }

        // Rétrécir (revenir au scale initial)
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            artifactText.transform.localScale = Vector3.Lerp(scaleUp, initialScale, t);
            yield return null;
        }

        // S'assure que le scale final est exactement le scale initial
        artifactText.transform.localScale = initialScale;
        Debug.Log("Animation terminée, scale final de artifactText : " + artifactText.transform.localScale);
    }
}