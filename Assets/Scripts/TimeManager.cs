using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float defaultFixedDeltaTime;
    private const float slowMotionFactor = .5f;

    private void Awake()
    {
        Debug.Log("Awake : " + defaultFixedDeltaTime);
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SetSlow(float slowTimeFactor = slowMotionFactor)
    {
        Time.timeScale = slowTimeFactor;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }
}
