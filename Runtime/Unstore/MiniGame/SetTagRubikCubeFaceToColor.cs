using UnityEngine;

public class SetTagRubikCubeFaceToColor : MonoBehaviour
{

    public TagRubikCubeFaceMatDrawer m_toAffect;

    public Color m_colorToUse;
    public RubikCubeFace m_faceToAffect;
    public RubikCubeFaceDirection m_directionOfFaceToAffect;


    public void SetTheFacePieceToInspectorColor()
    {

        m_toAffect.SetColorFor(m_faceToAffect, m_directionOfFaceToAffect, m_colorToUse);
    }
    public void SetTheFullFaceToInspectorColor()
    {

        m_toAffect.SetColorFor(m_faceToAffect, m_colorToUse);
    }
}