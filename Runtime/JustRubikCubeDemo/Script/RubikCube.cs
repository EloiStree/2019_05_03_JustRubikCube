using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;




//Info https://ruwix.com/the-rubiks-cube/fingertricks/

public interface IRubikCube {

    TagRubikCube GetLinkedCube();
    TagRubikCubeFace GetLinkedFace(RubikCubeFace face, RubikCubeFaceDirection direction);
    TagRubikCubePiece GetLinkedPiece(RubikPieceByPosition3D position);
    TagRubikCubePivot GetLinkedPivot(RubikCubePivotable pivot);

}


public class RubikCube : MonoBehaviour {

    [Header("Params")]
    public Transform m_root;


    [Header("Pivot")]
    public Transform m_left;
    public Transform m_right;
    public Transform m_up;
    public Transform m_down;
    public Transform m_face;
    public Transform m_back;
    public Transform m_middle;
    public Transform m_equator;
    public Transform m_standing;
    public RubikCubePivot[] m_pivots;

    public TagRubikCube m_fixedCube;
    public TagRubikCube m_movingCube;
    public RubikCubeRotateMotor m_rotationMotor;

    [Header("Event")]

    public static OnRubikCubeUsed onAnyRubikCubeUsed = new OnRubikCubeUsed();



    public static RotationTypeShort[] InverseOf(RotationTypeShort[] rotations)
    {
        RotationTypeShort[] result = new RotationTypeShort[rotations.Length];
        for (int i = 0; i < rotations.Length; i++)
        {
            result[rotations.Length-1-i] = RubikCube.GetInverseOf(rotations[i]);
        }
        return result;
    }

    internal static RotationTypeShort ConvertAsShort(RotationTypeLong value)
    {
        switch (value)
        {
            case RotationTypeLong.Left: return RotationTypeShort.L;
            case RotationTypeLong.LeftCounter: return RotationTypeShort.Lp;
            case RotationTypeLong.Right: return RotationTypeShort.R;
            case RotationTypeLong.RightCounter: return RotationTypeShort.Rp;
            case RotationTypeLong.Up: return RotationTypeShort.U;
            case RotationTypeLong.UpCounter: return RotationTypeShort.Rp;
            case RotationTypeLong.Down: return RotationTypeShort.D;
            case RotationTypeLong.DownCounter: return RotationTypeShort.Dp;
            case RotationTypeLong.Face: return RotationTypeShort.F;
            case RotationTypeLong.FaceCounter: return RotationTypeShort.Fp;
            case RotationTypeLong.Back: return RotationTypeShort.B;
            case RotationTypeLong.BackCounter: return RotationTypeShort.Bp;
            case RotationTypeLong.Middle: return RotationTypeShort.M;
            case RotationTypeLong.MiddleCounter: return RotationTypeShort.Mp;
            case RotationTypeLong.Equator: return RotationTypeShort.E;
            case RotationTypeLong.EquatorCounter: return RotationTypeShort.Ep;
            case RotationTypeLong.Standing: return RotationTypeShort.S;
            case RotationTypeLong.StandingCounter: return RotationTypeShort.Sp;
            default:
                return RotationTypeShort.L;
        }
    }

  


    internal static RotationTypeShort GetInverseOf(RotationTypeShort value)
    {
        switch (value)
        {
            case RotationTypeShort.L:
                return RotationTypeShort.Lp;
            case RotationTypeShort.Lp:
                return RotationTypeShort.L;
            case RotationTypeShort.R:
                return RotationTypeShort.Rp;
            case RotationTypeShort.Rp:
                return RotationTypeShort.R;
            case RotationTypeShort.U:
                return RotationTypeShort.Up;
            case RotationTypeShort.Up:
                return RotationTypeShort.U;
            case RotationTypeShort.D:
                return RotationTypeShort.Dp;
            case RotationTypeShort.Dp:
                return RotationTypeShort.D;
            case RotationTypeShort.F:
                return RotationTypeShort.Fp;
            case RotationTypeShort.Fp:
                return RotationTypeShort.F;
            case RotationTypeShort.B:
                return RotationTypeShort.Bp;
            case RotationTypeShort.Bp:
                return RotationTypeShort.B;
            case RotationTypeShort.M:
                return RotationTypeShort.Mp;
            case RotationTypeShort.Mp:
                return RotationTypeShort.M;
            case RotationTypeShort.E:
                return RotationTypeShort.Ep;
            case RotationTypeShort.Ep:
                return RotationTypeShort.E;
            case RotationTypeShort.S:
                return RotationTypeShort.Sp;
            case RotationTypeShort.Sp:
                return RotationTypeShort.S;
            default:
                return RotationTypeShort.Lp;
        }
    }

    public Dictionary<RubikCubePivotable, List<TagRubikCubePiece>> m_faceLinkedToSpots = new Dictionary<RubikCubePivotable, List<TagRubikCubePiece>>();

    void Awake() {
        foreach (TagRubikCubePiece spot in m_fixedCube.m_pieces)
        {
            foreach (RubikCubePivotable face in spot.GetPivots())
            {
                AddSpotToToRegister(face, spot);
            }
        }
        SaveInitialState();
        SaveCubeState();
        m_rotationMotor.m_onStartRotating.AddListener(OnCubeRotating);
        m_rotationMotor.m_onRotated.AddListener(OnCubeRotated);
    }

    private void OnCubeRotating(LocalRotationRequest arg0)
    {
        m_onStartRotating.Invoke(arg0);
    }

    private void OnCubeRotated(LocalRotationRequest rotationRequested)
    {
        RubikCubeFace[] allFaces = GetArrayOf<RubikCubeFace>();
        RubikCubeFaceDirection[] allDirections = GetArrayOf<RubikCubeFaceDirection>();
        for (int iFace = 0; iFace < allFaces.Length; iFace++)
        {
            for (int jDirection = 0; jDirection < allDirections.Length; jDirection++)
            {
                RubikCubeFace currentFace;
                RubikCubeFaceDirection currentDirection;
                GetCurrentFaceAt(
                    allFaces[iFace],
                    allDirections[jDirection],
                    out currentFace,
                    out currentDirection);
                m_cubeFaceDirectionState.SetPieceFace(
                    allFaces[iFace],
                    allDirections[jDirection],
                    currentFace,
                    currentDirection);
            }
        }

        SaveCubeState();
        m_onRotated.Invoke(rotationRequested);
    }

    private TagRubikCubeFace GetCurrentFaceAt(RubikCubeFace face, RubikCubeFaceDirection direction, out RubikCubeFace currentFace, out RubikCubeFaceDirection currentDirection)
    {
        TagRubikCubeFace faceOrigine = GetCorrespondingOrigineFace(face, direction);
        TagRubikCubeFace nearestFace = m_movingCube.m_faces.OrderBy(k => Vector3.Distance(k.GetPosition(), faceOrigine.GetPosition())).First();
        currentFace = nearestFace.m_belongToFace;
        currentDirection = nearestFace.m_faceDirection;
        return nearestFace;
        //
    }

