using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseDownEvent : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler {

    public UnityEvent m_onMouseDown;
    public UnityEvent m_onMouseLeftDown;
    public UnityEvent m_onMouseRightDown;
    public UnityEvent m_onMouseUp;
    public bool m_overObject;
    public bool m_clicked;
    void OnMouseOver()
    {
        m_overObject = true;

    }
    private void OnMouseExit()
    {
        m_overObject = false;

    }

    private void Update()
    {

        
        if (!m_overObject)
            return;

        m_onMouseDown.Invoke();
        if (Input.GetMouseButtonDown(0) || Input.touchCount==1)
            m_onMouseLeftDown.Invoke();
        if (Input.GetMouseButtonDown(1) || Input.touchCount >1)
            m_onMouseRightDown.Invoke();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_onMouseDown.Invoke();
        if (eventData.button==PointerEventData.InputButton.Left)
            m_onMouseLeftDown.Invoke();
        if (eventData.button == PointerEventData.InputButton.Right)
            m_onMouseRightDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_onMouseUp.Invoke();
    }
}
