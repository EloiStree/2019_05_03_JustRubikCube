using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotationOf : MonoBehaviour {

    public Transform m_rotationToMirror;

	void Update () {
        transform.rotation = m_rotationToMirror.rotation;

    }
}
