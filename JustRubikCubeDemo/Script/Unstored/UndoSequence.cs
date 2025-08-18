using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///BAD CODE. I was super sleepy when I write it. Should diserve 3 hours of concetrated thinking to be bug free.
public class UndoSequence : MonoBehaviour
{
    public RubikCubeEngineMono m_rubikCube;
    public SequenceHistory m_rotationsHistory;
    public List<RotationTypeShort> m_incomingUndo = new List<RotationTypeShort>();
    public List<RotationTypeShort> m_undoTrack = new List<RotationTypeShort>();
    public bool m_waitingUndo;
    public RotationTypeShort m_lastUndo;
    public bool m_waitingUnUndo;
    public RotationTypeShort m_lastUnUndo;
    public int m_index;
    public void Start()
    {
        m_rubikCube.m_onStartRotating.AddListener(CheckIfItIsUndoRotation);
    }

    private void CheckIfItIsUndoRotation(RubikCubeEngineMono.LocalRotationRequest arg0)
    {
        RotationTypeShort received =
            RubikCube.ConvertRotationToAcronymShort(
                arg0.m_faceToRotate, arg0.m_clockWise);
        //if received don't correspond at the undow combo

        if (m_waitingUndo && received != m_lastUndo)
            BreakUndo();
        else if (m_waitingUndo && !IsAtMaximumUndo())
            IncrementUndoIndex();
        else if (m_waitingUnUndo && received != m_lastUnUndo)
            BreakUndo();
        else if (m_waitingUnUndo && !IsAtMinimumUnUndo())
            DecreseUndoIndex();
        else BreakUndo();

        m_waitingUndo = false;
        m_waitingUnUndo = false;
    }

    private void DecreseUndoIndex()
    {
        m_index--;
        if (m_index <= 0)
            BreakUndo();
    }

    private void IncrementUndoIndex()
    {
        m_index++;
    }

    private void BreakUndo()
    {
        m_index = 0;
        m_undoTrack.Clear();
        m_incomingUndo.Clear();
    }

    public bool HasUndoStarted() { return m_incomingUndo.Count != 0; }
    public bool IsAtMaximumUndo() { return m_index >= m_incomingUndo.Count; }
    public bool IsAtMinimumUnUndo() { return m_index <= 0; }

    public bool TryToHaveUndoRotation(int undoIndex, out RotationTypeShort rotation) {
        rotation = RotationTypeShort.B;

        int incomingLenght = m_incomingUndo.Count;
        if (undoIndex >= incomingLenght)
            return false;

        rotation = m_incomingUndo[undoIndex];
        return true;
    }
    public bool TryToHaveUnUndoRotation(int undoIndex, out RotationTypeShort rotation)
    {
        rotation = RotationTypeShort.B;

        int incomingLenght = m_incomingUndo.Count;
        if (undoIndex > incomingLenght || undoIndex<=0)
            return false;

       // rotation = RubikCube.GetInverseOf(m_incomingUndo[undoIndex - 1]);
        rotation =m_incomingUndo[undoIndex - 1];
        return true;
    }

    // history: l r r b u d ui di |
    // Undo 3 : l r r b u d ui di | d u di _
    // UnUndo 3 : l r r b u d ui di | d u di | d ui di
    public void Undo()
    {
      
        if (!HasUndoStarted()) {
            BreakUndo();
            m_incomingUndo = 
                m_rotationsHistory.GetSequence().GetAsList();
            m_incomingUndo.Reverse();
        }

        if (IsAtMaximumUndo())
            return;

        if (TryToHaveUndoRotation(m_index, out m_lastUndo))
        {
            m_lastUndo = RubikCube.GetInverseOf(m_lastUndo);
            m_waitingUndo = true;
            m_rubikCube.AddLocalRotationSequence(m_lastUndo);
        }

        
    }
    public void UnUndo() {


        Debug.Log("UnUndo");
        //if (IsAtMinimumUnUndo())
        //    return;

        if (TryToHaveUnUndoRotation(m_index, out m_lastUnUndo))
        {
            Debug.Log("UnUndoB");
            m_waitingUnUndo = true;
            m_rubikCube.AddLocalRotationSequence(m_lastUnUndo);
        }
        else BreakUndo();
       
        

    }

}
