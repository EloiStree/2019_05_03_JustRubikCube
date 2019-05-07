using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text m_displayTimer;
    public float m_time;
    public bool m_playing;

    public void StartCounting()
    {
        m_playing = true;
        ResetCounting();
    }
    public void StopCounting()
    {
        m_playing = false;

    }
    public void ResetCounting()
    {
        m_time = 0;

    }
    void Update () {
        float seconds = m_time%60;
        float minutes =(int)( m_time/60f);
        m_displayTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if(m_playing)

            m_time += Time.deltaTime;
    }
}
