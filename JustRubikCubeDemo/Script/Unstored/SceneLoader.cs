using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public string m_sceneName="";

	public void LoadCurrentScene () {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	
	public void LoadSelectedScene () {
        SceneManager.LoadScene(m_sceneName);
	}
}
