using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectutility : MonoBehaviour {

    public void SetActiveInverse()
    {
        bool state = gameObject.activeSelf;
        gameObject.SetActive(!state);
    }
	void Destroy () {
        Destroy(this.gameObject);
	}
    void Destroy(float time) {
        Destroy(this.gameObject, time);
    }
	
}
