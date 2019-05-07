using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TryToRotateRightFace : MonoBehaviour {

    public RubikCube m_rubik;
    public RubikFocusPoint m_focus;
    public RubikCubeFaceInfo m_faceInfo;
    public Transform m_from;
    

    public void Update()
    {

        m_rubik = m_focus.m_cubeTargeted;
        if (m_focus)
            m_faceInfo = m_focus.m_faceFocused;
        if (m_focus == null || m_focus.m_faceFocused == null)
            return;
        if (m_from == null)
            m_from = Camera.main.transform;
        if (m_rubik) {

            if (Input.GetKeyDown(KeyCode.UpArrow))
                m_rubik.RotateFaceFrom(ArrowDirection.Up, m_focus.m_faceFocused, m_from);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                m_rubik.RotateFaceFrom(ArrowDirection.Down, m_focus.m_faceFocused, m_from);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                m_rubik.RotateFaceFrom(ArrowDirection.Left, m_focus.m_faceFocused, m_from);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                m_rubik.RotateFaceFrom(ArrowDirection.Right, m_focus.m_faceFocused, m_from);

        }
    }
   
    
    
}
