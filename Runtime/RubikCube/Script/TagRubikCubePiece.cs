using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

//[ExecuteInEditMode]
public class TagRubikCubePiece : MonoBehaviour {

    public Transform m_root;
    [SerializeField] RubikPiecePosition m_position;
    public RubikPiecePosition GetCubePosition() { return m_position; }
    public RubikCubePivotable[] GetPivots() { return RubikCube.GetPivotsFrom(m_position); }

    public TagRubikCube m_linkedCube;
    public TagRubikCubeFace[] m_linkedFaces;

    [Tooltip("Will try to set guest info from name keywords")]
    public bool m_autoJointbyName=true;

    public void OnValidate()
    {
        RefreshLink();
    }

    private void RefreshLink()
    {


        if (m_autoJointbyName)
            AutoJoinByName();
        if (m_root==null)
            m_root = this.transform;
        if (m_linkedCube == null )
            m_linkedCube = this.GetComponentInParent<TagRubikCube>();
        if(m_linkedFaces==null || m_linkedFaces.Length==0)
            m_linkedFaces = GetComponentsInChildren<TagRubikCubeFace>();
    }

    private void AutoJoinByName()
    {
        RubikPiecePosition position;
        if (RubikCube.GetPositionInfoInString(gameObject.name, out position)) {
            m_position = position;

        }

    }

    public void Reset()
    {
        m_root = this.transform;
        RefreshLink();
    }

    public Vector3 GetPosition() { return m_root.position; }
}


public class DebugUtility {
    public static void DrawCross(Transform position, float range, Color color, float time=0)
    {
        Debug.DrawLine(position.position + position.up * -range, position.position + position.up * range, color, time==0?Time.deltaTime:time);
        Debug.DrawLine(position.position + position.right * -range, position.position + position.right * range, color, time == 0 ? Time.deltaTime : time);
        Debug.DrawLine(position.position + position.forward * -range, position.position + position.forward * range, color, time == 0 ? Time.deltaTime : time);
    }
}