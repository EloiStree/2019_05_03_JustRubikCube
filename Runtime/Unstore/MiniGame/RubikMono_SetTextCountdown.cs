using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RubikMono_SetTextCountdown : MonoBehaviour
{
    [Tooltip("Format string for minutes and seconds. Example: {0:00}:{1:00}")]
    public string m_format = "{0:00}:{1:00}";

    public UnityEvent<string> m_onChange;
    /// <summary>
    /// Sets the countdown display using time in seconds.
    /// </summary>
    /// <param name="timeInSeconds">Time value in seconds.</param>
    public void SetTimeInSeconds(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        m_onChange.Invoke(string.Format(m_format, minutes, seconds));
    }
}
