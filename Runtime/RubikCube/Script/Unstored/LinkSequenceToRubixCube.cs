using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StoredSequence))]
public class LinkSequenceToRubixCube : MonoBehaviour
{
    public StoredSequence m_sequence;
    public RubikCube m_rubik;
    public bool m_focusLastUsedRubikCube = true;
    [Header("Auto Link")]
    public Button m_autoLinkButtons;

    public void Awake()
    {
        RubikCube.onAnyRubikCubeUsed.AddListener(ChangeSelection);
        if (m_autoLinkButtons != null)
            m_autoLinkButtons.onClick.AddListener(Apply);
    }

    private void ChangeSelection(RubikCube cube)
    {
        if (m_focusLastUsedRubikCube) {
            m_rubik = cube;
        }
    }

    public void OnDestroy()
    {

        RubikCube.onAnyRubikCubeUsed.RemoveListener(ChangeSelection);
    }
    public void Apply() {
        if (m_rubik != null)
        {
            if( m_sequence.m_sequenceType== StoredSequence.SequenceType.DefaultCamera)
                m_rubik.AddRotationSequenceWithDefaultCamera(m_sequence.m_sequence);
            else m_rubik.AddLocalRotationSequence(m_sequence.m_sequence);


        }
    }

    public void OnMouseDown()
    {
        Apply();
    }

    private void OnValidate()
    {
        if (m_autoLinkButtons == null)
            m_autoLinkButtons = GetComponent<Button>();
        m_sequence = GetComponent<StoredSequence>();
    }
}
