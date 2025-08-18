using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITime {
    float GetTimePast();
    void ResetToZero();
    void StartTimer();
    void StopTimer();
}
public class TimePastDefault : TimePast
{
    [SerializeField] float m_time;
    [SerializeField] bool m_isRunning;
    private void Update()
    {
        if(m_isRunning)
            m_time += Time.deltaTime;
    }
    public override float GetTimePast()
    {
        return m_time;
    }

    public override void ResetToZero()
    {
        m_time = 0;
    }

    public override void StartTimer()
    {
        m_isRunning = true;
    }

    public override void StopTimer()
    {
        m_isRunning = true;
    }
}
public abstract class TimePast : MonoBehaviour, ITime
{
    public abstract float GetTimePast();
    public abstract void ResetToZero();

    public abstract void StartTimer();
    public abstract void StopTimer();
}
