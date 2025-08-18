using System.Collections.Generic;
using UnityEngine;
public class TagRubikCube : TagRubikAbstract
{
    public TagRubikCubePiece [] m_pieces;
    public TagRubikCubeFace [] m_faces;
    public Dictionary<string, TagRubikCubeFace> m_faceDirectAccess = new Dictionary<string, TagRubikCubeFace>();
    public Dictionary<string, TagRubikCubePiece> m_pieceDirectAccess = new Dictionary<string, TagRubikCubePiece>();

    public void Start()
    {
        for (int i = 0; i < m_pieces.Length; i++)
        {
            string tag = RubikCube.GetTagOf(
                    m_pieces[i].GetCubePosition3D());
            if (!m_pieceDirectAccess.ContainsKey(tag))
                m_pieceDirectAccess.Add(tag
                ,
                m_pieces[i]);
        }
        for (int i = 0; i < m_faces.Length; i++)
        {
            string tag = RubikCube.GetTagOf(
                    m_faces[i].m_belongToFace, m_faces[i].m_faceDirection);
            if(!m_faceDirectAccess.ContainsKey(tag))
            m_faceDirectAccess.Add(tag
                ,
                m_faces[i]);
        }

    }

    public TagRubikCubeFace GetFace(RubikCubeFace face, RubikCubeFaceDirection direction) {
        string key = RubikCube.GetTagOf(face, direction);
        if (m_faceDirectAccess.ContainsKey(key))
           return m_faceDirectAccess[key];
        return null;
    }
    public TagRubikCubePiece GetPiece(RubikPiecePositionByPivot position)
    {
        string key = RubikCube.GetTagOf(position);
        if (m_pieceDirectAccess.ContainsKey(key))
            return m_pieceDirectAccess[RubikCube.GetTagOf(position)];
        return null;
    }

    public TagRubikCubePiece GetPiece(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        RubikPiecePositionByPivot p = RubikCube.GetPiecePositionBasedOn(face, direction);
        return GetPiece(p);
    }
    
    public TagRubikCubePiece GetPiece(RubikCubePivotable[] pivots)
    {

        RubikPiecePositionByPivot p = RubikCube.GetPiecePositionBasedOn(pivots);
        return GetPiece(p);
    }




    public new void Reset()
    {
        Refresh();
    }

    private void Refresh()
    {
        base.Reset();
        m_pieces = GetComponentsInChildren<TagRubikCubePiece>();
        m_faces = GetComponentsInChildren<TagRubikCubeFace>();
    }

   


}