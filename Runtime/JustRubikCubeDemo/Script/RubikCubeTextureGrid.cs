using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubeTextureGrid : MonoBehaviour
{
    public RubikCube m_rubikCube ;
    public SetTextureAsGrid m_testTexture;
    public CoordLinkedToFace[] m_coordinatesToFaceInfo;
    [System.Serializable]
    public class CoordLinkedToFace {
        public int x;
        public int y;
        public RubikCubeFace m_face;
        public RubikCubeFaceDirection m_direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in m_coordinatesToFaceInfo)
        {
            RubikCubeColor rc = RubikCube.GetDefaultColor(item.m_face);
            Color c = RubikCube.GetColor(rc);
            m_testTexture.SetColor(c, item.x, item.y);

        }
        
    }
    
}
