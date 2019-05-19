using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveAndLoad : MonoBehaviour {

    public RubikCube m_rubikCube;
    public string m_playPrefsId="PlayerSolutionSaved";

    [Header("Debug")]
    public string m_history;

    public OnSaveSequence m_onSave;
    [System.Serializable]
    public class OnSaveSequence : UnityEvent<string> { }
    void Start () {
        m_rubikCube.m_onStartRotating.AddListener(AddToHistory);

    }

    private void AddToHistory(RubikCube.LocalRotationRequest request)
    {
        m_history +=" "+ RubikCube.ConvertFaceRotationToString(request.m_faceToRotate, request.m_clockWise);
    }

    public void Save () {
        PlayerPrefs.SetString(m_playPrefsId, m_history);
        m_onSave.Invoke(m_history);

    }

    public void Load() {
        m_history = "";
        string history = PlayerPrefs.GetString(m_playPrefsId);
        m_rubikCube.ResetInitialState();
        m_rubikCube.AddLocalRotationSequence(history);
    }
}
