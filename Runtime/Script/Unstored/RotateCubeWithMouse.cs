using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateCubeWithMouse : MonoBehaviour
{


    public RotateFromCamera m_toRotate;

    public Vector3 m_mousePositionAtStart;
    public bool m_isFocused=true;
    public bool m_cursorOver;
  

    public Vector2 m_givenMousePixelPosition;
    public bool m_givenIsMousePressingRight;
    public bool m_previousMousePressingState;
    public float m_pixelForFullRotation200;


    public void SetAsCursorOver(bool isCursorOver) {

        m_cursorOver = isCursorOver;
    }

    public void Update()
    {

        Vector3 currentMousePosition = m_givenMousePixelPosition;
        bool detectChange = m_previousMousePressingState != m_givenIsMousePressingRight;
        m_previousMousePressingState = m_givenIsMousePressingRight;
        if (detectChange) {
            m_isFocused = m_givenIsMousePressingRight;
            if (m_isFocused)
            {
                m_mousePositionAtStart = currentMousePosition;
            }
        }

        if (m_isFocused)
        {
            Vector3 direction = (currentMousePosition - m_mousePositionAtStart);
         
            //  m_mousePosition = currentMousePosition;

            if (direction.x != 0f && m_toRotate)
                m_toRotate.m_externalDirection.x = direction.x/ m_pixelForFullRotation200;// > 0f ? 1f : -1f;
            if (direction.y != 0f && m_toRotate)
                m_toRotate.m_externalDirection.y = direction.y / m_pixelForFullRotation200; //> 0f ? 1f : -1f;
        }
    }

}
