using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimerAfterShuffle : MonoBehaviour {
    public RubikCubeEngineMono m_rubikCube;
    public Timer m_timer;

    public void  StartTimerShuffled() {
        StartCoroutine(StartTimerWhenShuffled());
    }

    IEnumerator StartTimerWhenShuffled () {
        yield return new WaitForSeconds(0.5f);

        while (m_rubikCube.IsRotating()) {
            yield return new WaitForEndOfFrame();
        }
        m_timer.StartCounting();



    }
	
	void Update () {
		
	}
}
