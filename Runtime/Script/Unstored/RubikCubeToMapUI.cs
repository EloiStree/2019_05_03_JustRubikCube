using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RubikCubeToMapUI : MonoBehaviour ,IRubikCubeRequired
{

    public RubikCubeEngineMono m_rubikCube;
    public FlatRubikCubeUI m_flatUI;

    public void OnNewRubikCubeFocused(RubikCubeInstance oldcube, RubikCubeInstance cube)
    {
        if (m_rubikCube)
        {

            m_rubikCube.m_onEndRotating.RemoveListener(RefreshUI);
            m_rubikCube = null;
        }

        if (m_rubikCube == null) {
            m_rubikCube=  cube.GetRubikCubeUnityRepresentation();
            m_rubikCube.m_onEndRotating.AddListener(RefreshUI);
        }
    }
    
    private void RefreshUI(RubikCubeEngineMono.LocalRotationRequest arg0)
    {

        foreach (RubikCubeFace f in Enum.GetValues(typeof(RubikCubeFace)).Cast<RubikCubeFace>().ToList())
        {
            foreach (RubikCubeFaceDirection d in Enum.GetValues(typeof(RubikCubeFaceDirection)).Cast<RubikCubeFaceDirection>().ToList())
            {


                RubikCubeFace currentFace;
                RubikCubeFaceDirection currentDirection;
                m_rubikCube.m_cubeFaceDirectionState.GetRealPieceFaceInfoAt(f, d, out currentFace, out currentDirection);
                Color color = RubikCube.GetColor(RubikCube.GetDefaultColor(currentFace));
                RubikCubeFaceUI face = m_flatUI.GetFace(f,d);
                face.SetColor(color);
            }
        }
    }
    private void Awake()
    {
        
    }
}
