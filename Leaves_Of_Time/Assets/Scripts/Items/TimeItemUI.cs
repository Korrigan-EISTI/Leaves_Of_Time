using UnityEngine;
using TMPro;
using System.Collections;

public class TimeItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeItemText;
    [SerializeField] private EnvironmentTimeControl timeControl;
    private int lastTimeItemCount = 0;
    private Vector3 initialScale; // Pour stocker le scale initial

    void Start()
    {
        Debug.Log("TimeItemUI démarré sur " + gameObject.name);

        if (timeControl == null)
        {
            timeControl = FindObjectOfType<EnvironmentTimeControl>();
            if (timeControl == null)
            {
                Debug.LogError("EnvironmentTimeControl non trouvé dans TimeItemUI !", this);
            }
            else
            {
                Debug.Log("EnvironmentTimeControl trouvé avec succès ! timeItemsCount initial : " + timeControl.timeItemsCount + ", trouvé sur : " + timeControl.gameObject.name);
            }
        }

        if (timeItemText == null)
        {
            Debug.LogError("TimeItemText non assigné dans TimeItemUI !", this);
        }
        else
        {
            Debug.Log("TimeItemText assigné avec succès !");
            initialScale = timeItemText.transform.localScale; // Stocke le scale initial
        }

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (timeControl != null)
            {
                timeControl.timeItemsCount++;
                Debug.Log("Test manuel : timeItemsCount forcé à " + timeControl.timeItemsCount);
            }
        }

        if (timeControl != null)
        {
            if (timeControl.timeItemsCount != lastTimeItemCount)
            {
                Debug.Log($"Changement détecté : {lastTimeItemCount} -> {timeControl.timeItemsCount}");
                lastTimeItemCount = timeControl.timeItemsCount;
                StartCoroutine(AnimateText());
            }
        }
        else
        {
            Debug.LogWarning("timeControl est null dans Update !");
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (timeItemText != null && timeControl != null)
        {
            timeItemText.text = $"{timeControl.timeItemsCount}";
            Debug.Log($"UI mise à jour : {timeControl.timeItemsCount} items de temps");
        }
        else
        {
            Debug.LogWarning("Problème avec timeItemText ou timeControl dans UpdateUI : " +
                            $"timeItemText = {(timeItemText != null ? "assigné" : "null")}, " +
                            $"timeControl = {(timeControl != null ? "assigné" : "null")}");
        }
    }

    IEnumerator AnimateText()
    {
        Debug.Log("Animation du texte déclenchée !");
        float scaleUpMultiplier = 1.2f; // Grossit de 20%
        float duration = 0.2f;

        // Calcule le scale maximum en fonction du scale initial
        Vector3 scaleUp = initialScale * scaleUpMultiplier;

        // Grossissement
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            timeItemText.transform.localScale = Vector3.Lerp(initialScale, scaleUp, t);
            yield return null;
        }

        // Retour au scale initial
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            timeItemText.transform.localScale = Vector3.Lerp(scaleUp, initialScale, t);
            yield return null;
        }

        // S'assure que le scale final est exactement le scale initial
        timeItemText.transform.localScale = initialScale;
    }
}