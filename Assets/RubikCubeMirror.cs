using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubeMirror : MonoBehaviour
{
    public RubikCube m_rubikCube;
    public Transform m_root;
    public Transform[] m_pieces;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        CreateFirstPattern();
    }
}
