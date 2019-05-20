using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RubikCubeInterface {

}

public class RubikCubeInstance : MonoBehaviour
{
    [SerializeField] RubikCubeEngineMono m_rubikCube;
    [SerializeField] RubikCubeSaveState m_instanceState;

    public void Shuffle(int randomIteration, out RotationSequence sequenceGenerated) {
       sequenceGenerated = RubikCube.GetRandomSequence(randomIteration);
       Shuffle(sequenceGenerated);
    }

    public RubikCubeEngineMono GetRubikCubeUnityRepresentation()
    {
        return m_rubikCube;
    }

    public RubikCubeSaveState GetRubikcubeStateReference()
    {
        return m_instanceState;
    }
    public CubeDirectionalState GetCubeStateReference()
    {
        return m_rubikCube.GetCubeState() ;
    }

   

    public void Rotate(string sequence, Transform pointOfView)
    {
        m_rubikCube.AddRotationSequence(sequence, pointOfView);
    }

    public void Shuffle(string sequence)
    {
        Shuffle(new RotationSequence(sequence));
    }

    //public void Rotate(ArrowDirection direction, TagRubikCubeFace face, Transform pointOfView)
    //{
    //    Rotate(direction, face.m_belongToFace, face.m_faceDirection, pointOfView);
    //}
    public void Rotate(ArrowDirection direction, RubikCubeFace face, RubikCubeFaceDirection faceDirection, Transform pointOfView)
    {
        TagRubikCubeFace tagFace = m_rubikCube.GetMovingFace(face, faceDirection);
        m_rubikCube.RotateFaceFrom(direction, tagFace, pointOfView);
    }

    public void Rotate(RotationTypeShort rotationType, TagRubikCubeFace face, Transform pointOfView)
    {
        throw new ToDo.Later(ToDo.PiorityExplicit.Major);
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


    private void Awake()
    {
        if (gameObject.tag == "MainRubikCube")
            BoardcastAsMainCube();
    }

    public void BoardcastAsMainCube() {
        RubikCubeInstance previous = m_mainCube;
        RubikCubeInstance newCube = this;
        m_mainCube = newCube;
        Component[] objects = GameObject.FindObjectsOfType<Component>();
        for (int i = 0; i < objects.Length; i++)
        {
            IRubikCubeRequired listener = objects[i] as IRubikCubeRequired;
            if (listener!=null) {
                listener.OnNewRubikCubeFocused(previous, newCube);
            }
        }
    }
    public static RubikCubeInstance m_mainCube;
}
