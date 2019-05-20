using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddRotationHistory : MonoBehaviour {

    public RubikCubeEngineMono m_rubikCube;
    public string m_rotation;
    public List<RotationTypeShort> m_rotationType;
    public Text m_tHistoryDisplay;
    public int index =-1;


    public void Awake()
    {
        m_rubikCube.m_onRotated.AddListener(AddLocalRotation);
    }

    public void AddLocalRotation(RubikCubeEngineMono.LocalRotationRequest request) {
        AddRotation(request.m_faceToRotate, request.m_clockWise);
    }
    public void AddRotation(RubikCubePivotable rotation, bool clockwise)
    {

        AddRotation(RubikCube.ConvertRotationToAcronymShort(rotation, clockwise));

    }
    public void AddRotation(RotationTypeShort rotation)
    {
        m_rotation = RubikCube.ConvertAcronymShortToString(rotation)+" ";
        m_rotationType.Add(rotation);
        if(m_tHistoryDisplay)   
        m_tHistoryDisplay.text = m_rotation;

    }

   
}
