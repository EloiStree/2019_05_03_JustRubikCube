using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;





public class RubikCubeEngineMono : MonoBehaviour {

    #region  VARIABLE
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

    public CubeDirectionalState m_cubeFaceDirectionState = new CubeDirectionalState();
    public UnityEvent m_onCubeResolved;
    public List<RotationTypeShort> m_rotationHistory = new List<RotationTypeShort>();

    internal RotationSequence GetSequence()
    {
        return new RotationSequence(m_rotationHistory);
    }

    public Dictionary<RubikCubePivotable, List<TagRubikCubePiece>> m_faceLinkedToPivots = new Dictionary<RubikCubePivotable, List<TagRubikCubePiece>>();
    public void SetWithSequence(RotationSequence sequence)
    {
        SetWithSequence(sequence.GetSequenceAsString());
    }
    public void SetWithSequence(string sequence)
    {

        ResetInitialState();
        AddLocalRotationSequence(sequence);
    }

    [Header("Events")]
    public RotationEvent m_onStartRotating = new RotationEvent();
    public RotationEvent m_onEndRotating = new RotationEvent();
    public CubeStateChangeEvent m_onSaveStateChanged = new CubeStateChangeEvent();

    #endregion

    #region STATIC VARIABLE
    public static OnRubikCubeUsed m_onAnyRubikCubeUsed = new OnRubikCubeUsed();
    public static RubikCubeEngineMono m_fakeCubeInBackground;
    #endregion

    #region UNITY MONO FUNCTION
    void Awake()
    {
        SaveLocalPositionOfEahPiece();
        SaveInitialState();
        NotifyCubeStateChange();
        m_rotationMotor.m_onStartRotating.AddListener(OnMotoStartRotating);
        m_rotationMotor.m_onRotated.AddListener(OnMotorEndRotating);
    }

    private void SaveLocalPositionOfEahPiece()
    {
        foreach (TagRubikCubePiece spot in m_fixedCube.m_pieces)
        {
            foreach (RubikCubePivotable face in spot.GetPivots())
            {
                AddSpotToToRegister(face, spot);
            }
        }
    }
    #endregion




    private void OnMotoStartRotating(LocalRotationRequest arg0)
    {
        m_onStartRotating.Invoke(arg0);
    }

    private void OnMotorEndRotating(LocalRotationRequest rotationRequested)
    {
        RefreshCubeStateInformationFromTransformPosition();
        NotifyCubeStateChange();
        m_onEndRotating.Invoke(rotationRequested);
    }

    private void RefreshCubeStateInformationFromTransformPosition()
    {
        RubikCubeFace[] allFaces = RubikCube.GetArrayOf<RubikCubeFace>();
        RubikCubeFaceDirection[] allDirections = RubikCube.GetArrayOf<RubikCubeFaceDirection>();
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

    private void NotifyCubeStateChange()
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
        ToDo.Later(ToDo.PiorityExplicit.Minor, "Could be replace using Index system instead with TagCollectoin");
        return m_faceLinkedToPivots[face];
    }

    private void AddSpotToToRegister(RubikCubePivotable face, TagRubikCubePiece spot)
    {
        if (!m_faceLinkedToPivots.ContainsKey(face))
            m_faceLinkedToPivots.Add(face, new List<TagRubikCubePiece>());
        m_faceLinkedToPivots[face].Add(spot);
    }

    public Transform GetPivotTransform(RubikCubePivotable faceToTurn)
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

    public TagRubikCubePiece[] GetPieces(RubikCubePivotable face)
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


    [ContextMenu("Rotation Local Face")]
    public void AddLocalRotationFace() => AddLocalRotate(RotationTypeShort.F);
    [ContextMenu("Rotation Local Inverse Face")]
    public void AddLocalRotationFaceInverse() => AddLocalRotate(RotationTypeShort.Fp);

    [ContextMenu("Rotation Global Face")]
    public void AddPointRotationFace() => RotateFaceFrom(RotationTypeShort.F, Camera.main.transform);
    [ContextMenu("Rotation Global  Inverse Face")]
    public void AddPointRotationFaceInverse() => RotateFaceFrom(RotationTypeShort.Fp, Camera.main.transform);

  
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

