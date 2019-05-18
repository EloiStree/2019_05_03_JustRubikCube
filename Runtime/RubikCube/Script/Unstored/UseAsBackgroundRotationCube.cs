using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAsBackgroundRotationCube : MonoBehaviour
{
    public RubikCube m_cube;
    void Awake()
    {
        RubikCube.m_fakeCubeInBackground = m_cube;
    }
    
}
