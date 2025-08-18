using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyEditor : MonoBehaviour {
    
	void Awake () {

#if !UNITY_EDITOR
        DestroyImmediate(gameObject);
#endif
    }
	
}
