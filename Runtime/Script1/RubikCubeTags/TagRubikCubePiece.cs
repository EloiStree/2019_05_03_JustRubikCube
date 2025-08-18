using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

//[ExecuteInEditMode]
public class TagRubikCubePiece : TagRubikAbstract
{
    
    [SerializeField] RubikPieceByPosition3D m_position;
    public RubikPieceByPosition3D GetCubePosition3D() { return m_position; }
    public RubikCubePivotable[] GetPivots() { return RubikCube.GetPieceInfoOf(m_position).GetPivots(); }

    public TagRubikCube m_linkedCube;
    public TagRubikCubeFace[] m_linkedFaces;

    public void OnValidate()
    {
        RefreshLink();
    }

    private void RefreshLink()
    {

        
        if (m_root==null)
            m_root = this.transform;
        if (m_linkedCube == null )
            m_linkedCube = this.GetComponentInParent<TagRubikCube>();
        if(m_linkedFaces==null || m_linkedFaces.Length==0)
            m_linkedFaces = GetComponentsInChildren<TagRubikCubeFace>();
        AutoJoinByName();
    }

    private void AutoJoinByName()
    {
        RubikPieceByPosition3D position;
        if (RubikCube.GetPositionInfoInString(gameObject.name, out position)) {
            m_position = position;

        }

        RubikPiecePositionByPivot pivot;
        if (RubikCube.GetPositionInfoInString(gameObject.name, out pivot))
        {
            m_position = RubikCube.GetPieceInfoOf(pivot).GetPosition3D();
        }
    }

    

    public new void Reset()
    {
        base.Reset();
        RefreshLink();
        AutoJoinByName();
    }
    
}


public class DebugUtility {
    public static void DrawCross(Transform position, float range, Color color, float time=0)
    {
        Debug.DrawLine(position.position + position.up * -range, position.position + position.up * range, color, time==0?Time.deltaTime:time);
        Debug.DrawLine(position.position + position.right * -range, position.position + position.right * range, color, time == 0 ? Time.deltaTime : time);
        Debug.DrawLine(position.position + position.forward * -range, position.position + position.forward * range, color, time == 0 ? Time.deltaTime : time);
    }
}