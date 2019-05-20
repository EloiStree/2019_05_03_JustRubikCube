using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredSequence : MonoBehaviour
{
    public string m_sequence;
    public RotationPointOfViewType m_sequenceType = RotationPointOfViewType.DefaultCamera;
    public void SetSequence(string sequence) { m_sequence = sequence; }
}

public enum RotationPointOfViewType { Local, DefaultCamera, PointOfView }