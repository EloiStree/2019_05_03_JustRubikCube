using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlatRubikCubeUI : MonoBehaviour {
    

    public RubikCubeFaceUI [] m_uiFaces;
    // Use this for initialization
    public RubikCubeFaceUI GetFace(RubikCubeFace face, RubikCubeFaceDirection direction) {
        return m_uiFaces.Where(k => k.m_face == face && k.m_direction == direction).First() ;
	}

    // Update is called once per frame
    public void SetFaceToColor (RubikCubeFace face, RubikCubeFaceDirection direction, Color color) {
        RubikCubeFaceUI faceResult = GetFace(face, direction);
        faceResult.SetColor(color);


    }
}
