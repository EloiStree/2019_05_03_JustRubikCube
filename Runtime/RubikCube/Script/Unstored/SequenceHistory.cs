using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceHistory : MonoBehaviour
{
    [SerializeField] RubikCube m_rubikCube=null;
    [SerializeField] RotationSequence m_sequence = new RotationSequence();
    [SerializeField] TimedRotationSequence m_timedSequence = new TimedRotationSequence();
    [SerializeField] TimePast m_timePasted;
    void Start()
    {
        m_timePasted.StartTimer();
        m_rubikCube.m_onStartRotating.AddListener(AddToHistory);

    }


    private void AddToHistory(RubikCube.LocalRotationRequest request)
    {
        float time = m_timePasted.GetTimePast();
        RotationTypeShort rotation = RubikCube.ConvertRotationToAcronymShort(request.m_faceToRotate, request.m_clockWise);
        m_sequence.Add(rotation);
        m_timedSequence.Add(rotation, time);
         m_sequence.GetSequenceAsString();
    }

    

    public RubikCube GetLinkedCube() { return m_rubikCube; }
    public RotationTypeShort[] GetSequenceAsRotation() { return m_sequence.m_rotations.ToArray(); }
    public RotationSequence GetSequence() { return m_sequence; }
    public TimedRotationSequence GetTimedSequence() { return m_timedSequence; }
    public string GetSequenceAsString() { return m_sequence.GetSequenceAsString(); }

    public void OverrideTimerReference(TimePast timer) {
        m_timePasted = timer;
    }

    public void Clear() {
        m_sequence.Clear();
        m_timedSequence.Clear();

    }
}


[System.Serializable]
public class RotationSequence {
    public List<RotationTypeShort> m_rotations = new List<RotationTypeShort>();
    public string m_sequenceAsString="";
    public void Add(RotationTypeShort rotation)
    {
       m_rotations.Add(rotation);
        string acro = RubikCube.ConvertAcronymShortToString(rotation);
      m_sequenceAsString += m_sequenceAsString.Length==0? acro: " "+acro;
    }
    public void Clear()
    {
        m_rotations.Clear();
        m_sequenceAsString = "";
    }

    public string GetSequenceAsString()
    {
        return m_sequenceAsString;
    }

    internal List<RotationTypeShort> GetAsList()
    {
        List<RotationTypeShort> duplica = new List<RotationTypeShort>();
        for (int i = 0; i < m_rotations.Count; i++)
        {
            duplica.Add(m_rotations[i]);
        }
        return duplica;
    }
}
[System.Serializable]
public class TimedRotationSequence {
    public List<TimedRotation> m_rotations = new List<TimedRotation>();
    public void Add(RotationTypeShort rotation, float time)
    {
        m_rotations.Add(new TimedRotation() { m_rotation = rotation, m_startTimeAssociated = time });
    }
    public void Clear()
    {
        m_rotations.Clear();
    }
}
[System.Serializable]
public class TimedRotation {
    public RotationTypeShort m_rotation;
    public float m_startTimeAssociated;
}
