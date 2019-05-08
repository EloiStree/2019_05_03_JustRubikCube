using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplaySolutionState : MonoBehaviour {


    public RubikCube m_rubikCube;
    public Text m_textDisplay;

    void Start () {
		
	}
	
	void Update () {
        float pct = 0;
        bool resolved = m_rubikCube.IsCubeResolved(out pct);
        m_textDisplay.text = string.Format("{0:0}%", pct*100f);


    }
}
