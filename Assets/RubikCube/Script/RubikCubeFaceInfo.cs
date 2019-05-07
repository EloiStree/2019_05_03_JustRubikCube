using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubeFaceInfo : MonoBehaviour {

    public Transform m_root;
    public RubikCubeSpot m_linkedSpot;
    public RubikCubeFace m_belongToFace;
    public RubikCubeFaceDirection m_faceDirection;

 
    private void Reset()
    {
        m_root = this.transform;
        m_linkedSpot = this.GetComponentInParent<RubikCubeSpot>();
    }

    internal RubikCubePivotable [] GetLinkedFaces()
    {
        return m_linkedSpot.m_faces;
    }

    public RubikCubeColor GetColorEnum() {
        return RubikCube.GetColor(m_belongToFace);
    }

    internal RubikCubePivotable GetPivot()
    {
        return RubikCube.ConvertFaceToPivot(m_belongToFace);
    }
}
