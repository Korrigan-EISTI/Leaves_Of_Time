using System.Collections;
using Chronos;
using UnityEngine;

public class EnvironmentTimeControl : MonoBehaviour
{
    private bool isPaused = false;
    public bool canBreakTime = false;
    public float pauseDuration = 3f;

    private void Update()
    {
        Clock clock = Timekeeper.instance.Clock("GroupEnvironment");

        if (Input.GetKeyDown(KeyCode.C) && canBreakTime && !isPaused)
        {
            StartCoroutine(PauseTime(clock));
        }
    }

    private IEnumerator PauseTime(Clock clock)
    {
        isPaused = true;
        clock.localTimeScale = 0;

        yield return new WaitForSecondsRealtime(pauseDuration);

        clock.localTimeScale = 1;
        isPaused = false;
    }
}
