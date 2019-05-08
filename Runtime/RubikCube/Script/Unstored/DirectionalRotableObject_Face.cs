using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalRotableObject_Face : MonoBehaviour, IDirectionalRotableObject
{
    public RubikCubeFaceInfo m_linkedFace;
    public RubikCube m_rubikCube;

    private void RotateRubik(Transform pointOfView, ArrowDirection direction)
    {
        m_rubikCube.RotateFaceFrom(
            direction, m_linkedFace, pointOfView);
    }

    public void TurnDown(Transform pointOfView)
    {
        RotateRubik(pointOfView, ArrowDirection.Down);
    }



    public void TurnLeft(Transform pointOfView)
    {
        RotateRubik(pointOfView, ArrowDirection.Left);
    }


    public void TurnRight(Transform pointOfView)
    {
        RotateRubik(pointOfView, ArrowDirection.Right);
    }


    public void TurnDownFromUserView()
    {
        if (Camera.main)
            TurnDown(Camera.main.transform);
    }
    public void TurnLeftFromUserView()
    {
        if (Camera.main)
            TurnLeft(Camera.main.transform);
    }
    public void TurnUp(Transform pointOfView)
    {
        RotateRubik(pointOfView, ArrowDirection.Up);
    }
    public void TurnRightFromUserView()
    {
        if (Camera.main)
            TurnRight(Camera.main.transform);
    }

    public void TurnUpFromUserView()
    {
        if (Camera.main)
            TurnUp(Camera.main.transform);
    }

    public void OnValidate()
    {
        if (m_rubikCube == null)
            m_rubikCube = GetComponentInParent<RubikCube>();
        if (m_linkedFace == null)
            m_linkedFace = GetComponentInParent<RubikCubeFaceInfo>();
    }
}

public interface IDirectionalRotableObject {
    void TurnLeftFromUserView();
    void TurnRightFromUserView();
    void TurnUpFromUserView();
    void TurnDownFromUserView();
    void TurnLeft(Transform pointOfView);
    void TurnRight(Transform pointOfView);
    void TurnUp(Transform pointOfView);
    void TurnDown(Transform pointOfView);
}
public abstract class DirectionalRotableObject : MonoBehaviour, IDirectionalRotableObject
{
    
    public abstract void TurnDown(Transform pointOfView);
    public abstract void TurnDownFromUserView();
    public abstract void TurnLeft(Transform pointOfView);
    public abstract void TurnLeftFromUserView();
    public abstract void TurnRight(Transform pointOfView);
    public abstract void TurnRightFromUserView();
    public abstract void TurnUp(Transform pointOfView);
    public abstract void TurnUpFromUserView();
}