    //add an array for string to RotationTypeShort


    public class StringToRotationTypeShort
    {
        public string m_textLowerCase;
        public RotationTypeShort m_value;

        public StringToRotationTypeShort() {

            m_textLowerCase = "";
            m_value = RotationTypeShort.M;
        }
        public StringToRotationTypeShort(string text, RotationTypeShort rotation)
        {
            m_textLowerCase = text;
            m_value = rotation;
        }

    }

    StringToRotationTypeShort[] m_stringToRotation = new StringToRotationTypeShort[]{

 new StringToRotationTypeShort(RotationTypeShort.L .ToString().ToLower(), RotationTypeShort.L ),
 new StringToRotationTypeShort(RotationTypeShort.R .ToString().ToLower(), RotationTypeShort.R ),
 new StringToRotationTypeShort(RotationTypeShort.U .ToString().ToLower(), RotationTypeShort.U ),
 new StringToRotationTypeShort(RotationTypeShort.D .ToString().ToLower(), RotationTypeShort.D ),
 new StringToRotationTypeShort(RotationTypeShort.F .ToString().ToLower(), RotationTypeShort.F ),
 new StringToRotationTypeShort(RotationTypeShort.B .ToString().ToLower(), RotationTypeShort.B ),
 new StringToRotationTypeShort(RotationTypeShort.M .ToString().ToLower(), RotationTypeShort.M ),
 new StringToRotationTypeShort(RotationTypeShort.E .ToString().ToLower(), RotationTypeShort.E ),
 new StringToRotationTypeShort(RotationTypeShort.S .ToString().ToLower(), RotationTypeShort.S ),
 new StringToRotationTypeShort(RotationTypeShort.Lp.ToString().ToLower(), RotationTypeShort.Lp),
 new StringToRotationTypeShort(RotationTypeShort.Rp.ToString().ToLower(), RotationTypeShort.Rp),
 new StringToRotationTypeShort(RotationTypeShort.Up.ToString().ToLower(), RotationTypeShort.Up),
 new StringToRotationTypeShort(RotationTypeShort.Dp.ToString().ToLower(), RotationTypeShort.Dp),
 new StringToRotationTypeShort(RotationTypeShort.Fp.ToString().ToLower(), RotationTypeShort.Fp),
 new StringToRotationTypeShort(RotationTypeShort.Bp.ToString().ToLower(), RotationTypeShort.Bp),
 new StringToRotationTypeShort(RotationTypeShort.Mp.ToString().ToLower(), RotationTypeShort.Mp),
 new StringToRotationTypeShort(RotationTypeShort.Ep.ToString().ToLower(), RotationTypeShort.Ep),
 new StringToRotationTypeShort("Si".ToLower(), RotationTypeShort.Sp),
 new StringToRotationTypeShort("Li".ToLower(), RotationTypeShort.Lp),
 new StringToRotationTypeShort("Ri".ToLower(), RotationTypeShort.Rp),
 new StringToRotationTypeShort("Ui".ToLower(), RotationTypeShort.Up),
 new StringToRotationTypeShort("Di".ToLower(), RotationTypeShort.Dp),
 new StringToRotationTypeShort("Fi".ToLower(), RotationTypeShort.Fp),
 new StringToRotationTypeShort("Bi".ToLower(), RotationTypeShort.Bp),
 new StringToRotationTypeShort("Mi".ToLower(), RotationTypeShort.Mp),
 new StringToRotationTypeShort("Ei".ToLower(), RotationTypeShort.Ep),
 new StringToRotationTypeShort("Si".ToLower(), RotationTypeShort.Sp)
    };
    public void TranslateToEnumRotation(string shortcut, out bool found, out RotationTypeShort rotation) { 
    
        shortcut = shortcut.ToLower();
        foreach (var k in m_stringToRotation) {

            if (shortcut == k.m_textLowerCase)
            {
                found = true;
                rotation =  k.m_value;
                return;
            }
        }
        found = false;
        rotation = RotationTypeShort.M;

    }
    public void AddRotationSequence(string sequence, Transform orientation)
    {

        string[] seg = sequence.Split(new char[] { ' ', ':', ';', ',' });
        foreach (string segment in seg)
        {
            try
            {
                TranslateToEnumRotation(segment, out bool found, out RotationTypeShort rot);
                if (found) { 
                
                    if (orientation == null)
                        AddLocalRotate(rot);
                    else
                        RotateFaceFrom(rot, orientation);
                }
            }
            catch (Exception) { }
        }
    }
    //public void AddLocalRotateFromRotation(string rotation)
    //{
    //    //RotationTypeShort rotationType;
    //    //if (RubikCube.ConvertStringToShortAcronym(rotation, out rotationType))
    //    //    AddLocalRotate(rotationType);
    //}


