using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPositionning : MonoBehaviour {

    public Transform m_toAffect;
    public Vector3 m_offset;

    public Vector3 m_worldPosition;

    public Ray m_ray;
    public Vector3 m_mousePosition;
    public Camera m_usedCamera;
    public float m_depth=0.5f;

    private void Reset()
    {
        m_toAffect = transform;
    }
    public void SetMousePositionInPixel(Vector3 mousePosition)
    {
        m_mousePosition = mousePosition;
    }
    public void SetMousePositionInPixel(Vector2 mousePosition)
    {
        m_mousePosition= mousePosition;
    }

    void Update () {
        if(m_usedCamera ==null)
           m_usedCamera = Camera.main;
        m_mousePosition.z = m_depth;
        m_worldPosition = m_usedCamera.ScreenToWorldPoint(m_mousePosition);
        m_ray =m_usedCamera.ScreenPointToRay(m_mousePosition);
        m_toAffect.position = m_worldPosition;
        m_toAffect.forward = m_ray.direction;
    }
}
