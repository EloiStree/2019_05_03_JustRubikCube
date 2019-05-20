using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawFace : MonoBehaviour {

    public RubikCubeEngineMono m_rubikCube;
    public RubikCubePivotable m_faceToDisplay;
    void Update () {
       switch (m_faceToDisplay)
        {
            case RubikCubePivotable.Left:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Left, 0, Color.yellow);

                break;
            case RubikCubePivotable.Right:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Right, 0, Color.white);

                break;
            case RubikCubePivotable.Up:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Up, 0, new Color(0, 0, 1));

                break;
            case RubikCubePivotable.Down:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Down, 0, Color.green);

                break;
            case RubikCubePivotable.Face:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Face, 0, new Color(255f / 255f, 128f / 255f, 0));

                break;
            case RubikCubePivotable.Back:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Back, 0, Color.red);

                break;
            case RubikCubePivotable.Middle:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Middle, 0, Color.green);
      
                break;
            case RubikCubePivotable.Equator:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Equator, 0, Color.green);
                break;
            case RubikCubePivotable.Standing:
                m_rubikCube.DebugDisplayFace(RubikCubePivotable.Standing, 0, Color.green);
                break;
            default:
                break;
        }
    }
	
}
