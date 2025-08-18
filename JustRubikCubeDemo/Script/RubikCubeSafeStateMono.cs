using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RubikCubeSafeStateMono : MonoBehaviour
{
    public RubikCubeSaveState m_state;
    public bool m_isCubeSolved;
    [Header("Event")]
    public  RubikCubeStateChangeEvent onCubeChange;
    public  void RefreshCubeState(CubeDirectionalState cubeDirectionalState)
    {
        m_state.SetCubeDirectionalState(cubeDirectionalState);
        onCubeChange.Invoke(m_state);
    }
    
}


[System.Serializable]
public class RubikCubeSaveState
{
    public void SetSequences( string global)
    {
        if (global == null)
            global = "";
        RotationTypeShort[] globalSequence = RubikCube.ConvertStringToShorts(global);
        SetSequences( globalSequence);
    }

    public void SetSequences(RotationTypeShort[] global)
    {
        if (global == null)
            global = new RotationTypeShort[0];
        m_globalSequence = new RotationSequence(global);
       
    }
    
    public RotationSequence m_globalSequence;
    public CubeDirectionalState m_givenCubeState;

    public Color GetUnityColorOf(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        return RubikCube.GetDefaultColorOfCube(GetRubikColorOf(face, direction));
    }
    public RubikCubeColor GetRubikColorOf(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
       return RubikCube.GetColor(m_givenCubeState.GetCubeFace(face).GetFace(direction).m_face);
    }

