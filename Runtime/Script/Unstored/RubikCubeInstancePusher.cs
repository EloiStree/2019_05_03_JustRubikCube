using UnityEngine;

public class RubikCubeInstancePusher :MonoBehaviour{

    public RubikCubeInstance I() {

        return RubikCubeInstance.Get();
    }
    public void Shuffle(string sequence) => I()?.Shuffle(10, out _);
    public void PushSequenceAsLocal(string sequence) => I()?.AddLocalSequence(sequence);
    public void PushSequenceMainCamera(string sequence) => I()?.AddSequence(sequence, Camera.main?.transform);
    public void PushSequenceCurrentTransform(string sequence) => I()?.AddSequence(sequence,this.transform);
    public void PushSequenceFrom(string sequence, Camera givenCamera) => I()?.AddSequence(sequence, givenCamera?.transform);
    public void PushSequenceFrom(string sequence, Transform givenTransform) => I()?.AddSequence(sequence, givenTransform);
    public void ResetCubeToEmptyState(string sequence) => I()?.ResetInitialState();

    public void PushSequence(StoredSequence sequence) { 
    
        if (sequence == null) return;

        string text = sequence.m_sequence;
        RotationPointOfViewType type = sequence.m_sequenceType;

        switch (type) {
            case RotationPointOfViewType.Local:
                PushSequenceAsLocal(text); break;
            case RotationPointOfViewType.DefaultCamera:
                PushSequenceMainCamera(text); break;
            case RotationPointOfViewType.PointOfView:
                PushSequenceMainCamera(text); break;

        }

    }

}

