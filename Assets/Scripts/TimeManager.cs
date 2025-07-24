using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float defaultFixedDeltaTime;
    [SerializeField]
    private float defaultSlowMotionFactor = .5f;

    private void Start()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SetSlow(float slowTimeFactor = -1)
    {
        if (slowTimeFactor == -1)
        {
            slowTimeFactor = defaultSlowMotionFactor;
        }
        Time.timeScale = slowTimeFactor;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }
}
