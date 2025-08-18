using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceHistory : MonoBehaviour
{
    [SerializeField] RubikCubeEngineMono m_rubikCube=null;
    [SerializeField] RotationSequence m_sequence = new RotationSequence();
    [SerializeField] TimedRotationSequence m_timedSequence = new TimedRotationSequence();
    [SerializeField] TimePast m_timePasted;
    void Start()
    {
        m_timePasted.StartTimer();
        m_rubikCube.m_onStartRotating.AddListener(AddToHistory);

    }


    private void AddToHistory(RubikCubeEngineMono.LocalRotationRequest request)
    {
        float time = m_timePasted.GetTimePast();
        RotationTypeShort rotation = RubikCube.ConvertRotationToAcronymShort(request.m_faceToRotate, request.m_clockWise);
        m_sequence.Add(rotation);
        m_timedSequence.Add(rotation, time);
         m_sequence.GetSequenceAsString();
    }

    

    public RubikCubeEngineMono GetLinkedCube() { return m_rubikCube; }
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

    public int Lenght { get { return m_rotations.Count; } }
    public RotationSequence()
    { Clear(); }
    public RotationSequence(string sequence): this(RubikCube.ConvertStringToShorts(sequence))
    {}


    public RotationSequence (IEnumerable<RotationTypeShort> sequence){
        Clear();
        Add(sequence);
    }
    
    public RotationSequence(IEnumerable<RotationTypeLong> sequence)
    {
        Clear();
        foreach (var item in sequence)
        {
            Add(RubikCube.ConvertAsShort(item));
        }
    }

    public List<RotationTypeShort> m_rotations = new List<RotationTypeShort>();
    public RotationTypeShort[] GetSequenceAsShort()
    {
        return m_rotations.ToArray();
    }
    public string GetSequenceAsString(string separation = "")
    {
        string s = "";
        for (int i = 0; i < m_rotations.Count; i++)
        {
            s += (i == 0 ? "" : separation) + RubikCube.ConvertAcronymShortToString(m_rotations[i]);
        }
        return s;
    }
    public RotationTypeLong[] GetSequenceAsLong()
    {
        RotationTypeLong[] result = new RotationTypeLong[m_rotations.Count];
        for (int i = 0; i < m_rotations.Count; i++)
        {

            result[i] = RubikCube.ConvertAcronymShortToLong(m_rotations[i]);
        }
        return result;
    }
    
    public string m_sequenceAsString="";
    private IEnumerator<RotationTypeShort> sequence;

    public virtual void Add(RotationTypeShort rotation)
    {
       m_rotations.Add(rotation);
        string acro = RubikCube.ConvertAcronymShortToString(rotation);
      m_sequenceAsString += m_sequenceAsString.Length==0? acro: " "+acro;
    }
    public void Add(IEnumerable<RotationTypeShort> sequence)
    {
        foreach (var item in sequence)
        {
            Add(item);
        }
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

    public List<RotationTypeShort> GetAsList()
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
public class TimedRotationSequence : RotationSequence
{

    public List<TimedRotation> m_timedRotation = new List<TimedRotation>();
    public  void Add(RotationTypeShort rotation, float time)
    {
        base.Add(rotation);
        m_timedRotation.Add(new TimedRotation() { m_rotation = rotation, m_startTimeAssociated = time });
    }

    public override void Add(RotationTypeShort rotation)
    {
        throw new Exception("You cant add a rotation to time rotation sequence without the time");
    }

    public new void Clear()
    {
        base.Clear();
        m_timedRotation.Clear();
    }
}
[System.Serializable]
public class TimedRotation {
    public RotationTypeShort m_rotation;
    public float m_startTimeAssociated;
}
