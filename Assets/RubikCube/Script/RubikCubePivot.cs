using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubePivot : MonoBehaviour {

    public Transform m_root;
    public RubikCubePivotable m_face;
    public bool m_isAtCenter;

    [Header("Debug")]
    public Vector3 m_offsetLine;
    private void Reset()
    {
        m_root = this.transform;
    }

    public void Update()
    {
        Debug.DrawLine(m_root.position+ m_offsetLine, m_root.position+ m_offsetLine + m_root.up * 3, GetColor(m_face));
    }
    public void OnValidate()
    {

        Debug.DrawLine(m_root.position+ m_offsetLine, m_root.position+ m_offsetLine + m_root.up * 3, GetColor(m_face),30);
    }

    private Color GetColor(RubikCubePivotable m_face)
    {
        switch (m_face)
        {
            case RubikCubePivotable.Left:return new Color(1,1,1);
            case RubikCubePivotable.Right: return new Color(1, 1, 0);
            case RubikCubePivotable.Up: return new Color(1, 0, 0);
            case RubikCubePivotable.Down: return new Color(255f/255f, 165f/255f, 0);
            case RubikCubePivotable.Face: return new Color(0, 1, 0);
            case RubikCubePivotable.Back: return new Color(0, 0, 1);
            case RubikCubePivotable.Middle: return Color.cyan;
            case RubikCubePivotable.Equator: return Color.magenta;
            case RubikCubePivotable.Standing: return Color.grey;
            default: return new Color(1, 1, 1);
        }
    }
}