    public bool IsCubeSolved()
    {
        return m_givenCubeState.IsCubeSolved();
    }
    public void SetCubeDirectionalState(CubeDirectionalState cubeState)
    {
        m_givenCubeState = cubeState;
    }
    public bool IsGivenCubeMixedUp() { throw new System.NotImplementedException(); }
    public CubeDirectionalState GetCubeStateBasedOnSequence() { throw new System.NotImplementedException();
    }
}
[System.Serializable]
public class CubeDirectionalState
    {
        public CubeFaceState m_up = new CubeFaceState(RubikCubeFace.Up);
        public CubeFaceState m_face = new CubeFaceState(RubikCubeFace.Face);
        public CubeFaceState m_down = new CubeFaceState(RubikCubeFace.Down);
        public CubeFaceState m_back = new CubeFaceState(RubikCubeFace.Back);
        public CubeFaceState m_left = new CubeFaceState(RubikCubeFace.Left);
        public CubeFaceState m_right = new CubeFaceState(RubikCubeFace.Right);

    public CubeFaceState[] m_allCubeFaces;
    public PieceFaceState[] m_allPieceFaces;


       public CubeFaceState [] GetAllCubeFaces()
        {
            if(m_allCubeFaces==null)
                m_allCubeFaces=  new CubeFaceState[] { m_up, m_face, m_down, m_back, m_left, m_right };
        return m_allCubeFaces;
        }
        public PieceFaceState [] GetAllPieceFaces()
        {
        if (m_allPieceFaces == null) {
            List<PieceFaceState> faces = new List<PieceFaceState>();
            faces.AddRange(m_up.GetFaces());
            faces.AddRange(m_face.GetFaces());
            faces.AddRange(m_down.GetFaces());
            faces.AddRange(m_back.GetFaces());
            faces.AddRange(m_left.GetFaces());
            faces.AddRange(m_right.GetFaces());
            m_allPieceFaces = faces.ToArray();
        }
        return m_allPieceFaces;
        }

    public CubeFaceState GetCubeFace(RubikCubeFace face)
    {
        switch (face)
        {
            case RubikCubeFace.Left:return m_left;
            case RubikCubeFace.Right: return m_right;
            case RubikCubeFace.Up: return m_up;
            case RubikCubeFace.Down: return m_down;
            case RubikCubeFace.Face: return m_face;
            case RubikCubeFace.Back: return m_back;
            default:
                return null;
        }
    }
    public void SetPieceFace(RubikCubeFace face, RubikCubeFaceDirection direction, RubikCubeFace newFace, RubikCubeFaceDirection newDirection)
    {
        CubeFaceState cubeface = GetCubeFace(face);
        cubeface.SetFace(direction, newFace, newDirection);
    }

    public void GetRealPieceFaceInfoAt(RubikCubeFace face, RubikCubeFaceDirection direction, out RubikCubeFace newFace, out RubikCubeFaceDirection newDirection) {

        CubeFaceState cubeface = GetCubeFace(face);
        cubeface.GetRealFaceAt( direction, out  newFace, out  newDirection);
    }


    public bool IsCubeSolved()
    {
        foreach (CubeFaceState face in GetAllCubeFaces())
        {
            if (!face.IsFaceSolved())
                return false;
        }
        return true;
    }
}

    [System.Serializable]
    public class CubeFaceState
    {
        public RubikCubeFace m_face;
        public PieceFaceState m_northWest, m_north, m_northEast;
        public PieceFaceState m_west, m_center, m_east;
        public PieceFaceState m_southWest, m_south, m_southEast;

        public CubeFaceState(RubikCubeFace face)
        {
        Reset(face);
        }

        public bool IsFaceSolved()
        {
            RubikCubeFace faceType;
            return IsFaceSolved(out faceType);

        }
        public bool IsFaceSolved(out RubikCubeFace faceType)
        {
            faceType = m_center.m_face;
            foreach (PieceFaceState face in GetFaces())
            {
                if (faceType != face.m_face)
                    return false;
            }
            return true;

        }


        public IEnumerable<PieceFaceState> GetFaces()
        {
            return new PieceFaceState[] {
                m_northWest,
                m_north,
                m_northEast,
                m_west,
                m_center,
                m_east,
                m_southWest,
                m_south,
                m_southEast
            };
        }

    public PieceFaceState GetFace(RubikCubeFaceDirection direction)
    {
        switch (direction)
        {
            case RubikCubeFaceDirection.SO: return m_southWest;
            case RubikCubeFaceDirection.S: return m_south;
            case RubikCubeFaceDirection.SE: return m_southEast;
            case RubikCubeFaceDirection.O: return m_west;
            case RubikCubeFaceDirection.C: return m_center;
            case RubikCubeFaceDirection.E: return m_east;
            case RubikCubeFaceDirection.NO: return m_northWest;
            case RubikCubeFaceDirection.N: return m_north;
            case RubikCubeFaceDirection.NE: return m_northEast;
            default:
                return null;
        }
    }
    public void SetFace(RubikCubeFaceDirection direction,RubikCubeFace newFace , RubikCubeFaceDirection newDirection  ) {
        PieceFaceState face = GetFace(direction);
        face.m_face = newFace;
        face.m_direction = newDirection;
    }

    public void GetRealFaceAt(RubikCubeFaceDirection direction, out RubikCubeFace newFace, out RubikCubeFaceDirection newDirection)
    {
        PieceFaceState face = GetFace(direction);
        newFace = face.m_face;
        newDirection = face.m_direction;
    }

    public void Reset() {
        Reset(m_face);
    }
    public void Reset( RubikCubeFace face)
    {
        this.m_face = face;
        m_northWest = new PieceFaceState(face, RubikCubeFaceDirection.NO);
        m_north = new PieceFaceState(face, RubikCubeFaceDirection.N);
        m_northEast = new PieceFaceState(face, RubikCubeFaceDirection.NE);
        m_west = new PieceFaceState(face, RubikCubeFaceDirection.O);
        m_center = new PieceFaceState(face, RubikCubeFaceDirection.N);
        m_east = new PieceFaceState(face, RubikCubeFaceDirection.E);
        m_southWest = new PieceFaceState(face, RubikCubeFaceDirection.SO);
        m_south = new PieceFaceState(face, RubikCubeFaceDirection.S);
        m_southEast = new PieceFaceState(face, RubikCubeFaceDirection.SE);
    }
}

[System.Serializable]
public class PieceFaceState
{
    public RubikCubeFace m_face;
    public RubikCubeFaceDirection m_direction;

    public PieceFaceState(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        this.m_face = face;
        this.m_direction = direction;
    }

    
}
public class FaceStateLinkedInfo<T> : PieceFaceState
{

    [SerializeField] T m_value;

    public FaceStateLinkedInfo(T value, RubikCubeFace face, RubikCubeFaceDirection direction) : base(face, direction)
    {
        this.m_value = value;
    }

    public T GetValu() { return m_value; }
    public void SetValue(T value) { m_value = value; }
}



