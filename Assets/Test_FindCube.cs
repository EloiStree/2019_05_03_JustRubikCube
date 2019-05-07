using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FindCube : MonoBehaviour
{

    public RubikCube m_rubik;
    public RubikCubeHighlight m_highlighter;

    public bool m_any=false;
    public RubikCubeColor m_allOfColor;
    public bool m_specific = true;
    public RubikCubeColor[] m_colorOfCube;
    public List<RubikCubeSpot> cubes;
    public RubikCubeSpot[] allSpot;


    void Start()
    {
        InvokeRepeating("OnValidate", 0, 1);   
    }

    private void OnValidate()
    {
     
        if (Application.isPlaying) {
            // allSpot = m_rubik.m_piecesSpots;
            //for (int i = 0; i < allSpot.Length; i++)
            //{
            //   // allSpot[i].transform.localScale = Vector3.one * 0.8f;
            //}

            //cubes = m_rubik.FindAnyCubesWithColor(m_allOfColor);
            //for (int i = 0; i < cubes.Count; i++)
            //{
            //   // cubes[i].transform.localScale =Vector3.one * 1.5f;
            //}

        }
    }
}
