using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlatRubikCubeUI : MonoBehaviour {
    

    public RubikCubeFaceUI [] m_uiFaces;
    public RubikFaceDictionary<RubikCubeFaceUI> m_facesRef = new RubikFaceDictionary<RubikCubeFaceUI>();
    public void Awake()
    {
        foreach (var item in m_uiFaces)
        {
            m_facesRef.Add(item.m_face, item.m_direction, item);
        }
    }
    // Use this for initialization
    public RubikCubeFaceUI GetFace(RubikCubeFace face, RubikCubeFaceDirection direction) {
        if (m_facesRef.Contains(face, direction))
            return m_facesRef.Get(face, direction);
        return null;
	}

  

    // Update is called once per frame
    public void SetFaceToColor (RubikCubeFace face, RubikCubeFaceDirection direction, Color color) {
        RubikCubeFaceUI faceResult = GetFace(face, direction);
        if(faceResult)
         faceResult.SetColor(color);


    }
}

