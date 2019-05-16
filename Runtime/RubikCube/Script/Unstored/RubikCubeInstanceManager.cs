using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RubikCubeInterface {

}

public class RubikCubeInstanceManager : MonoBehaviour
{
    public RubikCube m_rubikCube;
    public RubikCubeSaveState m_instanceState = new RubikCubeSaveState();

    public void Shuffle(int randomIteration, out RotationSequence sequenceGenerated) {
       sequenceGenerated = RubikCube.GetRandomSequence(randomIteration);
       Shuffle(sequenceGenerated);
    }

    public RubikCubeSaveState GetRubikcubeStateReference()
    {
        return m_instanceState;
    }
    public CubeDirectionalState GetCubeStateReference()
    {
        return m_rubikCube.GetCubeState() ;
    }

    public void Shuffle(string sequence)
    {
        Shuffle(new RotationSequence(sequence));
    }
    public void Shuffle(IEnumerable<RotationTypeShort> sequence)
    {
        Shuffle(new RotationSequence(sequence));
    }
    public void Shuffle(IEnumerable<RotationTypeLong> sequence)
    {
        Shuffle(new RotationSequence(sequence));
    }

    public void Shuffle(RotationSequence sequence, bool useCubeReset=true) {
        if (useCubeReset)
        {
            m_instanceState.m_shuffleSequence = sequence;
            m_rubikCube.ResetInitialState();
            m_rubikCube.AddLocalRotationSequence(sequence);
        }
        else throw new System.NotImplementedException("Not code yet");

    }
    public bool IsCubeRotating() { return m_rubikCube.IsRotating(); }
    public bool IsCubeSolved() { return !IsCubeRotating() && m_rubikCube.IsCubeResolved(); }
    public bool IsCubeShuffledAndSolved() {
        return m_instanceState.HasBeenShuffled() && !IsCubeRotating() && IsCubeSolved() ;
    }
}
