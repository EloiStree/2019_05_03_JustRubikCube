using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveAndLoad : MonoBehaviour {

    public RubikCubeInstance m_rubikCube;
    public string m_playPrefsId="PlayerSolutionSaved";
    
    public OnSaveSequence m_onSave;

    public void Save () {
        string sequence = m_rubikCube.GetSequence();
        PlayerPrefs.SetString(m_playPrefsId,sequence);
        m_onSave.Invoke(sequence);

    }

    public void Load() {
        string history = PlayerPrefs.GetString(m_playPrefsId);
        m_rubikCube.SetWithSequence(history);
        m_onSave.Invoke(history);

    }
}
