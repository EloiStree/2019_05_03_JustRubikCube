using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardToRubikRotation : MonoBehaviour {

    public RubikCubeEngineMono m_affectedRubikCube;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool foundOne = false;
        RubikCubePivotable faceToTurn = GetFaceToTurn(out foundOne);

        if(foundOne)
         m_affectedRubikCube.AddLocalRotate(faceToTurn, GetClockDirection());

        if (Input.GetKeyDown(KeyCode.End))
            m_affectedRubikCube.ResetInitialState();
    }

    private static bool GetClockDirection()
    {
        return !( Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }

    private static RubikCubePivotable GetFaceToTurn(out bool foundOne)
    {
        foundOne = false;
        RubikCubePivotable faceToTurn = RubikCubePivotable.Middle;
        if (Input.GetKeyDown(KeyCode.B))
        {
            faceToTurn = RubikCubePivotable.Back;
            foundOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            faceToTurn = RubikCubePivotable.Down;
            foundOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            faceToTurn = RubikCubePivotable.Equator;
            foundOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            faceToTurn = RubikCubePivotable.Face;
            foundOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            faceToTurn = RubikCubePivotable.Left;
            foundOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            faceToTurn = RubikCubePivotable.Middle;
            foundOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            faceToTurn = RubikCubePivotable.Right;
            foundOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            faceToTurn = RubikCubePivotable.Standing;
            foundOne = true;
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            faceToTurn = RubikCubePivotable.Up;
            foundOne = true;
        }

        return faceToTurn;
    }
}
