using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubeRotateMotorDirect : RubikCubeRotateMotor
{
    public bool isRotating;


    public RubikCubeEngineMono m_affectedRubikCube;

    public override void FinishQueuedRotation()
    {
    }

    public override bool IsRotating()
    {
        return isRotating;
    }

    public override void LocalRotate(RubikCubePivotable faceToRotate, bool clockwise)
    {
        isRotating = true;
        NotifyStartRotation(faceToRotate, clockwise);

        Transform pivot = m_affectedRubikCube.GetPivotTransform(faceToRotate);
        TagRubikCubePiece[] pieces = m_affectedRubikCube.GetPieces(faceToRotate);
        float colockDirection = clockwise?1f:-1f ;
        foreach (TagRubikCubePiece piece in pieces)
        {
            piece.m_root.RotateAround(pivot.position, pivot.up, 90f * colockDirection);
        }

        NotifyEndRotation(faceToRotate, clockwise);
        isRotating = false;
    }

    public override void LocalRotate(RotationTypeShort faceToRotate)
    {
        switch (faceToRotate)
        {
            case RotationTypeShort.L:
                LocalRotate(RubikCubePivotable.Left,true);
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


 
}
