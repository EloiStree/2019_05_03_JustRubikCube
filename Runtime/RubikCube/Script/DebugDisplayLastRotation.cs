using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplayLastRotation : MonoBehaviour {

    public RubikCube m_rubikCube;
    public Text m_textDisplay;

	void Awake ()
    {
        m_rubikCube.m_onStartRotating.AddListener(StarRotation);
        m_rubikCube.m_onRotated.AddListener(EndRotation);

    }

    private void EndRotation(RubikCube.LocalRotationRequest rot)
    {
        m_textDisplay.text = ""+RubikCube.ConvertFaceRotationToString(rot.m_faceToRotate, rot.m_clockWise);
    }

    private void StarRotation(RubikCube.LocalRotationRequest rot)
    {
        m_textDisplay.text = ">"+RubikCube.ConvertFaceRotationToString(rot.m_faceToRotate, rot.m_clockWise);
    }
    
}
