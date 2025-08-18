
using System.Collections.Generic;

public class RubikFaceDictionary<T>
{

    Dictionary<string, T> m_directAccess = new Dictionary<string, T>();

    public void Add(RubikCubeFace face, RubikCubeFaceDirection direction, T item)
    {
        string tag = RubikCube.GetTagOf(face, direction);
        if (!m_directAccess.ContainsKey(tag))
            m_directAccess.Add(tag, item);
        else m_directAccess[tag] = item;
    }
    public void Remove(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        string tag = RubikCube.GetTagOf(face, direction);
        m_directAccess.Remove(tag);
    }

    public bool Contains(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        string tag = RubikCube.GetTagOf(face, direction);
        return m_directAccess.ContainsKey(tag);
    }

    public T Get(RubikCubeFace face, RubikCubeFaceDirection direction)
    {

        if (!Contains(face, direction))
            throw new System.Exception("You try to access a missing information. Check that it has been add before");
        string tag = RubikCube.GetTagOf(face, direction);
        return m_directAccess[tag];

    }

}
public class RubikPieceDictionary<T>
{

    Dictionary<string, T> m_directAccess = new Dictionary<string, T>();

    public bool Contains(RubikCubeDepth depth, RubikCubeFaceDirection direction)
    {
        return Contains(RubikCube.GetPiecePositionBasedOn(depth, direction));
    }
    public bool Contains(RubikPieceByPosition3D piece)
    {
        string tag = RubikCube.GetTagOf(piece);
        return m_directAccess.ContainsKey(tag);
    }

    public T Get(RubikCubeDepth depth, RubikCubeFaceDirection direction)
    {
        return Get(RubikCube.GetPiecePositionBasedOn(depth, direction));
    }
    public T Get(RubikPieceByPosition3D piece)
    {

        if (!Contains(piece))
            throw new System.Exception("You try to access a missing information. Check that it has been add before");
        string tag = RubikCube.GetTagOf(piece);
        return m_directAccess[tag];

    }

    public void Add(RubikCubeDepth depth, RubikCubeFaceDirection direction, T item)
    {
        RubikPieceByPosition3D position = RubikCube.GetPiecePositionBasedOn(depth, direction);
        Add(position, item);
    }

    public void Add(RubikPieceByPosition3D piece, T item)
    {
        string tag = RubikCube.GetTagOf(piece);
        if (!m_directAccess.ContainsKey(tag))
            m_directAccess.Add(tag, item);
        else m_directAccess[tag] = item;
    }
    public void Remove(RubikPieceByPosition3D piece)
    {
        if (!Contains(piece))
            return;
        string tag = RubikCube.GetTagOf(piece);
        m_directAccess.Remove(tag);
    }
}