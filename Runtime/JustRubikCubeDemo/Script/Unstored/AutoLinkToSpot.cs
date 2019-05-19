using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(TagRubikCubeFace))]
public class AutoLinkToSpot : MonoBehaviour {

    public TagRubikCubePiece m_spot;
    public TagRubikCubeFace m_face;

	// Use this for initialization
	void Reset () {
        m_face = GetComponent<TagRubikCubeFace>();
       m_spot= GetClosestSpot();
        m_face.transform.parent = m_spot.m_root;

    }

    private TagRubikCubePiece GetClosestSpot()
    {
       TagRubikCubePiece [] spots = FindObjectsOfType<TagRubikCubePiece>();
        return spots.OrderBy(k => Vector3.Distance(k.m_root.position, this.transform.position)).First() ;
    }
    
}
