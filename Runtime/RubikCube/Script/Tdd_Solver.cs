using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tdd_Solver : MonoBehaviour {
    public float m_startAfter = 1;
    public RubikCubeRotateMotorSlow m_motor;
    public RubikCube.LocalRotationRequest[] m_sequence;
    
    public int m_loop = 10;


	IEnumerator Start () {
        yield return new WaitForSeconds(m_startAfter);
        for (int i = 0; i < m_loop; i++)
        {
            foreach (RubikCube.LocalRotationRequest note in m_sequence)
            {
                m_motor.LocalRotate(note.m_faceToRotate, note.m_clockWise);
            }
        }
    }
	
}
