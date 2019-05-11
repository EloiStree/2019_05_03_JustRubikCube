using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikPiece : MonoBehaviour {


    public Transform m_root;
    public RubikCube m_parentRubikCube;
    
	

    public void Reset()
    {
        m_root = this.transform;
        m_parentRubikCube = this.GetComponentInParent<RubikCube>();
    }
    //public void Rotate(RotationRequest rotationRequest) {
    //    //TODO 
    //}
    //public void Rotate(ArrowDirection direction, Vector3 originePosition, Quaternion origineRotation) {
    //    Rotate(new RotationRequest() { m_direction = direction, m_originePosition = originePosition, m_origineRotation = origineRotation });
    //}
}


public class RotationRequest
{
    public ArrowDirection m_direction;
    public Vector3 m_originePosition;
    public Quaternion m_origineRotation;
}

public class RubikPieceRotationRequest : RotationRequest
{
    public RubikPiece m_rubikTargeted;
}
