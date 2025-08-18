using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOutEditor : MonoBehaviour {

    private void Awake()
    {
#if UNITY_EDITOR
        Destroy(gameObject);
#endif
    }
}
