using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubeSafeStateMono : MonoBehaviour
{
    public RubikCubeSaveState m_state;
    public bool m_isCubeSolved;

    //TMP
   // public RubikCubeInstanceManager m_selectedCube;

    //void Start()
    //{
    //    RubikCubeSaveState stateSaved = m_selectedCube.GetRubikcubeStateReference();
    //    CubeDirectionalState cubeState = m_selectedCube.GetCubeStateReference();
    //}
}

[System.Serializable]
public class RubikCubeSaveState
{

    public void SetSequences( string global)
    {
        SetSequences(null, global);
    }
        public void SetSequences(string shuffle, string global)
    {
        if (shuffle == null)
            shuffle = "";
        if (global == null)
            global = "";
        RotationTypeShort[] shuffleSequence = RubikCube.ConvertStringToShorts(shuffle);
        RotationTypeShort[] globalSequence = RubikCube.ConvertStringToShorts(global);
        SetSequences(shuffleSequence, globalSequence);
    }

    public void SetSequences(RotationTypeShort[] global) {
        SetSequences(null, global);
    }
    public void SetSequences(RotationTypeShort[] shuffle, RotationTypeShort[] global)
    {
        if (shuffle == null)
            shuffle = new RotationTypeShort[0];
        if (global == null)
            global = new RotationTypeShort[0];

        m_shuffleSequence = new RotationSequence( shuffle);
        m_globalSequence = new RotationSequence(global);
        m_playerSequence = new RotationSequence();
        for (int i = 0; i < global.Length; i++)
        {
            if (i < shuffle.Length && shuffle[i] != global[i])
            {
                throw new Exception("Global should start with shuffle parameters");
            }
            else m_playerSequence.Add(global[i]);
        }
    }

    internal bool HasBeenShuffled()
    {
       return m_shuffleSequence != null && m_shuffleSequence.Lenght > 0;
    }

    public RotationSequence m_shuffleSequence;
    public RotationSequence m_globalSequence;
    public RotationSequence m_playerSequence;
    public CubeDirectionalState m_givenCubeState;

    public Color GetUnityColorOf(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        return RubikCube.GetDefaultColorOfCube(GetRubikColorOf(face, direction));
    }
    public RubikCubeColor GetRubikColorOf(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
       return RubikCube.GetColor(m_givenCubeState.GetFace(face).GetFace(direction).m_face);
    }

    public bool IsCubeSolved()
    {
        return m_givenCubeState.IsCubeSolved();
    }
    public void SetCubeDirecitonalState(CubeDirectionalState cubeState)
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

        public IEnumerable<CubeFaceState> GetFaces()
        {
            return new CubeFaceState[] { m_up, m_face, m_down, m_back, m_left, m_right };
        }

    public CubeFaceState GetFace(RubikCubeFace face)
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
    public void SetFace(RubikCubeFace face, RubikCubeFaceDirection direction, RubikCubeFace newFace, RubikCubeFaceDirection newDirection)
    {
        CubeFaceState cubeface = GetFace(face);
        cubeface.SetFace(direction, newFace, newDirection);
    }

    internal bool IsCubeSolved()
    {
        foreach (CubeFaceState face in GetFaces())
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
        public FaceState m_northWest, m_north, m_northEast;
        public FaceState m_west, m_center, m_east;
        public FaceState m_southWest, m_south, m_southEast;

        public CubeFaceState(RubikCubeFace face)
        {
            this.m_face = face;
            m_northWest = new FaceState(face, RubikCubeFaceDirection.NO);
            m_north = new FaceState(face, RubikCubeFaceDirection.N);
            m_northEast = new FaceState(face, RubikCubeFaceDirection.NE);
            m_west = new FaceState(face, RubikCubeFaceDirection.O);
            m_center = new FaceState(face, RubikCubeFaceDirection.N);
            m_east = new FaceState(face, RubikCubeFaceDirection.E);
            m_southWest = new FaceState(face, RubikCubeFaceDirection.SO);
            m_south = new FaceState(face, RubikCubeFaceDirection.S);
            m_southEast = new FaceState(face, RubikCubeFaceDirection.SE);
        }

        public bool IsFaceSolved()
        {
            RubikCubeFace faceType;
            return IsFaceSolved(out faceType);

        }
        public bool IsFaceSolved(out RubikCubeFace faceType)
        {
            faceType = m_center.m_face;
            foreach (FaceState face in GetFaces())
            {
                if (faceType != face.m_face)
                    return false;
            }
            return true;

        }


        public IEnumerable<FaceState> GetFaces()
        {
            return new FaceState[] {
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

    public FaceState GetFace(RubikCubeFaceDirection direction)
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
            case RubikCubeFaceDirection.NE: return m_southEast;
            default:
                return null;
        }
    }
    public void SetFace(RubikCubeFaceDirection direction,RubikCubeFace newFace , RubikCubeFaceDirection newDirection  ) {
        FaceState face = GetFace(direction);
        face.m_face = newFace;
        face.m_direction = newDirection;
    }
}

[System.Serializable]
public class FaceState
{
    public RubikCubeFace m_face;
    public RubikCubeFaceDirection m_direction;

    public FaceState(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        this.m_face = face;
        this.m_direction = direction;
    }
}
public class FaceStateLinkedInfo<T> : FaceState
{

    [SerializeField] T m_value;

    public FaceStateLinkedInfo(T value, RubikCubeFace face, RubikCubeFaceDirection direction) : base(face, direction)
    {
        this.m_value = value;
    }

    public T GetValu() { return m_value; }
    public void SetValue(T value) { m_value = value; }
}

