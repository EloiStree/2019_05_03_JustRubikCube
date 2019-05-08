using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(RubikCubeFaceInfo))]
public class AutoLinkToSpot : MonoBehaviour {

    public RubikCubeSpot m_spot;
    public RubikCubeFaceInfo m_face;

	// Use this for initialization
	void Reset () {
        m_face = GetComponent<RubikCubeFaceInfo>();
       m_spot= GetClosestSpot();
        m_face.transform.parent = m_spot.m_root;

    }

    private RubikCubeSpot GetClosestSpot()
    {
       RubikCubeSpot [] spots = FindObjectsOfType<RubikCubeSpot>();
        return spots.OrderBy(k => Vector3.Distance(k.m_root.position, this.transform.position)).First() ;
    }
    
}
