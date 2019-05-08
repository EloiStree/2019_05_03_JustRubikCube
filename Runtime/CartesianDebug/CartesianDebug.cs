using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartesianDebug : MonoBehaviour {


    public float m_unitSize = 1;
    public float m_centralCrossSize = 0.1f;
    public float m_negatifRepresenation=0.5f;

    void Update () {

        DrawCentralPoint();
        DrawAxes();

	}

    private void DrawAxes()
    {
        Debug.DrawLine(transform.position + transform.up, transform.position + transform.up * -m_negatifRepresenation, Color.green, Time.deltaTime);
        Debug.DrawLine(transform.position + transform.forward, transform.position + transform.forward * -m_negatifRepresenation, Color.blue, Time.deltaTime);
        Debug.DrawLine(transform.position + transform.right, transform.position + transform.right * -m_negatifRepresenation, Color.green, Time.deltaTime);
    }

    private void DrawCentralPoint()
    {   
        Debug.DrawLine(transform.position + transform.up* m_unitSize, transform.position + transform.up * m_centralCrossSize, Color.green, Time.deltaTime);
        Debug.DrawLine(transform.position + transform.forward* m_unitSize, transform.position + transform.forward * m_centralCrossSize, Color.blue, Time.deltaTime);
        Debug.DrawLine(transform.position + transform.right * m_unitSize, transform.position + transform.right * m_centralCrossSize, Color.green, Time.deltaTime);
    }


    public class TransformRedirectio {
        public Transform tracked;
        public Transform cartesianReference;
    }


    public Dictionary<string, Quaternion> m_quaternionStored = new Dictionary<string, Quaternion>();
    public void Set(string name, Quaternion orientation) {

        if (!m_quaternionStored.ContainsKey(name))
            m_quaternionStored.Add(name, Quaternion.identity);
        m_quaternionStored[name] = orientation;
    }

}
