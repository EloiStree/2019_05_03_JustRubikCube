using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Undo : MonoBehaviour
{
    public UndoSequence m_undo;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            m_undo.Undo();
        if (Input.GetKeyDown(KeyCode.A))
            m_undo.UnUndo();
    }
}
