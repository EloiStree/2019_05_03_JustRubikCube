using System.Collections.Generic;
using UnityEngine;
public class TagRubikCube : MonoBehaviour
{
    public TagRubikCubePiece [] m_pieces;
    public TagRubikCubeFace[] m_faces;
    public Dictionary<string, TagRubikCubeFace> m_faceDirectAccess = new Dictionary<string, TagRubikCubeFace>();
    public Dictionary<string, TagRubikCubePiece> m_pieceDirectAccess = new Dictionary<string, TagRubikCubePiece>();

    public void Start()
    {
        for (int i = 0; i < m_pieces.Length; i++)
        {
            string tag = RubikCube.GetTagOf(
                    m_pieces[i].GetCubePosition());
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
       return m_faceDirectAccess[RubikCube.GetTagOf(face, direction)];
    }
    public TagRubikCubePiece GetPiece(RubikPiecePosition position)
    {
        return m_pieceDirectAccess[RubikCube.GetTagOf(position)];
    }

    public TagRubikCubePiece GetPiece(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        RubikPiecePosition p = RubikCube.GetPiecePositionBasedOn(face, direction);
        return GetPiece(p);
    }
    
    public TagRubikCubePiece GetPiece(RubikCubePivotable[] pivots)
    {

        RubikPiecePosition p = RubikCube.GetPiecePositionBasedOn(pivots);
        return GetPiece(p);
    }




    public void Reset()
    {
        m_pieces = GetComponentsInChildren<TagRubikCubePiece>();
        m_faces = GetComponentsInChildren<TagRubikCubeFace>();
    }


}