using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StoredSequence))]
public class LinkSequenceToRubixCube : MonoBehaviour
{
    public Button m_button;
    public StoredSequence m_sequence;
    public RubikCube m_rubik;

    public void Awake()
    {
        if(m_button)
        m_button.onClick.AddListener(Apply);
    }

    public void Apply() {
        if (m_rubik != null)
        {
            if( m_sequence.m_sequenceType== StoredSequence.SequenceType.DefaultCamera)
                m_rubik.AddRotationSequenceWithDefaultCamera(m_sequence.m_sequence);
            else m_rubik.AddLocalRotationSequence(m_sequence.m_sequence);


        }
    }

    private void OnValidate()
    {
        m_button = GetComponent<Button>();
        m_sequence = GetComponent<StoredSequence>();
    }
}
