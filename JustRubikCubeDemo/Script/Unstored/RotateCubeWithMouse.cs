using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateCubeWithMouse : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    public RotateFromCamera m_toRotate;

    public Vector3 m_mousePosition;
    public bool m_isFocused;
    public bool m_cursorOver;
    public void OnMouseEnter()
    {
        m_cursorOver = true;
    }
    public void OnMouseExit()
    {
        m_cursorOver = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void Update()
    {

        Vector3 currentMousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(2)){// && m_cursorOver) {

            m_isFocused = true;
            m_mousePosition = currentMousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
            m_isFocused = false;

        if (m_isFocused)
        {
            Vector3 direction = (currentMousePosition - m_mousePosition).normalized;
            //  m_mousePosition = currentMousePosition;

            if (direction.x != 0f && m_toRotate)
                m_toRotate.m_externalDirection.x = direction.x;// > 0f ? 1f : -1f;
            if (direction.y != 0f && m_toRotate)
                m_toRotate.m_externalDirection.y = direction.y; //> 0f ? 1f : -1f;
        }
    }

}
