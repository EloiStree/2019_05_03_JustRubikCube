using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour {
    public string m_urlToOpen;

    public void Open()
    {
        Open(m_urlToOpen);
    }
    public void Open(string url)
    {
        Application.OpenURL(url);

    }
}
