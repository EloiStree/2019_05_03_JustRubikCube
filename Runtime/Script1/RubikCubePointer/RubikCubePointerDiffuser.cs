using UnityEngine;
using UnityEngine.Events;

public class RubikCubePointerDiffuser : MonoBehaviour {


    private void Reset()
    {
        m_source = GetComponent<RubikCubePointer>();

        if (m_source == null )
            m_source = GetComponentInParent<RubikCubePointer>(true);
    }

    public RubikCubePointer m_source;

    public UnityEvent<TagRubikCubeFace> m_onsPushSelectedFirstFace;
    public UnityEvent<StoredSequence> m_onsPushSelectedSequence;

    public string m_lastPushTextSequence;

    [ContextMenu("Relay the face selected")]
    public void RelayAllSelected()
    {

        RelayTheFaceSelected();
        RelayTheSequenceseSelected();
    }

    [ContextMenu("Relay the face selected")]
    public void RelayTheFaceSelected()
    {
        m_onsPushSelectedFirstFace.Invoke(m_source.GetSelectedFace());
    }

    [ContextMenu("Relay the face selected")]
    public void RelayTheSequenceseSelected()
    {
        var s = m_source.GetSelectedSequence();

        m_onsPushSelectedSequence.Invoke(s);
        m_lastPushTextSequence = s.m_sequence;  
    }


}
