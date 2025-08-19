using UnityEngine;
using UnityEngine.Events;

public class RubikMono_SetTextFromNumber : MonoBehaviour
{
    public string m_format = "{0:00}";

    public UnityEvent<string> m_onChange;
 

    public void SetTextWith(int number)
    {
        m_onChange.Invoke(string.Format(m_format, number));
    }
}
