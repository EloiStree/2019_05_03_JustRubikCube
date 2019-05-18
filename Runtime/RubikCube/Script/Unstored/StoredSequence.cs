using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredSequence : MonoBehaviour
{
    public string m_sequence;
    public SequenceType m_sequenceType = SequenceType.DefaultCamera;
    public enum SequenceType { Local , DefaultCamera}
    public void SetSequence(string sequence) { m_sequence = sequence; }
}
