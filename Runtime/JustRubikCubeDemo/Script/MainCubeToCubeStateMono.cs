using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RubikCubeSafeStateMono))]
public class MainCubeToCubeStateMono : MonoBehaviour, IRubikCubeRequired
{
    public RubikCubeInstance m_rubikCube;
    public RubikCubeSafeStateMono m_stateMono;

    private void OnValidate()
    {
        m_stateMono = GetComponent<RubikCubeSafeStateMono>();
    }

    private void Start()
    {
        if (m_rubikCube)
        {
            RefreshUI(null);
        }
    }

    public void OnNewRubikCubeFocused(RubikCubeInstance previousCube, RubikCubeInstance newCube)
    {
        if (m_rubikCube) {

            m_rubikCube.GetRubikCubeUnityRepresentation().m_onRotated.RemoveListener(RefreshUI);
            m_rubikCube = null;
        }
        if (m_rubikCube == null)
        {
            m_rubikCube = newCube;
            m_rubikCube.GetRubikCubeUnityRepresentation().m_onRotated.AddListener(RefreshUI);
        }

    }

    private void RefreshUI(RubikCube.LocalRotationRequest arg0)
    {
        if(m_rubikCube && m_stateMono)
            m_stateMono.RefreshCubeState( m_rubikCube.GetCubeStateReference());
    }
}
