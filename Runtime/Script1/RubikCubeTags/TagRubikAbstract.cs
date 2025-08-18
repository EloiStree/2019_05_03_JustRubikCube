using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TagRubikAbstract : MonoBehaviour
{
    public Transform m_root;


    public void Reset()
    {
        m_root = this.transform;
    }

    public Transform GetRoot() { return m_root; }
    public Vector3 GetPosition() { return m_root.position; }
    public Quaternion GetRotation() { return m_root.rotation; }
}
