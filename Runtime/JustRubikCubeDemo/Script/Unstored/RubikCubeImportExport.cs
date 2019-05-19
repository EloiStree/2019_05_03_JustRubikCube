using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class RubikCubeImportExport : MonoBehaviour
{
    public RubikCube  m_rubikCube;
    public SequenceHistory m_history;
    public InputField m_input;



    public void Import()
    {
        SetRubikWIthSequence(m_input.text);
    }

    private void SetRubikWIthSequence(string sequence)
    {
        m_rubikCube.ResetInitialState();
        m_rubikCube.AddLocalRotationSequence(sequence);
        m_rubikCube.FinishMotorQueuedRotation();
    }

    public void Export() {
        m_input.text = m_history.GetSequenceAsString();
    }


   
    
}
