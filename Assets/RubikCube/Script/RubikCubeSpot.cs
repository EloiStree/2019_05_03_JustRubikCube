using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

//[ExecuteInEditMode]
public class RubikCubeSpot : MonoBehaviour {

    public RubikCubePivotable [] m_faces;
    public Transform m_root;
    public float m_range=1;
    public bool m_useDebugDraw;
	void OnGUI ()
    {
        if(m_useDebugDraw)
        DebugUtility.DrawCross(m_root, m_range, Color.cyan);
    }

   

    public void Reset()
    {
        m_root = this.transform;
    }
}


public class DebugUtility {
    public static void DrawCross(Transform position, float range, Color color, float time=0)
    {
        Debug.DrawLine(position.position + position.up * -range, position.position + position.up * range, color, time==0?Time.deltaTime:time);
        Debug.DrawLine(position.position + position.right * -range, position.position + position.right * range, color, time == 0 ? Time.deltaTime : time);
        Debug.DrawLine(position.position + position.forward * -range, position.position + position.forward * range, color, time == 0 ? Time.deltaTime : time);
    }
}