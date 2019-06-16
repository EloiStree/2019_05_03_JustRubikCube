using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TryToRotateRightFace : MonoBehaviour {

    public RubikCubeInstance m_rubik;
    public RubikCubePointer m_focus;
    public TagRubikCubeFace m_faceInfo;
    public Transform m_from;
    

    public void Update()
    {

        m_rubik = m_focus.GetCubeManager() ;
        if (m_focus)
            m_faceInfo = m_focus.GetSelectedFace();
        if (m_focus == null || m_faceInfo == null)
            return;
        if (m_from == null)
            m_from = Camera.main.transform;

        if (m_rubik == null)
            m_rubik = RubikCubeInstance.m_mainCube;

      
    }

    public void ApplyRotation(ArrowDirection direction) {
        if (m_rubik)
        {
                m_rubik.Rotate(direction, m_faceInfo.m_belongToFace, m_faceInfo.m_faceDirection, m_from);
        }
    }
}