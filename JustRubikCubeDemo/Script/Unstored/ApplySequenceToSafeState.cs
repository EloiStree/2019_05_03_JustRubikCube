using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplySequenceToSafeState : MonoBehaviour
{
    public StoredSequence m_sequence;
    public RubikCubeSafeStateMono m_safeState;
    public bool m_applyAtStart=true;
    void Start()
    {
        if (m_applyAtStart)
            Apply();        
    }

    public void Apply(string sequence) {
        m_sequence.SetSequence(sequence);
        Apply();
    }

    public void Apply()
    {
      CubeDirectionalState state = RubikCube.CreateCubeStateFrom(new RotationSequence(m_sequence.m_sequence));
        m_safeState.RefreshCubeState(state);
    }
    public void OnValidate()
    {
        Apply();
    }
}
