using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class TimeEffects : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    private void Start()
    {
        // Récupère le Color Grading de l'effet Post-Process
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    public IEnumerator ApplyDesaturation(float duration, bool reverse)
    {
        float elapsedTime = 0f;
        float startValue = reverse ? -100f : 100f;
        float targetValue = reverse ? 100f : -100f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            colorGrading.saturation.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        colorGrading.saturation.value = targetValue;
    }
}
