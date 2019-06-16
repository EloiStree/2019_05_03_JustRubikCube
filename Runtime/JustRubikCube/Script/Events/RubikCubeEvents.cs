using UnityEngine.Events;

[System.Serializable]
public class RotationEvent : UnityEvent<RubikCubeEngineMono.LocalRotationRequest>{}

[System.Serializable]
public class CubeStateChangeEvent : UnityEvent<CubeDirectionalState>{}
[System.Serializable]
public class RubikCubeStateChangeEvent : UnityEvent<RubikCubeSaveState> { }


[System.Serializable]
public class OnRubikCubeUsed : UnityEvent<RubikCubeEngineMono> { }

[System.Serializable]
public class OnSaveSequence : UnityEvent<string> { }