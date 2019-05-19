using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagRubikCubeFace : TagRubikAbstract
{
    
    public TagRubikCube m_linkedCube;
    public TagRubikCubePiece m_linkedPiece;
    public RubikCubeFace m_belongToFace;
    public RubikCubeFaceDirection m_faceDirection;

    public void OnValidate()
    {
        RefreshLink();
    }

    private void RefreshLink()
    {
        if(m_linkedCube==null)
        m_linkedCube = this.GetComponentInParent<TagRubikCube>();
        if (m_linkedPiece == null)
            m_linkedPiece = this.GetComponentInParent<TagRubikCubePiece>();
    }

    private void AutoJoinByName()
    {


    RubikCubeFace face;
    RubikCubeFaceDirection direction;
        
    if(RubikCube.GetFaceInfoInString(gameObject.name, out face))
        m_belongToFace = face;
    if( RubikCube.GetDirectionInfoInString(gameObject.name, out direction))
        m_faceDirection = direction;

    }

    private new void Reset()
    {
        base.Reset();
        m_root = this.transform;
        RefreshLink();

    }

    internal RubikCubePivotable [] GetLinkedFaces()
    {
        return m_linkedPiece.GetPivots() ;
    }

    public RubikCubeColor GetColorEnum() {
        return RubikCube.GetColor(m_belongToFace);
    }

    public RubikCubePivotable GetPivot()
    {
        return RubikCube.ConvertFaceToPivot(m_belongToFace);
    }
    
}
