using UnityEngine;

public class RubikMono_FaceColorStateFromTag : MonoBehaviour
{
    public RubikCubeFace m_targetFaceInChildren;
    public TagRubikCubeOfficialColor m_faceC;
    public TagRubikCubeOfficialColor m_faceN ;
    public TagRubikCubeOfficialColor m_faceNO;
    public TagRubikCubeOfficialColor m_faceO ;
    public TagRubikCubeOfficialColor m_faceSO;
    public TagRubikCubeOfficialColor m_faceS ;
    public TagRubikCubeOfficialColor m_faceSE;
    public TagRubikCubeOfficialColor m_faceE ;
    public TagRubikCubeOfficialColor m_faceNE;


    private void Awake()
    {
        FetchTagInChildren();
    }

    [ContextMenu("Refresh from children")]
    public void FetchTagInChildren() {

        TagRubikCubeOfficialColor[]  child = this.GetComponentsInChildren<TagRubikCubeOfficialColor>();
        foreach (var tagColor in child) { 
        
            TagRubikCubeFace tagFace = tagColor.GetComponent<TagRubikCubeFace>();

            if (tagFace != null) {
                switch (tagFace.m_faceDirection) {
                    case RubikCubeFaceDirection.N: m_faceN = tagColor; break;
                    case RubikCubeFaceDirection.NE: m_faceNE = tagColor; break;
                    case RubikCubeFaceDirection.E: m_faceE = tagColor; break;
                    case RubikCubeFaceDirection.SE: m_faceSE = tagColor; break;
                    case RubikCubeFaceDirection.S: m_faceS = tagColor; break;
                    case RubikCubeFaceDirection.SO: m_faceSO = tagColor; break;
                    case RubikCubeFaceDirection.O: m_faceO = tagColor; break;
                    case RubikCubeFaceDirection.NO: m_faceNO = tagColor; break;
                    case RubikCubeFaceDirection.C: m_faceC = tagColor; break;
                }
            }

        }

    }
}
