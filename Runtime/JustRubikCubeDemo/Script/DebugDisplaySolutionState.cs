using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplaySolutionState : MonoBehaviour {


    public RubikCubeEngineMono m_rubikCube;
    public Text m_textDisplay;
    
	
	void Update () {
        bool resolved = m_rubikCube.IsCubeResolved();
        m_textDisplay.text = "" + resolved;


    }
}
