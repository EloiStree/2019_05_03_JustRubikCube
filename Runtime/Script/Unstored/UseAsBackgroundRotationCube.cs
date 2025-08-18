using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAsBackgroundRotationCube : MonoBehaviour
{
    public RubikCubeEngineMono m_cube;
    void Awake()
    {
        RubikCubeEngineMono.m_fakeCubeInBackground = m_cube;
    }
    
}
