using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;




//Info https://ruwix.com/the-rubiks-cube/fingertricks/

public interface IRubikCube {



}
/// https://ruwix.com/the-rubiks-cube/notation/
public enum RotationTypeShort
{
    L, Lp,
    R, Rp,
    U, Up,
    D, Dp,
    F, Fp,
    B, Bp,
    M, Mp,
    E, Ep,
    S, Sp,
    X, Xp,
    Y, Yp,
    Z, Zp
}
public enum RotationTypeLong
{
    Left, LeftCounter,
    Right, RightCounter,
    Up, UpCounter,
    Down, DownCounter,
    Face, FaceCounter,
    Back, BackCounter,
    Middle, MiddleCounter,
    Equator, EquatorCounter,
    Standing, StandingCounter,
    X, XCounter,
    Y, YCounter,
    Z, ZCounter
}
public enum RubikCubePivotable:int
{
    Left,
    Right,
    Up,
    Down,
    Face,
    Back,
    Middle,
    Equator,
    Standing

}
public enum RubikCubeFace : int
{
    Left,
    Right,
    Up,
    Down,
    Face,
    Back

}







public enum RubikCubeFaceDirection : int {

    SO = 1,
    S = 2,
    SE = 3,
    O = 4,
    C = 5,
    E = 6,
    NO = 7,
    N = 8,
    NE = 9
}

public enum RubikPieceType :int {

    X2_Y2_Z2 = -1,
    X1_Y1_Z1 = 0,
    X1_Y1_Z2 = 1,
    X1_Y1_Z3 = 2,
    X1_Y2_Z1 = 3,
    X1_Y2_Z2 = 4,
    X1_Y2_Z3 = 5,
    X1_Y3_Z1 = 6,
    X1_Y3_Z2 = 7,
    X1_Y3_Z3 = 8,

    X2_Y1_Z1 = 9,
    X2_Y1_Z2 = 10,
    X2_Y1_Z3 = 11,
    X2_Y2_Z1 = 12,
    X2_Y2_Z3 = 13,
    X2_Y3_Z1 = 14,
    X2_Y3_Z2 = 15,
    X2_Y3_Z3 = 16,

    X3_Y1_Z1 = 17,
    X3_Y1_Z2 = 18,
    X3_Y1_Z3 = 19,
    X3_Y2_Z1 = 20,
    X3_Y2_Z2 = 21,
    X3_Y2_Z3 = 22,
    X3_Y3_Z1 = 23,
    X3_Y3_Z2 = 24,
    X3_Y3_Z3 = 25,
}

public enum RubikCubeColor {
    White, Red, Green, Blue, Orange, Yellow

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

    public RubikCubeSpot[] m_piecesSpots;
    public Dictionary<RubikCubePivotable, List<RubikCubeSpot>> m_faces = new Dictionary<RubikCubePivotable, List<RubikCubeSpot>>();


    public RubikPiece[] m_pieces;

    public RubikCubeFaceInfo[] m_originalfaces;
    public RubikCubeFaceInfo[] m_piecesfaces;
    public RubikCubeRotateMotor m_rotationMotor;

    void Awake() {

        foreach (RubikCubeSpot spot in m_piecesSpots)
        {
            foreach (RubikCubePivotable face in spot.m_faces)
            {
                AddSpotToToRegister(face, spot);
            }

        }
        SaveInitialState();

    }



    public void DebugDisplayFace(RubikCubePivotable face, float time, Color color)
    {
        List<RubikCubeSpot> spots = GetSpots(face);
        foreach (RubikCubeSpot spot in spots)
        {
            DebugUtility.DrawCross(spot.m_root, 0.2f, color, time);

        }
    }

    private List<RubikCubeSpot> GetSpots(RubikCubePivotable face)
    {
        return m_faces[face];
    }