    public void AddLocalRotate(RubikCubePivotable faceToTurn, bool clockWise)
    {
      
        RotationTypeShort rotationType = RubikCube.ConvertRotationToAcronymShort(faceToTurn, clockWise);
        m_rotationHistory.Add(rotationType);
        m_onAnyRubikCubeUsed.Invoke(this);
        Debug.Log("THE A FUCK");
        m_rotationMotor.LocalRotate(faceToTurn, clockWise);
    }

    public void FinishMotorQueuedRotation()
    {
        m_rotationMotor.FinishQueuedRotation();

    }

    #region ROTATION LISTENER

    


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
        m_onEndRotating.Invoke(req);
    }
    public void NotifyStateChange(CubeDirectionalState newState)
    {
        m_onSaveStateChanged.Invoke(newState);
        if (IsCubeResolved())
            NotifyCubeAsResolved();
    }


    #endregion

    #region Solution LISTENER



    public bool IsCubeResolved()
    {
        return m_cubeFaceDirectionState.IsCubeSolved();
    }

    public void NotifyCubeAsResolved() {
        m_onCubeResolved.Invoke();
    }

    #endregion

    public void AddLocalRotateWithAcronym(string requestAcryonym) {
        RotationTypeShort acronym;
        if (RubikCube.ConvertStringToShortAcronym(requestAcryonym, out acronym))
            AddLocalRotate(acronym);
    }



    public void AddLocalRotate(RotationTypeShort instruction) {
        RubikCubePivotable faceToRotate;
        bool clockwise;
        RubikCube.ConvertAcronymToFaceRotation(instruction, out faceToRotate, out clockwise);

        AddLocalRotate(faceToRotate, clockwise);

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
        m_rotationHistory.Clear();

    }

    public PieceInitialState[] GetInitialSpots()
    {
        return m_initialState;
    }
    

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



   

    private bool GetRandomDirection()
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

    public bool IsRotating()
    {
        return m_rotationMotor.IsRotating();
    }
    #endregion

    #region ROTATE THE CUBE FROM POINT OF 
  

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

   
    public Color GetRgbColorFromLocal(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        TagRubikCubeFace selectedFace = GetMovingFaceFromLocal(face, direction);
        if (selectedFace)
            return RubikCube. GetColor(selectedFace.GetColorEnum());
        return Color.black;
    }

    public TagRubikCubeFace GetMovingFaceFromLocal(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
        RubikCubeFace currentFace;
        RubikCubeFaceDirection currentDirection;
        m_cubeFaceDirectionState.GetRealPieceFaceInfoAt(face, direction, out currentFace, out currentDirection);
        return m_movingCube.GetFace(currentFace, currentDirection);

    }
    public TagRubikCubeFace GetMovingFace(RubikCubeFace face, RubikCubeFaceDirection direction)
    {
       return m_movingCube.GetFace(face, direction);

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
                    RubikCubeColor col = RubikCube. GetColor(pivots[j], out isDefined);
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
                RubikCubeColor col = RubikCube. GetColor(pivots[j], out isDefined);
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
    

    public CubeDirectionalState GetCubeState() {

        return m_cubeFaceDirectionState;
    }

    public void AddLocalRotationSequence(RotationSequence sequence)
    {
        foreach (RotationTypeShort shortRotation in sequence.GetSequenceAsShort())
        {
            AddLocalRotate(shortRotation);
        }
    }






}


#region RUBIK CUBE EVENT
#endregion

