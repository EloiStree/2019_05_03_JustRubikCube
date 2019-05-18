using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedFaceDebug : MonoBehaviour
{
    public int m_linkCount;
    public GameObject m_debugCube;
    public FaceLinks m_current;
    public TagRubikCube m_cubeFace;
    // Start is called before the first frame update
    void Start()
    {
        m_linkCount=  FaceLinkUtility.GetAllLinks().Length;
        foreach (var item in FaceLinkUtility.GetAllLinks())
        {
            Debug.Log("i:" + item.m_face+" "+ item.m_direction
                + " " + (item.Up() == null ? 0 : 1)
                + " " + (item.Right() == null ? 0 : 1)
                + " " + (item.Down() == null ? 0 : 1)
                + " " + (item.Left() == null ? 0 : 1));
        }


        m_current = FaceLinkUtility.GetFaceLinks(RubikCubeFace.Face, RubikCubeFaceDirection.C);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_current = m_current.Up();
            RepalceAndMove();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            m_current = m_current.Left();
            RepalceAndMove();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            m_current = m_current.Down();
            RepalceAndMove();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            m_current = m_current.Right();
            RepalceAndMove();
        }
    }

    private void RepalceAndMove()
    {
        if(m_current==null)

            m_current = FaceLinkUtility.GetFaceLinks(RubikCubeFace.Face, RubikCubeFaceDirection.C);

        TagRubikCubeFace face = m_cubeFace.GetFace(m_current.GetFace(), m_current.GetDirection());
        m_debugCube.transform.position = face.GetPosition();
    }
}
