using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    internal void AddStateChangeListener(Action<RubikCubeSaveState> addToHistory)
    {
        throw new NotImplementedException();
    }

    public RubikCubeEngineMono GetRubikCubeUnityRepresentation()
    {
        return m_rubikCube;
    }

    internal string GetSequence()
    {
        return m_rubikCube.GetSequence().GetSequenceAsString();
    }

    public RubikCubeSaveState GetRubikcubeStateReference()
    {
        return m_instanceState;
    }

    internal void SetWithSequence(string sequence)
    {
        m_rubikCube.SetWithSequence( sequence);
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
        throw new ToDo.LaterException(ToDo.PiorityExplicit.Major);
    }


    [SerializeField] RotationSequence m_shuffleSequence;

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
            m_shuffleSequence = sequence;
            m_rubikCube.SetWithSequence(sequence);
        }
        else throw new System.NotImplementedException("Not code yet");

    }
    public bool IsCubeRotating() { return m_rubikCube.IsRotating(); }
    public bool IsCubeSolved() { return !IsCubeRotating() && m_rubikCube.IsCubeResolved(); }
    public bool IsCubeShuffledAndSolved() {
        return m_shuffleSequence.Lenght>0 && !IsCubeRotating() && IsCubeSolved() ;
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

