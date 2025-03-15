using System.Collections;
using Chronos;
using UnityEngine;

public class EnvironmentTimeControl : MonoBehaviour
{
    private bool isPaused = false;
    public bool canBreakTime = false;
    public float pauseDuration = 3f;
    public float mainDollyZoomEffectDuration = 2f;
    public Camera mainCamera;
    public float dollyZoomSpeed = 2f;
    public float zoomFOV = 30f;
    private float originalFOV;
    public TimeEffects timeEffects;
    public KeyCode pauseKey = KeyCode.C;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalFOV = mainCamera.fieldOfView;
    }

    private void Update()
    {
        Clock clock = Timekeeper.instance.Clock("GroupEnvironment");

        if (Input.GetKeyDown(pauseKey) && canBreakTime && !isPaused)
        {
            StartCoroutine(HandleTimePause(clock));
        }
    }

    private IEnumerator HandleTimePause(Clock clock)
    {
        isPaused = true;
        yield return StartCoroutine(DollyZoomEffect(mainCamera.fieldOfView, originalFOV * 2, mainDollyZoomEffectDuration));
        yield return StartCoroutine(DollyZoomEffect(mainCamera.fieldOfView, zoomFOV, mainDollyZoomEffectDuration / 10));

        clock.localTimeScale = 0;
        StartCoroutine(timeEffects.ApplyDesaturation(1f, false));
        yield return new WaitForSecondsRealtime(pauseDuration);

        StartCoroutine(timeEffects.ApplyDesaturation(1f, true));
        yield return StartCoroutine(DollyZoomEffect(mainCamera.fieldOfView, zoomFOV, mainDollyZoomEffectDuration));
        yield return StartCoroutine(DollyZoomEffect(mainCamera.fieldOfView, originalFOV, mainDollyZoomEffectDuration / 10));

        clock.localTimeScale = 1;
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
