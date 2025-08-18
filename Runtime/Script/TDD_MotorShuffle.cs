using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_MotorShuffle : MonoBehaviour {

    public RubikCubeRotateMotor m_targetMotor;
    public int m_numberRotation=50;
	// Use this for initialization
	void Start () {
       
	}

    private void ShuffleFace(int timeToRotate = 20)
    {
        for (int i = 0; i < m_numberRotation; i++)
        {
            m_targetMotor.LocalRotate(GetRandomFace(), GetRandomDirection());
        }
    }

    private RubikCubePivotable GetRandomFace()
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
}