    private TagRubikCubeFace GetCorrespondingOrigineFace(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        TagRubikCubeFace[] faces = m_fixedCube.m_faces;
        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i].m_faceDirection == direction &&
                faces[i].m_belongToFace == face)
                return faces[i];
        }
        throw new Exception("A face is missing");
    }

    private void SaveCubeState()
    {
        m_onSaveStateChanged.Invoke(m_cubeFaceDirectionState);
    }

    public void DebugDisplayFace(RubikCubePivotable face, float time, Color color)
    {
        List<TagRubikCubePiece> spots = GetSpots(face);
        foreach (TagRubikCubePiece spot in spots)
        {
            DebugUtility.DrawCross(spot.m_root, 0.2f, color, time);

        }
    }

    private List<TagRubikCubePiece> GetSpots(RubikCubePivotable face)
    {
        return m_faceLinkedToSpots[face];
    }

    private void AddSpotToToRegister(RubikCubePivotable face, TagRubikCubePiece spot)
    {
        if (!m_faceLinkedToSpots.ContainsKey(face))
            m_faceLinkedToSpots.Add(face, new List<TagRubikCubePiece>());
        m_faceLinkedToSpots[face].Add(spot);
    }

    void Update() {

    }

    internal Transform GetPivotTransform(RubikCubePivotable faceToTurn)
    {
        switch (faceToTurn)
        {
            case RubikCubePivotable.Left: return m_left;
            case RubikCubePivotable.Right: return m_right;
            case RubikCubePivotable.Up: return m_up;
            case RubikCubePivotable.Down: return m_down;
            case RubikCubePivotable.Face: return m_face;
            case RubikCubePivotable.Back: return m_back;
            case RubikCubePivotable.Middle: return m_middle;
            case RubikCubePivotable.Equator: return m_equator;
            case RubikCubePivotable.Standing: return m_standing;
        }
        return null;
    }

    internal TagRubikCubePiece[] GetPieces(RubikCubePivotable face)
    {
        List<TagRubikCubePiece> spots = GetSpots(face);
        List<TagRubikCubePiece> pieces = new List<TagRubikCubePiece>();
        foreach (TagRubikCubePiece spot in spots)
        {
            TagRubikCubePiece piece = GetClosestPieceOf(spot.m_root);
            pieces.Add(piece);
        }
        return pieces.ToArray();
    }

    private TagRubikCubePiece GetClosestPieceOf(Transform m_root)
    {
        TagRubikCubePiece closest = null;
        float smallestdistance = float.MaxValue;
        //HERE
        foreach (TagRubikCubePiece piece in m_movingCube.m_pieces)
        {
            float dist = Vector3.Distance(m_root.position, piece.m_root.position);
            if (dist < smallestdistance)
            {
                smallestdistance = dist;
                closest = piece;
            }

        }
        return closest;
    }


    internal void AddLocalRotationSequence(RotationTypeShort shortRotation)
    {
        string seq = RubikCube.ConvertAcronymShortToString(shortRotation);
        AddLocalRotationSequence(seq);
    }

    public void AddLocalRotationSequence(string sequence)
    {
        AddRotationSequence(sequence, null);
    }
    public void AddRotationSequenceWithDefaultCamera(string sequence)
    {
        AddRotationSequence(sequence, GetDefaultOrientation());
    }

    internal static RotationTypeLong ConvertAcronymShortToLong(RotationTypeShort rotationTypeShort)
    {
        switch (rotationTypeShort)
        {
            case RotationTypeShort.L: return RotationTypeLong.Left;
            case RotationTypeShort.Lp: return RotationTypeLong.LeftCounter;
            case RotationTypeShort.R: return RotationTypeLong.Right;
            case RotationTypeShort.Rp: return RotationTypeLong.RightCounter;
            case RotationTypeShort.U: return RotationTypeLong.Up;
            case RotationTypeShort.Up: return RotationTypeLong.UpCounter;
            case RotationTypeShort.D: return RotationTypeLong.Down;
            case RotationTypeShort.Dp: return RotationTypeLong.DownCounter;
            case RotationTypeShort.F: return RotationTypeLong.Face;
            case RotationTypeShort.Fp: return RotationTypeLong.FaceCounter;
            case RotationTypeShort.B: return RotationTypeLong.Back;
            case RotationTypeShort.Bp: return RotationTypeLong.BackCounter;
            case RotationTypeShort.M: return RotationTypeLong.Middle;
            case RotationTypeShort.Mp: return RotationTypeLong.MiddleCounter;
            case RotationTypeShort.E: return RotationTypeLong.Equator;
            case RotationTypeShort.Ep: return RotationTypeLong.EquatorCounter;
            case RotationTypeShort.S: return RotationTypeLong.Standing;
            case RotationTypeShort.Sp: return RotationTypeLong.StandingCounter;

            default:
                return RotationTypeLong.Back;

        }
    }

    public Transform GetDefaultOrientation() {
        return Camera.main.transform;
    }

    public void AddRotationSequence(string sequence, Transform orientation)
    {

        string[] seg = sequence.Split(new char[] { ' ', ':', ';', ',' });
        foreach (string segment in seg)
        {
            try
            {

                if (orientation == null)
                    AddLocalRotate(segment);
                else
                    RotateFaceFrom(segment, orientation);
            }
            catch (Exception) { }
        }
    }
    public void AddLocalRotate(string rotation)
    {
        RotationTypeShort rotationType;
        if (ConvertStringToShortAcronym(rotation, out rotationType))
            AddLocalRotate(rotationType);
    }


    public void AddLocalRotate(RubikCubePivotable faceToTurn, bool clockWise)
    {
        onAnyRubikCubeUsed.Invoke(this);
        m_rotationMotor.LocalRotate(faceToTurn, clockWise);
    }

    public void FinishMotorQueuedRotation()
    {
        m_rotationMotor.FinishQueuedRotation();

    }

    #region ROTATION LISTENER
    [Header("Events")]
    public RotationEvent m_onStartRotating = new RotationEvent();
    public RotationEvent m_onRotated = new RotationEvent();
    public RubikCubeStateChangeEvent m_onSaveStateChanged = new RubikCubeStateChangeEvent();

    [System.Serializable]
    public class RotationEvent : UnityEvent<LocalRotationRequest>
    {

    }
    [System.Serializable]
    public class RubikCubeStateChangeEvent : UnityEvent<CubeDirectionalState>
    {
    }


    [System.Serializable]
    public class LocalRotationRequest
    {
        public RubikCubePivotable m_faceToRotate;
        public bool m_clockWise;

        public LocalRotationRequest(RubikCubePivotable face, bool clockWise)
        {
            this.m_faceToRotate = face;
            this.m_clockWise = clockWise;
        }
    }
    public void NotifyStartRotation(RubikCubePivotable face, bool clockWise)
    {
        NotifyStartRotation(new LocalRotationRequest(face, clockWise));
    }
    public void NotifyEndRotation(RubikCubePivotable face, bool clockWise)
    {
        NotifyStartRotation(new LocalRotationRequest(face, clockWise));
    }
    public void NotifyStartRotation(LocalRotationRequest req)
    {
        m_onStartRotating.Invoke(req);
    }
    public void NotifyEndRotation(LocalRotationRequest req)
    {
        m_onRotated.Invoke(req);
    }
    public void NotifyStateChange(CubeDirectionalState newState)
    {
        m_onSaveStateChanged.Invoke(newState);
        if (IsCubeResolved())
            NotifyCubeAsResolved();
    }


    #endregion

    #region Solution LISTENER

    public CubeDirectionalState m_cubeFaceDirectionState = new CubeDirectionalState();
    public UnityEvent m_onCubeResolved;


    public bool IsCubeResolved()
    {
        return m_cubeFaceDirectionState.IsCubeSolved();
    }

    public void NotifyCubeAsResolved() {
        m_onCubeResolved.Invoke();
    }

    #endregion

    public void LocalRotateWithAcronym(string requestAcryonym) {
        RotationTypeShort acronym;
        if (ConvertStringToShortAcronym(requestAcryonym, out acronym))
            AddLocalRotate(acronym);
    }



    public void AddLocalRotate(RotationTypeShort instruction) {
        RubikCubePivotable faceToRotate;
        bool clockwise;
        ConvertAcronymToFaceRotation(instruction, out faceToRotate, out clockwise);
        AddLocalRotate(faceToRotate, clockwise);

    }

    public static void ConvertAcronymToFaceRotation(RotationTypeShort instruction, out RubikCubePivotable faceToRotate, out bool clockwise)
    {
        switch (instruction)
        {
            case RotationTypeShort.L: clockwise = true; faceToRotate = RubikCubePivotable.Left;
                break;
            case RotationTypeShort.Lp:
                clockwise = false; faceToRotate = RubikCubePivotable.Left;
                break;
            case RotationTypeShort.R:
                clockwise = true; faceToRotate = RubikCubePivotable.Right;
                break;
            case RotationTypeShort.Rp:
                clockwise = false; faceToRotate = RubikCubePivotable.Right;
                break;
            case RotationTypeShort.U:
                clockwise = true; faceToRotate = RubikCubePivotable.Up;
                break;
            case RotationTypeShort.Up:
                clockwise = false; faceToRotate = RubikCubePivotable.Up;
                break;
            case RotationTypeShort.D:
                clockwise = true; faceToRotate = RubikCubePivotable.Down;
                break;
            case RotationTypeShort.Dp:
                clockwise = false; faceToRotate = RubikCubePivotable.Down;
                break;
            case RotationTypeShort.F:
                clockwise = true; faceToRotate = RubikCubePivotable.Face;
                break;
            case RotationTypeShort.Fp:
                clockwise = false; faceToRotate = RubikCubePivotable.Face;
                break;
            case RotationTypeShort.B:
                clockwise = true; faceToRotate = RubikCubePivotable.Back;
                break;
            case RotationTypeShort.Bp:
                clockwise = false; faceToRotate = RubikCubePivotable.Back;
                break;
            case RotationTypeShort.M:
                clockwise = true; faceToRotate = RubikCubePivotable.Middle;
                break;
            case RotationTypeShort.Mp:
                clockwise = false; faceToRotate = RubikCubePivotable.Middle;
                break;
            case RotationTypeShort.E:
                clockwise = true; faceToRotate = RubikCubePivotable.Equator;
                break;
            case RotationTypeShort.Ep:
                clockwise = false; faceToRotate = RubikCubePivotable.Equator;
                break;
            case RotationTypeShort.S:
                clockwise = true; faceToRotate = RubikCubePivotable.Standing;
                break;
            case RotationTypeShort.Sp:
                clockwise = false; faceToRotate = RubikCubePivotable.Standing;
                break;
            default:
                clockwise = true; faceToRotate = RubikCubePivotable.Middle;
                break;
        }
    }
    
    public static string ConvertAcronymShortToString(RotationTypeShort rotation)
    {
        RubikCubePivotable face;
        bool clockwise;
        ConvertAcronymToFaceRotation(rotation, out face, out clockwise);
        return ConvertFaceRotationToString(face, clockwise);

    }
    public static string ConvertFaceRotationToString(RubikCubePivotable faceToRotate, bool clockWise)
    {
        RotationTypeShort rotType = ConvertRotationToAcronymShort(faceToRotate, clockWise);
        switch (rotType)
        {
            case RotationTypeShort.L: return "L";
            case RotationTypeShort.Lp: return "L'";
            case RotationTypeShort.R: return "R";
            case RotationTypeShort.Rp: return "R'";
            case RotationTypeShort.U: return "U";
            case RotationTypeShort.Up: return "U'";
            case RotationTypeShort.D: return "D";
            case RotationTypeShort.Dp: return "D'";
            case RotationTypeShort.F: return "F";
            case RotationTypeShort.Fp: return "F'";
            case RotationTypeShort.B: return "B";
            case RotationTypeShort.Bp: return "B'";
            case RotationTypeShort.M: return "M";
            case RotationTypeShort.Mp: return "M'";
            case RotationTypeShort.E: return "E";
            case RotationTypeShort.Ep: return "E'";
            case RotationTypeShort.S: return "S";
            case RotationTypeShort.Sp: return "S'";
            default: return "?";
        }


    }

    public static RotationTypeShort ConvertRotationToAcronymShort(RubikCubePivotable faceToRotate, bool clockWise)
    {
        switch (faceToRotate)
        {
            case RubikCubePivotable.Left: return clockWise ? RotationTypeShort.L : RotationTypeShort.Lp;
            case RubikCubePivotable.Right: return clockWise ? RotationTypeShort.R : RotationTypeShort.Rp;
            case RubikCubePivotable.Up: return clockWise ? RotationTypeShort.U : RotationTypeShort.Up;
            case RubikCubePivotable.Down: return clockWise ? RotationTypeShort.D : RotationTypeShort.Dp;
            case RubikCubePivotable.Face: return clockWise ? RotationTypeShort.F : RotationTypeShort.Fp;
            case RubikCubePivotable.Back: return clockWise ? RotationTypeShort.B : RotationTypeShort.Bp;
            case RubikCubePivotable.Middle: return clockWise ? RotationTypeShort.M : RotationTypeShort.Mp;
            case RubikCubePivotable.Equator: return clockWise ? RotationTypeShort.E : RotationTypeShort.Ep;
            case RubikCubePivotable.Standing: return clockWise ? RotationTypeShort.S : RotationTypeShort.Sp;
            default:
                return clockWise ? RotationTypeShort.E : RotationTypeShort.Ep;
        }
    }

    public static bool ConvertStringToShortAcronym(string requestAcryonym, out RotationTypeShort type)
    {
        requestAcryonym = requestAcryonym.ToLower();
        switch (requestAcryonym)
        {
            case "b": type = RotationTypeShort.B; break;
            case "b'": case "bp": case "bi": type = RotationTypeShort.Bp; break;
            case "d": type = RotationTypeShort.D; break;
            case "d'": case "dp": case "di": type = RotationTypeShort.Dp; break;
            case "e": type = RotationTypeShort.E; break;
            case "e'": case "ep": case "ei": type = RotationTypeShort.Ep; break;
            case "f": type = RotationTypeShort.F; break;
            case "f'": case "fp": case "fi": type = RotationTypeShort.Fp; break;
            case "l": type = RotationTypeShort.L; break;
            case "l'": case "lp": case "li": type = RotationTypeShort.Lp; break;
            case "m": type = RotationTypeShort.M; break;
            case "m'": case "mp": case "mi": type = RotationTypeShort.Mp; break;

            case "r": type = RotationTypeShort.R; break;
            case "r'": case "rp": case "ri": type = RotationTypeShort.Rp; break;

            case "s": type = RotationTypeShort.S; break;
            case "s'": case "sp": case "si": type = RotationTypeShort.Sp; break;

            case "u": type = RotationTypeShort.U; break;
            case "u'": case "up": case "ui": type = RotationTypeShort.Up; break;

            default:
                type = RotationTypeShort.B;
                return false;
        }
        return true;
    }


    public void SaveInitialState() {
        List<PieceInitialState> state = new List<PieceInitialState>();
        foreach (TagRubikCubePiece piece in m_movingCube.m_pieces)
        {
            state.Add(new PieceInitialState(piece, piece.transform.localRotation, piece.transform.localPosition));
        }
        m_initialState = state.ToArray();

    }
    public void ResetInitialState() {

        foreach (PieceInitialState state in m_initialState)
        {
            state.m_linkedPiece.transform.localPosition = state.m_initialPosition;
            state.m_linkedPiece.transform.localRotation = state.m_initialRotation;
        }

    }

    internal PieceInitialState[] GetInitialSpots()
    {
        return m_initialState;
    }

    //internal TagRubikCubePiece[] GetPieces()
    //{
    //    return m_initialSpot;
    //}

    private PieceInitialState[] m_initialState;

    [Serializable]
    public class PieceInitialState
    {
        public TagRubikCubePiece m_linkedPiece;
        public Quaternion m_initialRotation;
        public Vector3 m_initialPosition;

        public PieceInitialState(TagRubikCubePiece piece, Quaternion localRotation, Vector3 localPosition)
        {
            this.m_linkedPiece = piece;
            this.m_initialRotation = localRotation;
            this.m_initialPosition = localPosition;
        }
    }


    #region SHUFFLE
    public void Shuffle() {
        Shuffle(20);
    }
    public void Shuffle(int timeToRotate)
    {
        RotationSequence sequence = new RotationSequence();

        for (int i = 0; i < timeToRotate; i++)
        {
            sequence.Add(RubikCube.GetRandomShort());
        }
        Shuffle(sequence);
    }

    public void Shuffle(RotationSequence sequence) {

        foreach (RotationTypeShort item in sequence.GetSequenceAsShort())
        {
            AddLocalRotate(item);
        }
    }



    public static List<T> GetListOf<T>() { return Enum.GetValues(typeof(T)).Cast<T>().ToList(); }
    public static T[] GetArrayOf<T>() { return Enum.GetValues(typeof(T)).Cast<T>().ToArray(); }
    public static T GetEnumRandom<T>()
    {
        List<T> l = GetListOf<T>();
        return l[UnityEngine.Random.Range(0, l.Count)];
    }
    private static RotationTypeShort GetRandomShort()
    {
        return GetEnumRandom<RotationTypeShort>();
    }
    private static RotationTypeLong GetRandomLong()
    {
        return GetEnumRandom<RotationTypeLong>();
    }


    public static RubikCubePivotable GetRandomFace()
    {
        int face = UnityEngine.Random.Range(0, 9);
        switch (face)
        {
            case 0: return RubikCubePivotable.Back;
            case 1: return RubikCubePivotable.Down;
            case 2: return RubikCubePivotable.Equator;
            case 3: return RubikCubePivotable.Face;
            case 4: return RubikCubePivotable.Left;
            case 5: return RubikCubePivotable.Middle;
            case 6: return RubikCubePivotable.Right;
            case 7: return RubikCubePivotable.Standing;
            case 8: return RubikCubePivotable.Up;
            default:
                return RubikCubePivotable.Middle;
        }
    }

    private bool GetRandomDirection()
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

    internal bool IsRotating()
    {
        return m_rotationMotor.IsRotating();
    }
    #endregion

    #region ROTATE THE CUBE FROM POINT OF 
    public void RotateFaceFrom(string acronym)
    {
        RotateFaceFrom(acronym, GetDefaultOrientation());
    }
    public void RotateFaceFrom(string acronym, Transform orientation)
    {
        if (orientation == null)
            orientation = GetDefaultOrientation();
        RotationTypeShort rotationType;
        if (RubikCube.ConvertStringToShortAcronym(acronym, out rotationType))
            RotateFaceFrom(rotationType, orientation);
    }

    public void RotateFaceFrom(RotationTypeShort rotation, Transform orientation)
    {
        bool clockwise;
        RubikCubePivotable face;
        RubikCube.ConvertAcronymToFaceRotation(rotation, out face, out clockwise);
        RotateFaceFrom(face, clockwise, ref orientation);

    }
    public void RotateFaceFrom(RubikCubePivotable rotationFace, bool clockwise, ref Transform orientation)
    {
        RubikCubePivotable realFaceToRotate = GetRealFaceOf(rotationFace, ref orientation);
        AddLocalRotate(realFaceToRotate, clockwise);

    }

    #endregion

    #region CALIBER THE REAL FACE TO ROTATE IN FRONT OF THE CAMERA
    private RubikCubePivot GetPivotFrom(RubikCubePivotable face, Transform cameraOrientation)
    {
        switch (face)
        {
            case RubikCubePivotable.Left: return GetLeftPivot(cameraOrientation);
            case RubikCubePivotable.Right: return GetRightPivot(cameraOrientation);
            case RubikCubePivotable.Up: return GetUpPivot(cameraOrientation);
            case RubikCubePivotable.Down: return GetDownPivot(cameraOrientation);
            case RubikCubePivotable.Face: return GetFacePivot(cameraOrientation);
            case RubikCubePivotable.Back: return GetBackPivot(cameraOrientation);
            case RubikCubePivotable.Middle: return GetMiddlePivot(cameraOrientation);
            case RubikCubePivotable.Equator: return GetEquatorPivot(cameraOrientation);
            case RubikCubePivotable.Standing: return GetStandingPivot(cameraOrientation);
            default:
                throw new Exception("No face found");
        }
    }

    #region ACCESS PIVOT ALGORITHM
    #region GET PIVOT
    private RubikCubePivot GetLeftPivot(Transform m_refOrientationPoint)
    {
        return m_pivots.OrderBy(t => LeftDistanceOf(m_refOrientationPoint, t.transform.position)).First();
    }
    private RubikCubePivot GetRightPivot(Transform m_refOrientationPoint)
    {
        return m_pivots.OrderByDescending(t => LeftDistanceOf(m_refOrientationPoint, t.transform.position)).First();
    }
    private RubikCubePivot GetUpPivot(Transform m_refOrientationPoint)
    {
        return m_pivots.OrderByDescending(t => UpDistanceOf(m_refOrientationPoint, t.transform.position)).First();
    }
    private RubikCubePivot GetDownPivot(Transform m_refOrientationPoint)
    {
        return m_pivots.OrderBy(t => UpDistanceOf(m_refOrientationPoint, t.transform.position)).First();
    }
    private RubikCubePivot GetFacePivot(Transform m_refOrientationPoint)
    {
        return m_pivots.OrderBy(t => FaceDistanceOf(m_refOrientationPoint, t.transform.position)).First();
    }
    private RubikCubePivot GetBackPivot(Transform m_refOrientationPoint)
    {
        return m_pivots.OrderByDescending(t => FaceDistanceOf(m_refOrientationPoint, t.transform.position)).First();
    }
    private RubikCubePivot GetEquatorPivot(Transform m_refOrientationPoint)
    {
        return m_pivots.Where(t => t.m_isAtCenter).OrderByDescending(t => EquatorDistanceOf(m_refOrientationPoint, t.transform.up)).First();
    }
    private RubikCubePivot GetStandingPivot(Transform m_refOrientationPoint)
    {
        return m_pivots.Where(t => t.m_isAtCenter).OrderByDescending(t => StandingDistanceOf(m_refOrientationPoint, t.transform.up)).First();
    }
    private RubikCubePivot GetMiddlePivot(Transform m_refOrientationPoint)
    {
        return m_pivots.Where(t => t.m_isAtCenter).OrderByDescending(t => MiddleDistanceOf(m_refOrientationPoint, t.transform.up)).First();
    }
    #endregion

    #region WAY TO GET THE PIVOT
    /// COULD BE MUSH IMPROVED
    private object LeftDistanceOf(Transform m_refOrientationPoint, Vector3 position)
    {
        Vector3 localPosition = m_refOrientationPoint.InverseTransformPoint(position);
        return localPosition.x;
    }
    private object UpDistanceOf(Transform m_refOrientationPoint, Vector3 position)
    {
        Vector3 localPosition = m_refOrientationPoint.InverseTransformPoint(position);
        return localPosition.y;
    }
    private object FaceDistanceOf(Transform m_refOrientationPoint, Vector3 position)
    {
        Vector3 localPosition = m_refOrientationPoint.InverseTransformPoint(position);
        return localPosition.z;
    }
    private float EquatorDistanceOf(Transform m_refOrientationPoint, Vector3 upDirection)
    {
        Vector3 localDirection = m_refOrientationPoint.InverseTransformDirection(upDirection);
        return Mathf.Abs(localDirection.y);
    }
    private float StandingDistanceOf(Transform m_refOrientationPoint, Vector3 upDirection)
    {
        Vector3 localDirection = m_refOrientationPoint.InverseTransformDirection(upDirection);
        return Mathf.Abs(localDirection.z);
    }


    private float MiddleDistanceOf(Transform m_refOrientationPoint, Vector3 upDirection)
    {
        Vector3 localDirection = m_refOrientationPoint.InverseTransformDirection(upDirection);
        return Mathf.Abs(localDirection.x);
    }


    #endregion

    #endregion



















    public Vector3 GetCartesianPosition(RubikCubePivot pivot, ref Transform orientation)
    {
        return orientation.InverseTransformPoint(pivot.m_root.position);
    }

    public RubikCubePivotable GetRealFaceOf(RubikCubePivotable faceChosen, ref Transform orientation)
    {
        return GetPivotFrom(faceChosen, orientation).m_face;
    }

    internal static RotationTypeShort GetInvertOf(RotationTypeShort rotationTypeShort)
    {
        switch (rotationTypeShort)
        {
            case RotationTypeShort.L: return RotationTypeShort.Lp;
            case RotationTypeShort.Lp: return RotationTypeShort.L;
            case RotationTypeShort.R: return RotationTypeShort.Rp;
            case RotationTypeShort.Rp: return RotationTypeShort.R;
            case RotationTypeShort.U: return RotationTypeShort.Up;
            case RotationTypeShort.Up: return RotationTypeShort.U;
            case RotationTypeShort.D: return RotationTypeShort.Dp;
            case RotationTypeShort.Dp: return RotationTypeShort.D;
            case RotationTypeShort.F: return RotationTypeShort.Fp;
            case RotationTypeShort.Fp: return RotationTypeShort.F;
            case RotationTypeShort.B: return RotationTypeShort.Bp;
            case RotationTypeShort.Bp: return RotationTypeShort.B;
            case RotationTypeShort.M: return RotationTypeShort.Mp;
            case RotationTypeShort.Mp: return RotationTypeShort.M;
            case RotationTypeShort.E: return RotationTypeShort.Ep;
            case RotationTypeShort.Ep: return RotationTypeShort.E;
            case RotationTypeShort.S: return RotationTypeShort.Sp;
            case RotationTypeShort.Sp: return RotationTypeShort.S;
            default:
                throw new Exception("Shout not be possible");
        }
    }

    internal static RubikCubeColor GetColor(RubikCubePivotable pivot, out bool iSNotDefined)
    {
        iSNotDefined = false;
        switch (pivot)
        {
            case RubikCubePivotable.Left: return RubikCubeColor.White;
            case RubikCubePivotable.Right: return RubikCubeColor.Yellow;
            case RubikCubePivotable.Up: return RubikCubeColor.Red;
            case RubikCubePivotable.Down: return RubikCubeColor.Orange;
            case RubikCubePivotable.Face: return RubikCubeColor.Green;
            case RubikCubePivotable.Back: return RubikCubeColor.Blue;
        }
        iSNotDefined = true;
        return RubikCubeColor.White;
    }
    internal static RubikCubeColor GetColor(RubikCubeFace face)
    {
        switch (face)
        {
            case RubikCubeFace.Left: return RubikCubeColor.White;
            case RubikCubeFace.Right: return RubikCubeColor.Yellow;
            case RubikCubeFace.Up: return RubikCubeColor.Red;
            case RubikCubeFace.Down: return RubikCubeColor.Orange;
            case RubikCubeFace.Face: return RubikCubeColor.Green;
            case RubikCubeFace.Back: return RubikCubeColor.Blue;
            default:
                throw new Exception("No color define");
        }
    }

    public static Color GetColor(RubikCubeColor colorType) {
        switch (colorType)
        {
            case RubikCubeColor.White: return new Color(1, 1, 1);
            case RubikCubeColor.Red: return new Color(1, 0, 0);
            case RubikCubeColor.Green: return new Color(0, 1, 0);
            case RubikCubeColor.Blue: return new Color(0, 0, 1);
            case RubikCubeColor.Orange: return new Color(1f, 165f / 255f, 0);
            case RubikCubeColor.Yellow: return new Color(1f, 1, 0);
            default:
                throw new Exception("No color define");
        }
    }

    public static RubikCubeFace ConvertPivotToFace(RubikCubePivotable pivot)
    {
        switch (pivot)
        {
            case RubikCubePivotable.Left: return RubikCubeFace.Left;
            case RubikCubePivotable.Right: return RubikCubeFace.Right;
            case RubikCubePivotable.Up: return RubikCubeFace.Up;
            case RubikCubePivotable.Down: return RubikCubeFace.Down;
            case RubikCubePivotable.Face: return RubikCubeFace.Face;
            case RubikCubePivotable.Back: return RubikCubeFace.Back;
            case RubikCubePivotable.Middle:
            case RubikCubePivotable.Equator:
            case RubikCubePivotable.Standing:
            default:
                throw new Exception("Can convert to pivot");
        }
    }
    public static RubikCubePivotable ConvertFaceToPivot(RubikCubeFace pivot)
    {
        switch (pivot)
        {
            case RubikCubeFace.Left: return RubikCubePivotable.Left;
            case RubikCubeFace.Right: return RubikCubePivotable.Right;
            case RubikCubeFace.Up: return RubikCubePivotable.Up;
            case RubikCubeFace.Down: return RubikCubePivotable.Down;
            case RubikCubeFace.Face: return RubikCubePivotable.Face;
            case RubikCubeFace.Back: return RubikCubePivotable.Back;
            default:
                throw new Exception("Can convert to pivot");
        }
    }
    internal Color GetRgbColorFromLocal(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        TagRubikCubeFace selectedFace = GetMovingFaceFromLocal(face, direction);
        if (selectedFace)
            return GetColor(selectedFace.GetColorEnum());
        return Color.black;
    }

    internal TagRubikCubeFace GetMovingFaceFromLocal(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        RubikCubeFace currentFace;
        RubikCubeFaceDirection currentDirection;
        m_cubeFaceDirectionState.GetRealPieceFaceInfoAt(face, direction, out currentFace, out currentDirection);
        return m_movingCube.GetFace(currentFace, currentDirection);

    }


    private TagRubikCubeFace GetMovingFaceAt(Vector3 position)
    {
        return m_movingCube.m_faces.OrderBy(k => Vector3.Distance(k.m_root.position, position)).First();

    }

    #endregion





    public Vector3 GetDirectionFromView(Vector3 position, Transform viewPosition)
    {

        Vector3 objPosition = viewPosition.InverseTransformPoint(position);
        Vector3 rubPosition = viewPosition.InverseTransformPoint(m_root.position);
        return (objPosition - rubPosition).normalized;
    }
    public void RotateFaceFrom(ArrowDirection direction, TagRubikCubeFace movingFace, Transform from)
    {
        /////////////HERE///////
        List<RubikCubePivotable> implyFaces = movingFace.GetLinkedFaces().ToList();
        List<RubikCubePivot> pivots = new List<RubikCubePivot>();
        foreach (RubikCubePivotable f in implyFaces)
        { pivots.Add(GetPivot(f)); }

        RubikCubePivot rotationToApply = null;
        bool rotationClockwise = true;

        if (direction == ArrowDirection.Left || direction == ArrowDirection.Right)
        {
            rotationClockwise = direction == ArrowDirection.Left;
            RubikCubePivot up = GetUpPivot(from);
            RubikCubePivot equator = GetEquatorPivot(from);
            RubikCubePivot down = GetDownPivot(from);
            RubikCubePivotable correspondingPivot = RubikCubePivotable.Up;


            if (pivots.Contains(up)) {
                rotationToApply = up;
                correspondingPivot = RubikCubePivotable.Up;
            }
            if (pivots.Contains(equator)) {
                rotationToApply = equator;
                correspondingPivot = RubikCubePivotable.Equator;
            }
            if (pivots.Contains(down)) {
                rotationToApply = down;
                correspondingPivot = RubikCubePivotable.Down;
            }
            rotationClockwise = GetClockWiseFrom(from, rotationToApply, correspondingPivot, direction);
        }

        if (direction == ArrowDirection.Up || direction == ArrowDirection.Down)
        {
            rotationClockwise = direction == ArrowDirection.Up;
            RubikCubePivot left = GetLeftPivot(from);
            RubikCubePivot middle = GetMiddlePivot(from);
            RubikCubePivot right = GetRightPivot(from);
            RubikCubePivotable correspondingPivot = RubikCubePivotable.Left;

            if (pivots.Contains(left)) {
                rotationToApply = left;
                correspondingPivot = RubikCubePivotable.Left;
            }
            if (pivots.Contains(middle)) {
                rotationToApply = middle;
                correspondingPivot = RubikCubePivotable.Middle;
            }
            if (pivots.Contains(right)) {
                rotationToApply = right;
                correspondingPivot = RubikCubePivotable.Right;
            }
            rotationClockwise = GetClockWiseFrom(from, rotationToApply, correspondingPivot, direction);
        }

        //        Debug.Log(string.Format("Pivot {0}: {1} ({2})", direction, rotationToApply, rotationClockwise),rotationToApply);

        AddLocalRotate(rotationToApply.m_face, rotationClockwise);
    }

    private bool GetClockWiseFrom(Transform from, RubikCubePivot pivot, RubikCubePivotable pivotrepresented, ArrowDirection direction)
    {
        Vector3 localDirection = from.InverseTransformDirection(pivot.m_root.up);
        Vector3 localDirectionOfPivot = Vector3.zero;
        bool clockwiseDirection = true;

        switch (pivotrepresented)
        {
            case RubikCubePivotable.Left:
                localDirectionOfPivot = Vector3.left;
                break;
            case RubikCubePivotable.Right:
                localDirectionOfPivot = Vector3.right;
                break;
            case RubikCubePivotable.Up:
                localDirectionOfPivot = Vector3.up;
                break;
            case RubikCubePivotable.Down:
                localDirectionOfPivot = Vector3.down;
                break;
            case RubikCubePivotable.Face:
                localDirectionOfPivot = Vector3.back;
                break;
            case RubikCubePivotable.Back:
                localDirectionOfPivot = Vector3.forward;
                break;
            case RubikCubePivotable.Middle:
                localDirectionOfPivot = Vector3.left;
                break;
            case RubikCubePivotable.Equator:
                localDirectionOfPivot = Vector3.down;
                break;
            case RubikCubePivotable.Standing:
                localDirectionOfPivot = Vector3.back;
                break;
            default:
                break;
        }

        if (pivotrepresented == RubikCubePivotable.Up && direction == ArrowDirection.Left) clockwiseDirection = true;
        if (pivotrepresented == RubikCubePivotable.Up && direction == ArrowDirection.Right) clockwiseDirection = false;
        if (pivotrepresented == RubikCubePivotable.Down && direction == ArrowDirection.Left) clockwiseDirection = false;
        if (pivotrepresented == RubikCubePivotable.Down && direction == ArrowDirection.Right) clockwiseDirection = true;


        if (pivotrepresented == RubikCubePivotable.Left && direction == ArrowDirection.Up) clockwiseDirection = false;
        if (pivotrepresented == RubikCubePivotable.Left && direction == ArrowDirection.Down) clockwiseDirection = true;
        if (pivotrepresented == RubikCubePivotable.Right && direction == ArrowDirection.Up) clockwiseDirection = true;
        if (pivotrepresented == RubikCubePivotable.Right && direction == ArrowDirection.Down) clockwiseDirection = false;


        if (pivotrepresented == RubikCubePivotable.Equator && direction == ArrowDirection.Left) clockwiseDirection = false;
        if (pivotrepresented == RubikCubePivotable.Equator && direction == ArrowDirection.Right) clockwiseDirection = true;

        if (pivotrepresented == RubikCubePivotable.Middle && direction == ArrowDirection.Up) clockwiseDirection = false;
        if (pivotrepresented == RubikCubePivotable.Middle && direction == ArrowDirection.Down) clockwiseDirection = true;




        float angle = Vector3.Angle(localDirectionOfPivot, localDirection);


        Debug.DrawRay(Vector3.zero, localDirection * 5, Color.red, 15);
        bool clockwise = angle < 90f;

        return clockwiseDirection ? clockwise : !clockwise;

    }




    private bool IsMiddlePivot(RubikCubePivotable pivot)
    {
        return pivot == RubikCubePivotable.Equator || pivot == RubikCubePivotable.Standing || pivot == RubikCubePivotable.Middle;
    }
    private bool IsMiddlePivot(RubikCubePivot pivot)
    {
        return IsMiddlePivot(pivot.m_face);
    }

    private RubikCubePivot GetPivot(RubikCubePivotable pivot)
    {
        return m_pivots.Where(k => k.m_face == pivot).First();
    }

    private List<RubikCubePivotable> ExclusiveFaces(RubikCubePivotable face, List<RubikCubePivotable> implyFaces)
    {
        implyFaces.Remove(face);
        return implyFaces;
    }
    private List<RubikCubePivotable> ExclusiveLateral(List<RubikCubePivotable> implyFaces)
    {

        return
         implyFaces.Where(k => GetRotationType(k) != RotationType.Lateral).ToList();
    }
    private RubikCubePivotable GetRotationTypeFromPivtos(RotationType rotationType, List<RubikCubePivotable> implyFaces)
    {

        return implyFaces.Where(k => GetRotationType(k) == rotationType).First();
    }





    public RotationType GetRotationType(RubikCubePivotable pivot) {
        switch (pivot)
        {
            case RubikCubePivotable.Left: return RotationType.Vertical;
            case RubikCubePivotable.Right: return RotationType.Vertical;
            case RubikCubePivotable.Up: return RotationType.Horizontal;
            case RubikCubePivotable.Down: return RotationType.Horizontal;

            case RubikCubePivotable.Middle: return RotationType.Vertical;
            case RubikCubePivotable.Equator: return RotationType.Horizontal;
            case RubikCubePivotable.Standing: return RotationType.Lateral;
            case RubikCubePivotable.Face: return RotationType.Lateral;
            case RubikCubePivotable.Back: return RotationType.Lateral;
            default:
                throw new Exception("Humm ... ?");
        }

    }

    public void GetCenter(out Vector3 position, out Quaternion rotation) {
        position = m_root.position;
        rotation = m_root.rotation;
    }
    public Transform GetRoot() { return m_root; }

    //public Transform GetPieceDefinedBy(RubikCubeFace face, RubikCubeFaceDirection direction)
    //{

    //}
    //public Transform GetPieceAtPosition(RubikCubeFace face, RubikCubeFaceDirection direction)
    //{

    //}
    //public Transform GetPieceAtPositionFromPointOfView(RubikCubeFace face, RubikCubeFaceDirection direction)
    //{

    //}
    public static Color m_white = Color.white;
    public static Color m_green = Color.green;
    public static Color m_yellow = Color.yellow;
    public static Color m_orange = new Color(255f / 255f, 165f / 255f, 0f);
    public static Color m_blue = Color.blue;
    public static Color m_red = Color.red;

    public static Color GetDefaultColorOfCube(RubikCubeColor color) {
        switch (color)
        {
            case RubikCubeColor.White:
                return m_white;
            case RubikCubeColor.Red:
                return m_red;
            case RubikCubeColor.Green:
                return m_green;
            case RubikCubeColor.Blue:
                return m_blue;
            case RubikCubeColor.Orange:
                return m_orange;
            case RubikCubeColor.Yellow:
                return m_yellow;
            default:
                break;
        }
        return Color.black;
    }


    public static RubikCubeColor GetDefaultColor(RubikCubeFace face)
    {
        switch (face)
        {
            case RubikCubeFace.Left:
                return RubikCubeColor.White;
            case RubikCubeFace.Right:
                return RubikCubeColor.Yellow;
            case RubikCubeFace.Up:
                return RubikCubeColor.Red;
            case RubikCubeFace.Down:
                return RubikCubeColor.Orange;
            case RubikCubeFace.Face:
                return RubikCubeColor.Green;
            case RubikCubeFace.Back:
                return RubikCubeColor.Blue;
            default:
                break;
        }
        return RubikCubeColor.White;
    }



    public TagRubikCubePiece FindCubeByColors(params RubikCubeColor[] colors)
    {
        TagRubikCubePiece[] spot = m_fixedCube.m_pieces;
        for (int i = 0; i < spot.Length; i++)
        {
            RubikCubePivotable[] pivots = spot[i].GetPivots();
            if (pivots.Length == colors.Length)
            {
                bool allFaceIsIn = true;
                for (int j = 0; j < pivots.Length; j++)
                {
                    bool isDefined;
                    RubikCubeColor col = GetColor(pivots[j], out isDefined);
                    if (isDefined && !colors.Contains(col))
                        allFaceIsIn = false;
                }
                if (allFaceIsIn)
                    return spot[i];

            }
        }
        return null;
    }
    public List<TagRubikCubePiece> FindAnyCubesWithColor(RubikCubeColor color)
    {
        List<TagRubikCubePiece> result = new List<TagRubikCubePiece>();
        TagRubikCubePiece[] spot = m_movingCube.m_pieces;
        for (int i = 0; i < spot.Length; i++)
        {
            RubikCubePivotable[] pivots = spot[i].GetPivots();
            for (int j = 0; j < pivots.Length; j++)
            {
                bool isDefined;
                RubikCubeColor col = GetColor(pivots[j], out isDefined);
                if (isDefined && col == color)
                    result.Add(spot[i]);
            }
        }
        return result;
    }


    public TagRubikCubePiece GetClosestPiece(Vector3 position)
    {
        return m_fixedCube.m_pieces.OrderBy(k => Vector3.Distance(k.m_root.position, position)).First();
    }
    public RubikCubePivot GetClosestPivot(Vector3 position)
    {
        return m_pivots.OrderBy(k => Vector3.Distance(k.m_root.position, position)).First();
    }
    public static string ConvertShortToString(string separation, IEnumerable<RotationTypeShort> seq)
    {
        string sequence = "";
        bool first = true;
        foreach (RotationTypeShort rotation in seq)
        {
            string r = RubikCube.ConvertAcronymShortToString(rotation);
            if (first)
            {
                first = false;
                sequence += r;
            }
            else {
                sequence += separation + r;
            }
        }
        return sequence;
    }


    public CubeDirectionalState GetCubeState() {

        return m_cubeFaceDirectionState;
    }

    internal static RotationTypeShort[] ConvertStringToShorts(string sequence)
    {
        List<RotationTypeShort> rotations = new List<RotationTypeShort>();

        string[] seg = sequence.Split(new char[] { ' ', ':', ';', ',' });
        foreach (string str in seg)
        {

            RotationTypeShort rotationType;
            if (ConvertStringToShortAcronym(str, out rotationType)) {
                rotations.Add(rotationType);
            }

        }
        return rotations.ToArray();

    }


    public static RotationSequence GetRandomSequence(int randomIteration) {
        return new RotationSequence(GetRandomShortSequence(randomIteration));
    }

    internal static IEnumerable<RotationTypeShort> GetRandomShortSequence(int randomIteration)
    {
        List<RotationTypeShort> rotations = new List<RotationTypeShort>();
        for (int i = 0; i < randomIteration; i++)
            rotations.Add(GetRandomShort());
        return rotations;
    }

    internal void AddLocalRotationSequence(RotationSequence sequence)
    {
        foreach (RotationTypeShort shortRotation in sequence.GetSequenceAsShort())
        {
            AddLocalRotate(shortRotation);
        }
    }

    internal static bool GetFaceInfoInString(string text, out RubikCubeFace face)
    {
        face = RubikCubeFace.Back;
        text = text.ToLower();
        foreach (RubikCubeFace f in GetListOf<RubikCubeFace>())
        {
            if (text.IndexOf(f.ToString().ToLower()) > -1)
            {
                face = f;
                return true;
            }
        }
        return false;
    }

    internal static bool GetDirectionInfoInString(string text, out RubikCubeFaceDirection direction)
    {
        direction = RubikCubeFaceDirection.C;
        text = text.ToLower();
        foreach (RubikCubeFaceDirection f in GetListOf<RubikCubeFaceDirection>())
        {
            if (text.IndexOf(f.ToString().ToLower()) > -1)
            {
                direction = f;
                return true;
            }
        }
        return false;
    }
    internal static bool GetPositionInfoInString(string text, out RubikPieceByPosition3D position)
    {
        position = RubikPieceByPosition3D.X0_Y0_Z0;
        text = text.ToLower();
        foreach (RubikPieceByPosition3D f in GetListOf<RubikPieceByPosition3D>())
        {
            if (text.IndexOf(f.ToString().ToLower()) > -1)
            {
                position = f;
                return true;
            }
        }
        return false;
    }
    public static bool GetPositionInfoInString(string text, out RubikPiecePositionByPivot position)
    {
        position = RubikPiecePositionByPivot.M_E_S;
        text = text.ToLower();
        foreach (RubikPiecePositionByPivot f in GetListOf<RubikPiecePositionByPivot>())
        {
            if (text.IndexOf(f.ToString().ToLower()) > -1)
            {
                position = f;
                return true;
            }
        }
        return false;

    }



    public static string GetTagOf(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        return face.ToString() + "|" + direction.ToString();

    }
    public static string GetTagOf(RubikPieceByPosition3D position)
    {
        return position.ToString();
    }
    public static string GetTagOf(RubikPiecePositionByPivot position)
    {
        return position.ToString();
    }
    public static string GetTagOf(RubikCubePivotable[] pivot)
    {
        string s = "";
        for (int i = 0; i < pivot.Length; i++)
        {
            s += (i == 0 ? "" : "|") + pivot[i].ToString();
        }
        return s;
    }


    public static RubikPieceByPosition3D GetPiecePositionBasedOn(RubikCubeDepth depth, RubikCubeFaceDirection direction)
    {

        int x = 1;
        int y = 1;
        int z = (int)depth;
        switch (direction)
        {
            case RubikCubeFaceDirection.SO: x = 0; y = 0; break;
            case RubikCubeFaceDirection.S: x = 1; y = 0; break;
            case RubikCubeFaceDirection.SE: x = 2; y = 0; break;
            case RubikCubeFaceDirection.O: x = 0; y = 1; break;
            case RubikCubeFaceDirection.C: x = 1; y = 1; break;
            case RubikCubeFaceDirection.E: x = 2; y = 1; break;
            case RubikCubeFaceDirection.NO: x = 0; y = 2; break;
            case RubikCubeFaceDirection.N: x = 1; y = 2; break;
            case RubikCubeFaceDirection.NE: x = 2; y = 2; break;
            default:
                break;
        }
        return GetPieceOf(x, y, z).GetPosition3D();
    }

    #region NOT IMPLEMENTED YET
    public static RubikPiecePositionByPivot GetPiecePositionBasedOn(RubikCubeFace face, RubikCubeFaceDirection direction) {
        throw new System.NotImplementedException();
    }

    internal static RubikPiecePositionByPivot GetPiecePositionBasedOn(RubikCubePivotable[] pivots)
    {
        throw new NotImplementedException();
    }
    
    public static RubikCubePivotable[] GetPivotsFrom(RubikPiecePositionByPivot position)
    {
        throw new NotImplementedException();

    }
    #endregion

    #region CUBE DIRECTIONAL STATE
    public static CubeDirectionalState CreateCubeStateFrom(RotationSequence rotationSequence)
    {

        if (m_fakeCubeInBackground) {
            m_fakeCubeInBackground.ResetInitialState();
            m_fakeCubeInBackground.AddLocalRotationSequence(rotationSequence);
            m_fakeCubeInBackground.FinishMotorQueuedRotation();
            return m_fakeCubeInBackground.GetCubeState();

        }
        return null;
    }
    #endregion

    public class RubikCubePieceIndex
    {


        private static RubikCubePieceIndex[] m_indexTable = new RubikCubePieceIndex[27];
        private static RubikCubePieceIndex[,,] m_indexmatrice = new RubikCubePieceIndex[3, 3, 3];
        private static RubikPieceByPosition3D[,,] m_positionMatrice = new RubikPieceByPosition3D[3, 3, 3];
        public int m_index;
        public RubikPieceByPosition3D m_positionByVector3;
        public int m_x;
        public int m_y;
        public int m_z;
        public RubikPiecePositionByPivot m_positionByPivots;
        private RubikCubePivotable m_vertical;
        private RubikCubePivotable m_horizontal;
        private RubikCubePivotable m_standing;

        public RubikCubePieceIndex(RubikPieceByPosition3D position3D, int x, int y, int z, RubikPiecePositionByPivot positionPivot, RubikCubePivotable vertical, RubikCubePivotable horizontal, RubikCubePivotable standing)
        {
            this.m_index = (int)position3D;
            this.m_positionByVector3 = position3D;
            this.m_x = x;
            this.m_y = y;
            this.m_z = z;
            this.m_positionByPivots = positionPivot;
            this.m_vertical = vertical;
            this.m_horizontal = horizontal;
            this.m_standing = standing;
            m_indexTable[m_index] = this;
            m_indexmatrice[x, y, z] = this;
            m_positionMatrice[x, y, z] = position3D;
        }

        public static RubikCubePieceIndex GetIndex(int x, int y, int z)
        {
            if (x <= 0) x = 0;
            if (y <= 0) y = 0;
            if (z <= 0) z = 0;
            if (x > 2) x = 2;
            if (y > 2) y = 2;
            if (z > 2) z = 2;
            return m_indexmatrice[x, y, z];

        }
        public static RubikPieceByPosition3D GetPosition3D(int x, int y, int z)
        {
            if (x <= 0) x = 0;
            if (y <= 0) y = 0;
            if (z <= 0) z = 0;
            if (x > 2) x = 2;
            if (y > 2) y = 2;
            if (z > 2) z = 2;
            return m_positionMatrice[x, y, z];

        }

        public RubikPieceByPosition3D GetPosition3D()
        {
            return m_positionByVector3;
        }

        internal RubikCubePivotable[] GetPivots()
        {
            return new RubikCubePivotable[] { m_horizontal, m_vertical, m_standing };
        }

        internal static RubikCubePieceIndex GetIndex(RubikPieceByPosition3D position3D)
        {
            return m_indexTable[(int)position3D];
        }

        internal static RubikCubePieceIndex GetPieceIndex(RubikPiecePositionByPivot pivot)
        {
            for (int i = 0; i < m_indexTable.Length; i++)
            {
                if (m_indexTable[i].m_positionByPivots == pivot)
                    return m_indexTable[i];
            }
            return null;
        }
    }
    [System.Serializable]

    public class RubikCubeFaceIndex {
        public RubikCubeFace m_face;
        public RubikCubeFaceDirection m_direction;
        public RubikPieceByPosition3D m_positionReference;
        public CrossVector m_crossPosition = new CrossVector();

        public RubikCubeFaceIndex(short crossX, short crossY, RubikPieceByPosition3D positionReference, RubikCubeFace face, RubikCubeFaceDirection direction)
        {
            this.m_positionReference = positionReference;
            this.m_face = face;
            this.m_direction = direction;
            this.m_crossPosition.x = crossX;
            this.m_crossPosition.y = crossY;
        }
        public class CrossVector
        {
            public short x, y;
        }
    }
    #region QUICK ACCESS TO STATIC DATA ON THE RUBIK CUBE
    public static RubikCubePieceIndex GetPieceOf(int x, int y, int z)
    {
        return RubikCubePieceIndex.GetIndex(x, y, z);
    }
    public static RubikCubePieceIndex GetPieceOf(RubikPieceByPosition3D position3D)
    {
        return RubikCubePieceIndex.GetIndex(position3D);
    }

    internal static RubikCubePieceIndex GetPieceOf(RubikPiecePositionByPivot pivot)
    {

        return RubikCubePieceIndex.GetPieceIndex(pivot);
    }
  

    public static RubikCubePieceIndex GetPieceOf(RubikCubeFace face, RubikCubeFaceDirection direction) {
        throw new NotImplementedException();
    }
    private static void GetIntPosition(RubikPieceByPosition3D position, out short x, out short y, out short z)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region INDEX

    static RubikCube (){
     FlyWeighInitialisation();
    }

    private static RubikCubeFaceIndex[] facesIndex = null;
    private static RubikCubePieceIndex[] piecesIndex = null;
    public static void FlyWeighInitialisation() {
        if (facesIndex == null)
            facesIndex = GetFaceIndex();
        if (piecesIndex == null)
            piecesIndex = GetPieceIndex();

    }

    private static RubikCubePieceIndex[] GetPieceIndex()
    {
        return new RubikCubePieceIndex[] {
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y0_Z0, 0,0,0,RubikPiecePositionByPivot.L_D_F ,RubikCubePivotable.Left  , RubikCubePivotable.Down , RubikCubePivotable.Face     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y0_Z0, 1,0,0,RubikPiecePositionByPivot.M_D_F ,RubikCubePivotable.Middle, RubikCubePivotable.Down , RubikCubePivotable.Face     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y0_Z0, 2,0,0,RubikPiecePositionByPivot.R_D_F ,RubikCubePivotable.Right , RubikCubePivotable.Down , RubikCubePivotable.Face     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y0_Z1, 0,0,1,RubikPiecePositionByPivot.L_D_S ,RubikCubePivotable.Left  , RubikCubePivotable.Down , RubikCubePivotable.Standing ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y0_Z1, 1,0,1,RubikPiecePositionByPivot.M_D_S ,RubikCubePivotable.Middle, RubikCubePivotable.Down , RubikCubePivotable.Standing ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y0_Z1, 2,0,1,RubikPiecePositionByPivot.R_D_S ,RubikCubePivotable.Right , RubikCubePivotable.Down , RubikCubePivotable.Standing ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y0_Z2, 0,0,2,RubikPiecePositionByPivot.L_D_B ,RubikCubePivotable.Left  , RubikCubePivotable.Down , RubikCubePivotable.Back     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y0_Z2, 1,0,2,RubikPiecePositionByPivot.M_D_B ,RubikCubePivotable.Middle, RubikCubePivotable.Down , RubikCubePivotable.Back     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y0_Z2, 2,0,2,RubikPiecePositionByPivot.R_D_B ,RubikCubePivotable.Right , RubikCubePivotable.Down , RubikCubePivotable.Back     ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y1_Z0, 0,1,0,RubikPiecePositionByPivot.L_E_F ,RubikCubePivotable.Left  , RubikCubePivotable.Equator , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y1_Z0, 1,1,0,RubikPiecePositionByPivot.M_E_F ,RubikCubePivotable.Middle, RubikCubePivotable.Equator , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y1_Z0, 2,1,0,RubikPiecePositionByPivot.R_E_F ,RubikCubePivotable.Right , RubikCubePivotable.Equator , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y1_Z1, 0,1,1,RubikPiecePositionByPivot.L_E_S ,RubikCubePivotable.Left  , RubikCubePivotable.Equator , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y1_Z1, 1,1,1,RubikPiecePositionByPivot.M_E_S ,RubikCubePivotable.Middle, RubikCubePivotable.Equator , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y1_Z1, 2,1,1,RubikPiecePositionByPivot.R_E_S ,RubikCubePivotable.Right , RubikCubePivotable.Equator , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y1_Z2, 0,1,2,RubikPiecePositionByPivot.L_E_B ,RubikCubePivotable.Left  , RubikCubePivotable.Equator , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y1_Z2, 1,1,2,RubikPiecePositionByPivot.M_E_B ,RubikCubePivotable.Middle, RubikCubePivotable.Equator , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y1_Z2, 2,1,2,RubikPiecePositionByPivot.R_E_B ,RubikCubePivotable.Right , RubikCubePivotable.Equator , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y2_Z0, 0,2,0,RubikPiecePositionByPivot.L_U_F ,RubikCubePivotable.Left  , RubikCubePivotable.Up , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y2_Z0, 1,2,0,RubikPiecePositionByPivot.M_U_F ,RubikCubePivotable.Middle, RubikCubePivotable.Up , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y2_Z0, 2,2,0,RubikPiecePositionByPivot.R_U_F ,RubikCubePivotable.Right , RubikCubePivotable.Up , RubikCubePivotable.Face      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y2_Z1, 0,2,1,RubikPiecePositionByPivot.L_U_S ,RubikCubePivotable.Left  , RubikCubePivotable.Up , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y2_Z1, 1,2,1,RubikPiecePositionByPivot.M_U_S ,RubikCubePivotable.Middle, RubikCubePivotable.Up , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y2_Z1, 2,2,1,RubikPiecePositionByPivot.R_U_S ,RubikCubePivotable.Right , RubikCubePivotable.Up , RubikCubePivotable.Standing  ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X0_Y2_Z2, 0,2,2,RubikPiecePositionByPivot.L_U_B ,RubikCubePivotable.Left  , RubikCubePivotable.Up , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X1_Y2_Z2, 1,2,2,RubikPiecePositionByPivot.M_U_B ,RubikCubePivotable.Middle, RubikCubePivotable.Up , RubikCubePivotable.Back      ),
        new RubikCubePieceIndex( RubikPieceByPosition3D.X2_Y2_Z2, 2,2,2,RubikPiecePositionByPivot.R_U_B ,RubikCubePivotable.Right , RubikCubePivotable.Up , RubikCubePivotable.Back      )

    };
}
    private static RubikCubeFaceIndex[] GetFaceIndex()
    {
        return new RubikCubeFaceIndex[] {
        new RubikCubeFaceIndex(0,8,RubikPieceByPosition3D.X0_Y2_Z2, RubikCubeFace.Left, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(1,8,RubikPieceByPosition3D.X0_Y2_Z1, RubikCubeFace.Left, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(2,8,RubikPieceByPosition3D.X0_Y2_Z0, RubikCubeFace.Left, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(0,7,RubikPieceByPosition3D.X0_Y1_Z2, RubikCubeFace.Left, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(1,7,RubikPieceByPosition3D.X0_Y1_Z1, RubikCubeFace.Left, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(2,7,RubikPieceByPosition3D.X0_Y1_Z0, RubikCubeFace.Left, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(0,6,RubikPieceByPosition3D.X0_Y0_Z2, RubikCubeFace.Left, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(1,6,RubikPieceByPosition3D.X0_Y0_Z1, RubikCubeFace.Left, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(2,6,RubikPieceByPosition3D.X0_Y0_Z0, RubikCubeFace.Left, RubikCubeFaceDirection.SE),

        new RubikCubeFaceIndex(3,8,RubikPieceByPosition3D.X0_Y2_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(4,8,RubikPieceByPosition3D.X1_Y2_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(5,8,RubikPieceByPosition3D.X2_Y2_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(3,7,RubikPieceByPosition3D.X0_Y1_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(4,7,RubikPieceByPosition3D.X1_Y1_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(5,7,RubikPieceByPosition3D.X2_Y1_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(3,6,RubikPieceByPosition3D.X0_Y0_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(4,6,RubikPieceByPosition3D.X1_Y0_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(5,6,RubikPieceByPosition3D.X2_Y0_Z0, RubikCubeFace.Face, RubikCubeFaceDirection.SE),

        new RubikCubeFaceIndex(6,8,RubikPieceByPosition3D.X2_Y2_Z0, RubikCubeFace.Right, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(7,8,RubikPieceByPosition3D.X2_Y2_Z1, RubikCubeFace.Right, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(8,8,RubikPieceByPosition3D.X2_Y2_Z2, RubikCubeFace.Right, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(6,7,RubikPieceByPosition3D.X2_Y1_Z0, RubikCubeFace.Right, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(7,7,RubikPieceByPosition3D.X2_Y1_Z1, RubikCubeFace.Right, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(8,7,RubikPieceByPosition3D.X2_Y1_Z2, RubikCubeFace.Right, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(6,6,RubikPieceByPosition3D.X2_Y0_Z0, RubikCubeFace.Right, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(7,6,RubikPieceByPosition3D.X2_Y0_Z1, RubikCubeFace.Right, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(8,6,RubikPieceByPosition3D.X2_Y0_Z2, RubikCubeFace.Right, RubikCubeFaceDirection.SE),


        new RubikCubeFaceIndex(3,11,RubikPieceByPosition3D.X0_Y2_Z2, RubikCubeFace.Up, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(4,11,RubikPieceByPosition3D.X1_Y2_Z2, RubikCubeFace.Up, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(5,11,RubikPieceByPosition3D.X2_Y2_Z2, RubikCubeFace.Up, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(3,10,RubikPieceByPosition3D.X0_Y2_Z1, RubikCubeFace.Up, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(4,10,RubikPieceByPosition3D.X1_Y2_Z1, RubikCubeFace.Up, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(5,10,RubikPieceByPosition3D.X2_Y2_Z1, RubikCubeFace.Up, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(3,9 ,RubikPieceByPosition3D.X0_Y2_Z0, RubikCubeFace.Up, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(4,9 ,RubikPieceByPosition3D.X1_Y2_Z0, RubikCubeFace.Up, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(5,9 ,RubikPieceByPosition3D.X2_Y2_Z0, RubikCubeFace.Up, RubikCubeFaceDirection.SE),

        new RubikCubeFaceIndex(3,5,RubikPieceByPosition3D.X0_Y0_Z0, RubikCubeFace.Down, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(4,5,RubikPieceByPosition3D.X1_Y0_Z0, RubikCubeFace.Down, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(5,5,RubikPieceByPosition3D.X2_Y0_Z0, RubikCubeFace.Down, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(3,4,RubikPieceByPosition3D.X0_Y0_Z1, RubikCubeFace.Down, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(4,4,RubikPieceByPosition3D.X1_Y0_Z1, RubikCubeFace.Down, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(5,4,RubikPieceByPosition3D.X2_Y0_Z1, RubikCubeFace.Down, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(3,3,RubikPieceByPosition3D.X0_Y0_Z2, RubikCubeFace.Down, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(4,3,RubikPieceByPosition3D.X1_Y0_Z2, RubikCubeFace.Down, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(5,3,RubikPieceByPosition3D.X2_Y0_Z2, RubikCubeFace.Down, RubikCubeFaceDirection.SE),

        new RubikCubeFaceIndex(5,0,RubikPieceByPosition3D.X2_Y2_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.NO),
        new RubikCubeFaceIndex(4,0,RubikPieceByPosition3D.X1_Y2_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.N),
        new RubikCubeFaceIndex(3,0,RubikPieceByPosition3D.X0_Y2_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.NE),
        new RubikCubeFaceIndex(5,1,RubikPieceByPosition3D.X2_Y1_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.O),
        new RubikCubeFaceIndex(4,1,RubikPieceByPosition3D.X1_Y1_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.C),
        new RubikCubeFaceIndex(3,1,RubikPieceByPosition3D.X0_Y1_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.E),
        new RubikCubeFaceIndex(5,2,RubikPieceByPosition3D.X2_Y0_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.SO),
        new RubikCubeFaceIndex(4,2,RubikPieceByPosition3D.X1_Y0_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.S),
        new RubikCubeFaceIndex(3,2,RubikPieceByPosition3D.X0_Y0_Z2, RubikCubeFace.Back, RubikCubeFaceDirection.SE)

    };
    }
    #endregion

    

   
    public static RubikCube m_fakeCubeInBackground;


}


#region RUBIK CUBE EVENT
[System.Serializable]
public class OnRubikCubeUsed : UnityEvent<RubikCube> { }
#endregion

