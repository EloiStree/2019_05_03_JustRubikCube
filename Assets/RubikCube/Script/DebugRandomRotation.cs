using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRandomRotation : MonoBehaviour {

    public KeyCode m_letterToRandomRotate = KeyCode.F1;
	void Update () {
        if (Input.GetKeyDown(m_letterToRandomRotate)) {

            RotateRandomly();
        }
        
	}

    public void RotateRandomly()
    {
        transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
    }
}
