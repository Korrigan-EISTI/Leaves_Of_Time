using UnityEngine;
using TMPro;
using System.Collections;

public class TimeItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeItemText;
    [SerializeField] private EnvironmentTimeControl timeControl;
    private int lastTimeItemCount = 0;

    void Start()
    {
        if (timeControl == null)
        {
            timeControl = FindObjectOfType<EnvironmentTimeControl>();
            if (timeControl == null)
            {
                Debug.LogError("EnvironmentTimeControl non trouvé dans TimeItemUI !", this);
            }
            else
            {
                Debug.Log("EnvironmentTimeControl trouvé avec succès ! timeItemsCount initial : " + timeControl.timeItemsCount);
            }
        }

        if (timeItemText == null)
        {
            Debug.LogError("TimeItemText non assigné dans TimeItemUI !", this);
        }
        else
        {
            Debug.Log("TimeItemText assigné avec succès !");
            timeItemText.transform.localScale = Vector3.one; // Assure que l'échelle est correcte
        }

        UpdateUI();
    }

    void Update()
    {
        // Test manuel avec la touche T
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
        float scaleUp = 1.2f;
        float scaleDown = 1f;
        float duration = 0.2f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, scaleUp, elapsed / duration);
            timeItemText.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(scaleUp, scaleDown, elapsed / duration);
            timeItemText.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
    }
}