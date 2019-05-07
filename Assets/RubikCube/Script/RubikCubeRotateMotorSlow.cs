using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubeRotateMotorSlow : RubikCubeRotateMotor
{


    public RubikCube m_affectedRubikCube;
    public float m_rotationSpeed = 30f;

    [Header("Debug")]
    public float m_leftAngleToRotate;
    public bool m_isRotating;
    public int m_inQueue;
    public RegisteredRotation m_currentRotation;
    public Queue<RegisteredRotation> m_queue = new Queue<RegisteredRotation>() ;

    public class RegisteredRotation {
        public RubikCubePivotable m_face;
        public bool m_clockWise;
    }


    public void Update()
    {
        if (HasRotation() && !IsRotating())
            SetRotationTo(GetNextRotation());

        if (IsRotating())
        {
            float angleToRotate = Time.deltaTime*m_rotationSpeed;
            if (angleToRotate > m_leftAngleToRotate)
                angleToRotate = m_leftAngleToRotate;
            
            foreach (RubikPiece piece in pieces)
            {
                piece.m_root.RotateAround(pivot.position, pivot.up, angleToRotate * colockDirection);
            }

            m_leftAngleToRotate -= angleToRotate;
            if (m_leftAngleToRotate <= 0f) {
                m_isRotating = false;
                m_inQueue = m_queue.Count;
                NotifyEndRotation(m_currentRotation.m_face, m_currentRotation.m_clockWise);


            }

        }

        if (m_rotationSpeed <= 0f)
            FinishQueuedRotation();



    }
    public Transform pivot;
    public RubikPiece[] pieces;
    public float colockDirection;


    public void SetSpeed(float speed)
    {
        m_rotationSpeed = speed;
    }
  

    private void SetRotationTo(RegisteredRotation registeredRotation)
    {
        m_isRotating = true;
        m_leftAngleToRotate = 90f;

        pivot  = m_affectedRubikCube.GetPivotTransform(registeredRotation.m_face);
        pieces = m_affectedRubikCube.GetPieces(registeredRotation.m_face);
        colockDirection = registeredRotation.m_clockWise ? 1f : -1f;
        m_currentRotation = registeredRotation;
        NotifyStartRotation(m_currentRotation.m_face, m_currentRotation.m_clockWise);

    }

    private RegisteredRotation GetNextRotation()
    {
       return m_queue.Dequeue();
    }

    private bool HasRotation()
    {
        return m_queue.Count > 0;
    }

    public override bool IsRotating()
    {
        return m_isRotating;
    }

    public override void LocalRotate(RubikCubePivotable faceToRotate, bool clockwise)
    {
        m_queue.Enqueue(new RegisteredRotation() { m_face = faceToRotate, m_clockWise = clockwise });
        m_inQueue = m_queue.Count;
    }

    public override void LocalRotate(RotationTypeShort faceToRotate)
    {
        switch (faceToRotate)
        {
            case RotationTypeShort.L:
                LocalRotate(RubikCubePivotable.Left, true);
                break;
            case RotationTypeShort.Lp:
                LocalRotate(RubikCubePivotable.Left, false);
                break;
            case RotationTypeShort.R:
                LocalRotate(RubikCubePivotable.Right, true);
                break;
            case RotationTypeShort.Rp:
                LocalRotate(RubikCubePivotable.Right, false);
                break;
            case RotationTypeShort.U:
                LocalRotate(RubikCubePivotable.Up, true);
                break;
            case RotationTypeShort.Up:
                LocalRotate(RubikCubePivotable.Up, false);
                break;
            case RotationTypeShort.D:
                LocalRotate(RubikCubePivotable.Down, true);
                break;
            case RotationTypeShort.Dp:
                LocalRotate(RubikCubePivotable.Down, false);
                break;
            case RotationTypeShort.F:
                LocalRotate(RubikCubePivotable.Face, true);
                break;
            case RotationTypeShort.Fp:
                LocalRotate(RubikCubePivotable.Face, false);
                break;
            case RotationTypeShort.B:
                LocalRotate(RubikCubePivotable.Back, true);
                break;
            case RotationTypeShort.Bp:
                LocalRotate(RubikCubePivotable.Back, false);
                break;
            case RotationTypeShort.M:
                LocalRotate(RubikCubePivotable.Middle, true);
                break;
            case RotationTypeShort.Mp:
                LocalRotate(RubikCubePivotable.Middle, false);
                break;
            case RotationTypeShort.E:
                LocalRotate(RubikCubePivotable.Equator, true);
                break;
            case RotationTypeShort.Ep:
                LocalRotate(RubikCubePivotable.Equator, false);
                break;
            case RotationTypeShort.S:
                LocalRotate(RubikCubePivotable.Standing, true);
                break;
            case RotationTypeShort.Sp:
                LocalRotate(RubikCubePivotable.Standing, false);
                break;
        }
    }

    protected override void NotifyStartRotation(RubikCubePivotable face, bool clockWise)
    {
        RubikCube.LocalRotationRequest request = new RubikCube.LocalRotationRequest(face, clockWise);
        m_onStartRotating.Invoke(request);
        //m_affectedRubikCube.NotifyStartRotation(request);
    }
    protected override void NotifyEndRotation(RubikCubePivotable face, bool clockWise)
    {
        RubikCube.LocalRotationRequest request = new RubikCube.LocalRotationRequest(face, clockWise);
        m_onRotated.Invoke(request);
        //m_affectedRubikCube.NotifyEndRotation(request);
    }

    public override void FinishQueuedRotation()
    {
        if (IsRotating())
        {
            foreach (RubikPiece piece in pieces)
            {
                piece.m_root.RotateAround(pivot.position, pivot.up, m_leftAngleToRotate);
            }
            m_leftAngleToRotate=0;
            m_isRotating = false;
            m_inQueue = m_queue.Count;
            NotifyEndRotation(m_currentRotation.m_face, m_currentRotation.m_clockWise);
        }
        while (m_queue.Count > 0)
        {
            m_currentRotation = m_queue.Dequeue();
            m_inQueue = m_queue.Count;
            ImmediateLocalRotate(m_currentRotation.m_face, m_currentRotation.m_clockWise);
        }

    }

    public void ImmediateLocalRotate(RubikCubePivotable faceToRotate, bool clockwise)
    {
        m_isRotating = true;
        NotifyStartRotation(faceToRotate, clockwise);

        Transform pivot = m_affectedRubikCube.GetPivotTransform(faceToRotate);
        RubikPiece[] pieces = m_affectedRubikCube.GetPieces(faceToRotate);
        float colockDirection = clockwise ? 1f : -1f;
        foreach (RubikPiece piece in pieces)
        {
            piece.m_root.RotateAround(pivot.position, pivot.up, 90f * colockDirection);
        }

        NotifyEndRotation(faceToRotate, clockwise);
        m_isRotating = false;
    }
}