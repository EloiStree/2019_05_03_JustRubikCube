using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RubikCubeToMapUI : MonoBehaviour {

    public RubikCube m_rubikCube;
    public FlatRubikCubeUI m_flatUI;

    void Awake () {
        m_rubikCube.m_onRotated.AddListener(RefreshUI);

    }

    private void RefreshUI(RubikCube.LocalRotationRequest arg0)
    {

        foreach (RubikCubeFace f in Enum.GetValues(typeof(RubikCubeFace)).Cast<RubikCubeFace>().ToList())
        {
            foreach (RubikCubeFaceDirection d in Enum.GetValues(typeof(RubikCubeFaceDirection)).Cast<RubikCubeFaceDirection>().ToList())
            {


                Color color = m_rubikCube.GetRgbColorFromLocal(f,d);
                RubikCubeFaceUI face = m_flatUI.GetFace(f,d);
                face.SetColor(color);
            }
        }
    }
    
}
