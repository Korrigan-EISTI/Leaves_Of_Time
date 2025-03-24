using System.Collections;
using Chronos;
using UnityEngine;
using TMPro;

public class EnvironmentTimeControl : MonoBehaviour
{
    private bool isPaused = false;
    public bool canBreakTime = false;
    public int timeItemsCount = 0; // Compteur d'items de temps
    private const int MAX_TIME_ITEMS = 3;
    public float pauseDuration = 3f;
    public float mainDollyZoomEffectDuration = 2f;
    public Camera mainCamera;
    public float dollyZoomSpeed = 2f;
    public float zoomFOV = 30f;
    private float originalFOV;
    public TimeEffects timeEffects;
    public KeyCode pauseKey = KeyCode.C;
    public Terrain terrain;
    public TextMeshProUGUI timeStopPrompt;
    private bool hasShownPrompt = false;
    public MenuManager menuManager;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalFOV = mainCamera.fieldOfView;
        Debug.Log($"EnvironmentTimeControl initialisé sur {gameObject.name}, timeItemsCount initial : {timeItemsCount}");

        if (timeStopPrompt != null)
        {
            timeStopPrompt.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        Clock clock = Timekeeper.instance.Clock("GroupEnvironment");

        // Vérifie si le menu est fermé avant de permettre la pause temporelle
        if (Input.GetKeyDown(pauseKey) && canBreakTime && !isPaused && timeItemsCount > 0 && (menuManager == null || !menuManager.IsExitMenuOpen()))
        {
            StartCoroutine(HandleTimePause(clock));
            timeItemsCount--;
            Debug.Log($"Touche C pressée ! timeItemsCount décrémenté à {timeItemsCount}");

            if (timeStopPrompt != null)
            {
                timeStopPrompt.gameObject.SetActive(false);
            }

            if (timeItemsCount <= 0)
            {
                canBreakTime = false;
                Debug.Log("Plus d'items de temps, canBreakTime désactivé !");
            }
        }
    }

    public bool AddTimeItem()
    {
        // Vérifie si on peut ajouter un item (limite non atteinte)
        if (timeItemsCount >= MAX_TIME_ITEMS)
        {
            Debug.Log("Limite d'items de temps atteinte (3 max) ! Impossible d'en collecter plus.");
            return false; 
        }

        timeItemsCount++;
        canBreakTime = true;
        Debug.Log($"AddTimeItem appelé ! timeItemsCount = {timeItemsCount}, canBreakTime = {canBreakTime}");

        if (timeItemsCount == 1 && !hasShownPrompt && timeStopPrompt != null)
        {
            timeStopPrompt.gameObject.SetActive(true);
            hasShownPrompt = true;
        }

        return true; 
    }

    private IEnumerator HandleTimePause(Clock clock)
    {
        isPaused = true;
        yield return StartCoroutine(DollyZoomEffect(mainCamera.fieldOfView, originalFOV + 20f, mainDollyZoomEffectDuration));
        yield return StartCoroutine(DollyZoomEffect(mainCamera.fieldOfView, zoomFOV, mainDollyZoomEffectDuration / 10));

        float startSpeed = terrain.terrainData.wavingGrassStrength;
        clock.localTimeScale = 0;
        terrain.terrainData.wavingGrassStrength = 0f;
        StartCoroutine(timeEffects.ApplyDesaturation(1f, false));
        yield return new WaitForSecondsRealtime(pauseDuration);

        StartCoroutine(timeEffects.ApplyDesaturation(1f, true));
        yield return StartCoroutine(DollyZoomEffect(mainCamera.fieldOfView, zoomFOV, mainDollyZoomEffectDuration));
        yield return StartCoroutine(DollyZoomEffect(mainCamera.fieldOfView, originalFOV + 20f, mainDollyZoomEffectDuration / 10));

        mainCamera.fieldOfView = originalFOV;
        clock.localTimeScale = 1;
        terrain.terrainData.wavingGrassStrength = startSpeed;
        isPaused = false;
    }

    private IEnumerator DollyZoomEffect(float startFOV, float targetFOV, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / duration);
            yield return null;
        }
        mainCamera.fieldOfView = targetFOV;
    }
}