using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RubikCubeRotateMotor : MonoBehaviour {

    public abstract bool IsRotating();
    public abstract void LocalRotate(RubikCubePivotable faceToRotate, bool clockwise);
    public abstract void LocalRotate(RotationTypeShort faceToRotate);

    [Header("Events")]
    public RotationEvent m_onStartRotating;
    public RotationEvent m_onRotated;
    public UnityEvent m_onStartMotorRotation;
    public UnityEvent m_onStopMotorRotation;

    protected virtual void NotifyStartRotation(RubikCubePivotable face, bool clockWise)
    {
        m_onStartRotating.Invoke(new RubikCubeEngineMono.LocalRotationRequest(face, clockWise));
    }
    protected virtual void NotifyEndRotation(RubikCubePivotable face, bool clockWise)
    {
        m_onRotated.Invoke(new RubikCubeEngineMono.LocalRotationRequest(face, clockWise));
    }
    protected virtual void NotifyStartUsingMotor()
    {
        m_onStartMotorRotation.Invoke();
    }
    protected virtual void NotifyEndUsingMotor()
    {
        m_onStopMotorRotation.Invoke();
    }


    public abstract void FinishQueuedRotation();
}