    private void AddSpotToToRegister(RubikCubePivotable face, RubikCubeSpot spot)
    {
        if (!m_faces.ContainsKey(face))
            m_faces.Add(face, new List<RubikCubeSpot>());
        m_faces[face].Add(spot);
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

    internal RubikPiece[] GetPieces(RubikCubePivotable face)
    {
        List<RubikCubeSpot> spots = GetSpots(face);
        List<RubikPiece> pieces = new List<RubikPiece>();
        foreach (RubikCubeSpot spot in spots)
        {
            RubikPiece piece = GetClosestPieceOf(spot.m_root);
            pieces.Add(piece);
        }
        return pieces.ToArray();
    }

    private RubikPiece GetClosestPieceOf(Transform m_root)
    {
        RubikPiece closest = null;
        float smallestdistance = float.MaxValue;
        foreach (RubikPiece piece in m_pieces)
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

    public void AddLocalRotationSequence(string sequence)
    {
        AddRotationSequence(sequence, null);
    }
    public void AddRotationSequenceWithDefaultCamera(string sequence)
    {
        AddRotationSequence(sequence, GetDefaultOrientation());
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
                    LocalRotate(segment);
                else
                    RotateFaceFrom(segment, orientation);
            }
            catch (Exception) { }
        }
    }
    public void LocalRotate(string rotation) {
        RubikCubePivotable face;
        bool clockWise;
        RotationTypeShort rotationType = ConvertStringToShortAcronym(rotation);
        ConvertAcronymToFaceRotation(rotationType, out face, out clockWise);
        LocalRotate(face, clockWise);
    }
    public void LocalRotate(RubikCubePivotable faceToTurn, bool clockWise)
    {
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

    [System.Serializable]
    public class RotationEvent : UnityEvent<LocalRotationRequest> {

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

    #endregion

    #region Solution LISTENER

    public RubikCubeResolvedState m_cubeResolvedState;
    public UnityEvent m_onCubeResolved;
    public bool IsCubeResolved()
    {
        float pct;
        return IsCubeResolved(out pct);
    }
    public bool IsCubeResolved(out float pourcent)
    {
        float pct = 0;
        bool resolved = m_cubeResolvedState.IsResolved(out pct);
        pourcent = pct;
        return resolved;
    }

    public void NotifyCubeAsResolved() {
        m_onCubeResolved.Invoke();
    }

    #endregion
    //internal static void AddLocalRotationRequest(RotationTypeLong face)
    //{
    //    throw new NotImplementedException();
    //}

    //internal static void AddRotationRequest(RotationTypeLong face, Transform m_viewOrientation)
    //{
    //    throw new NotImplementedException();
    //}

    public void LocalRotateWithAcronym(string requestAcryonym) {
        RotationTypeShort acronym = ConvertStringToShortAcronym(requestAcryonym);
        LocalRotate(acronym);
    }



    public void LocalRotate(RotationTypeShort instruction) {
        RubikCubePivotable faceToRotate;
        bool clockwise;
        ConvertAcronymToFaceRotation(instruction, out faceToRotate, out clockwise);
        LocalRotate(faceToRotate, clockwise);

    }

    public static  void ConvertAcronymToFaceRotation(RotationTypeShort instruction, out RubikCubePivotable faceToRotate, out bool clockwise)
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
            case RotationTypeShort.X: return "X";
            case RotationTypeShort.Xp: return "X'";
            case RotationTypeShort.Y: return "Y";
            case RotationTypeShort.Yp: return "Y'";
            case RotationTypeShort.Z: return "Z";
            case RotationTypeShort.Zp: return "Z'";
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

    public static RotationTypeShort ConvertStringToShortAcronym(string requestAcryonym)
    {
        requestAcryonym = requestAcryonym.ToLower();
        switch (requestAcryonym)
        {
            case "b": return RotationTypeShort.B;
            case "b'": case "bp": case "bi": return RotationTypeShort.Bp;
            case "d": return RotationTypeShort.D;
            case "d'": case "dp": case "di": return RotationTypeShort.Dp;
            case "e": return RotationTypeShort.E;
            case "e'": case "ep": case "ei": return RotationTypeShort.Ep;
            case "f": return RotationTypeShort.F;
            case "f'": case "fp": case "fi": return RotationTypeShort.Fp;
            case "l": return RotationTypeShort.L;
            case "l'": case "lp": case "li": return RotationTypeShort.Lp;
            case "m": return RotationTypeShort.M;
            case "m'": case "mp": case "mi": return RotationTypeShort.Mp;

            case "r": return RotationTypeShort.R;
            case "r'": case "rp": case "ri": return RotationTypeShort.Rp;

            case "s": return RotationTypeShort.S;
            case "s'": case "sp": case "si": return RotationTypeShort.Sp;

            case "u": return RotationTypeShort.U;
            case "u'": case "up": case "ui": return RotationTypeShort.Up;
            case "x": return RotationTypeShort.X;
            case "x'": case "xp": case "xi": return RotationTypeShort.Xp;

            case "y": return RotationTypeShort.Y;
            case "y'": case "yp": case "yi": return RotationTypeShort.Yp;

            case "z": return RotationTypeShort.Z;
            case "z'": case "zp": case "zi": return RotationTypeShort.Zp;

            default: throw new  Exception("Impossible to convert to Short Rotation Type");

        }
    }


    public void SaveInitialState() {
        List<PieceInitialState> state = new List<PieceInitialState>();
        foreach (RubikPiece piece in m_pieces)
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

    internal RubikPiece[] GetPieces()
    {
        return m_pieces;
    }

    private PieceInitialState[] m_initialState;

    [Serializable]
    public class PieceInitialState
    {
        public RubikPiece m_linkedPiece;
        public Quaternion m_initialRotation;
        public Vector3 m_initialPosition;

        public PieceInitialState(RubikPiece piece, Quaternion localRotation, Vector3 localPosition)
        {
            this.m_linkedPiece = piece;
            this.m_initialRotation = localRotation;
            this.m_initialPosition = localPosition;
        }
    }


    #region SHUFFLE
    public void ShuffleFace() {
        ShuffleFace(20);
    }
    public void ShuffleFace(int timeToRotate)
    {
        for (int i = 0; i < timeToRotate; i++)
        {
            LocalRotate(GetRandomFace(), GetRandomDirection());
        }
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
       return  m_rotationMotor.IsRotating();
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
        RotateFaceFrom(RubikCube.ConvertStringToShortAcronym(acronym),orientation);
    }

    public void RotateFaceFrom(RotationTypeShort rotation, Transform orientation)
    {
        bool clockwise;
        RubikCubePivotable face;
        RubikCube.ConvertAcronymToFaceRotation(rotation, out face, out clockwise);
        RotateFaceFrom(face, clockwise, ref orientation);

    }
    public void RotateFaceFrom(RubikCubePivotable rotationFace, bool clockwise , ref Transform orientation)
    {
        RubikCubePivotable realFaceToRotate = GetRealFaceOf(rotationFace, ref orientation);
        LocalRotate(realFaceToRotate, clockwise);

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
            case RotationTypeShort.L:return RotationTypeShort.Lp;
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
            case RotationTypeShort.X: return RotationTypeShort.Xp;
            case RotationTypeShort.Xp: return RotationTypeShort.X;
            case RotationTypeShort.Y: return RotationTypeShort.Yp;
            case RotationTypeShort.Yp: return RotationTypeShort.Y;
            case RotationTypeShort.Z: return RotationTypeShort.Zp;
            case RotationTypeShort.Zp: return RotationTypeShort.Z;
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

   public  static Color GetColor(RubikCubeColor colorType) {
        switch (colorType)
        {
            case RubikCubeColor.White:return new Color(1,1,1);
            case RubikCubeColor.Red: return new Color(1, 0, 0);
            case RubikCubeColor.Green: return new Color(0, 1, 0);
            case RubikCubeColor.Blue: return new Color(0, 0, 1);
            case RubikCubeColor.Orange: return new Color(1f, 165f / 255f,0);
            case RubikCubeColor.Yellow: return new Color(1f, 1, 0);
            default:
                throw new Exception("No color define");
        }
    }
    
    public static RubikCubeFace ConvertPivotToFace(RubikCubePivotable pivot)
    {
        switch (pivot)
        {
            case RubikCubePivotable.Left:return RubikCubeFace.Left;
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
        return GetColor(GetMovingFaceFromLocal(face, direction).GetColorEnum());
    }
    internal RubikCubeFaceInfo GetMovingFaceFromLocal(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        RubikCubeFaceInfo askedFace = m_originalfaces.Where(k => k.m_belongToFace == face && k.m_faceDirection == direction).First();
        RubikCubeFaceInfo closestOfAsked = GetMovingFaceAt(askedFace.m_root.position);
        return closestOfAsked;
    }

    private RubikCubeFaceInfo GetMovingFaceAt(Vector3 position)
    {
        return m_piecesfaces.OrderBy(k => Vector3.Distance( k.m_root.position, position)).First();

    }

    #endregion





    public Vector3 GetDirectionFromView(Vector3 position, Transform viewPosition)
    {

        Vector3 objPosition = viewPosition.InverseTransformPoint(position);
        Vector3 rubPosition = viewPosition.InverseTransformPoint(m_root.position);
        return (objPosition - rubPosition).normalized;
    }
    public void RotateFaceFrom(ArrowDirection direction, RubikCubeFaceInfo movingFace, Transform from)
    {
        /////////////HERE///////
        List<RubikCubePivotable> implyFaces = movingFace.GetLinkedFaces().ToList();
        List<RubikCubePivot> pivots = new List<RubikCubePivot>();
        foreach (RubikCubePivotable f in implyFaces)
        { pivots.Add( GetPivot(f)); }

        RubikCubePivot rotationToApply=null;
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
            RubikCubePivotable correspondingPivot = RubikCubePivotable.Left ;

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

        Debug.Log(string.Format("Pivot {0}: {1} ({2})", direction, rotationToApply, rotationClockwise),rotationToApply);
            
        LocalRotate(rotationToApply.m_face, rotationClockwise);
    }

    private bool GetClockWiseFrom(Transform from, RubikCubePivot pivot, RubikCubePivotable pivotrepresented, ArrowDirection direction)
    {
        Vector3 localDirection = from.InverseTransformDirection(pivot.m_root.up);
        Vector3 localDirectionOfPivot=Vector3.zero;
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


        Debug.DrawRay(Vector3.zero, localDirection*5,Color.red,15);
        Debug.Log("Angle: " + angle);
        bool clockwise = angle < 90f;

        return clockwiseDirection?clockwise: !clockwise;
//        return  inverse ? !clockwise:clockwise;

    }

    //private RubikCubePivotable GetFaceFromDirection(Vector3 direction, bool isMiddle)
    //{
    //    Axis dominantAxis = GetBestAxis(direction);
    //    switch (dominantAxis)
    //    {
    //        case Axis.X: return isMiddle ?RubikCubePivotable.Middle : (direction.x > 0 ? RubikCubePivotable.Right : RubikCubePivotable.Left);
    //        case Axis.Y: return isMiddle ? RubikCubePivotable.Equator : (direction.y > 0 ? RubikCubePivotable.Up : RubikCubePivotable.Down);
    //        case Axis.Z: return isMiddle ? RubikCubePivotable.Standing : (direction.z > 0 ? RubikCubePivotable.Face : RubikCubePivotable.Back);
    //        default:
    //            throw new Exception("Humm ??");
    //    }
    //}

    //private Axis GetBestAxis(Vector3 direction)
    //{
    //    Axis dominantAxis =Axis.Z ;
    //    float dominantValue = Mathf.Abs(direction.z);
    //    if (Mathf.Abs(direction.y) > dominantValue)
    //    {
    //        dominantAxis = Axis.Y;
    //        dominantValue = Mathf.Abs(direction.y);
    //    }
    //    if (Mathf.Abs(direction.x ) > dominantValue)
    //    {
    //        dominantAxis = Axis.X;
    //        dominantValue = Mathf.Abs(direction.x);
    //    }


    //    return dominantAxis;
    //}

    public enum Axis { X,Y,Z }

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
    private List<RubikCubePivotable> ExclusiveLateral( List<RubikCubePivotable> implyFaces)
    {

        return
         implyFaces.Where(k => GetRotationType(k) != RotationType.Lateral).ToList();
    }
    private RubikCubePivotable GetRotationTypeFromPivtos(RotationType rotationType, List<RubikCubePivotable> implyFaces)
    {

        return         implyFaces.Where(k => GetRotationType(k) == rotationType).First();
    }




    public enum RotationType { Vertical, Horizontal, Lateral}

    public RotationType GetRotationType(RubikCubePivotable pivot) {
        switch (pivot)
        {
            case RubikCubePivotable.Left:return RotationType.Vertical;
            case RubikCubePivotable.Right:return RotationType.Vertical;
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
    public Color m_white = Color.white;
    public Color m_green = Color.green;
    public Color m_yellow =Color.yellow;
    public Color m_orange = new Color(255f/255f, 165f / 255f, 0f);
    public Color m_blue = Color.blue;
    public Color m_red = Color.red;

    public Color GetDefaultColorOfCube(RubikCubeColor color) {
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

  
    public RubikCubeColor GetDefaultColor(RubikCubeFace face)
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
    //public RubikCubeColor[] GetDefaultColor(RubikCubeFaceInfo face)
    //{
    //    spot.m_fa
    //    return RubikCubeColor.White;
    //}
    //public RubikCubeColor[] GetDefaultColor(RubikCubeFace face)
    //{
    //    spot.m
    //    return RubikCubeColor.White;
    //}


    public RubikCubeSpot FindCubeByColors(params RubikCubeColor[] colors)
    {
        RubikCubeSpot[] spot = m_piecesSpots;
        for (int i = 0; i < spot.Length; i++)
        {
            if (spot[i].m_faces.Length == colors.Length)
            {
                bool allFaceIsIn = true;
                for (int j = 0; j < spot[i].m_faces.Length; j++)
                {
                    bool isDefined;
                    RubikCubeColor col = GetColor(spot[i].m_faces[j], out isDefined);
                    if ( isDefined  && !colors.Contains(col))
                        allFaceIsIn = false;
                }
                if (allFaceIsIn)
                    return spot[i];

            }
        }
        return null;
    }
    public List<RubikCubeSpot> FindAnyCubesWithColor( RubikCubeColor color)
    {
        List<RubikCubeSpot> result = new List<RubikCubeSpot>();
        RubikCubeSpot[] spot = m_piecesSpots;
        for (int i = 0; i < spot.Length; i++)
        {
                for (int j = 0; j < spot[i].m_faces.Length; j++)
                {
                    bool isDefined;
                    RubikCubeColor col = GetColor(spot[i].m_faces[j], out isDefined);
                    if (isDefined && col == color)
                       result.Add(spot[i]);
                }
        }
        return result;
    }


    public RubikPiece GetClosestPiece(Vector3 position)
    {
        return m_pieces.OrderBy(k => Vector3.Distance(k.m_root.position, position)).First();
    }
    public RubikCubePivot GetClosestPivot(Vector3 position)
    {
        return m_pivots.OrderBy(k => Vector3.Distance(k.m_root.position, position)).First();
    }
}

