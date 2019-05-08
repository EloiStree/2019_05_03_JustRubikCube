using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddRotationHistory : MonoBehaviour {

    public RubikCube m_rubikCube;
    public string m_rotation;
    public List<RotationTypeShort> m_rotationType;
    public Text m_tHistoryDisplay;
    public int index =-1;
    

    public void AddLocalRotation(RubikCube.LocalRotationRequest request) {
        AddRotation(request.m_faceToRotate, request.m_clockWise);
    }
    public void AddRotation(RubikCubePivotable rotation, bool clockwise)
    {

        AddRotation(RubikCube.ConvertRotationToAcronymShort(rotation, clockwise));

    }
    public void AddRotation(RotationTypeShort rotation)
    {
        m_rotation = RubikCube.ConvertAcronymShortToString(rotation)+" "+ m_rotation;
        m_rotationType.Add(rotation);
        if(m_tHistoryDisplay)   
        m_tHistoryDisplay.text = m_rotation;

    }

    void ControlZut ()
    {
        if (m_rotationType.Count < 1)
            return;
        if(index<0)
            index = m_rotationType.Count-1;

        RotationTypeShort shortRot= RubikCube.GetInvertOf(m_rotationType[index]);
        m_rubikCube.LocalRotate(shortRot);
        index--;

    }
	
	void Update () {

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z))
            ControlZut();

	}
}
