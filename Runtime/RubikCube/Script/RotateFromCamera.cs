using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFromCamera : MonoBehaviour {

    public enum Direction { Up,Down,Left,Right,Center}

    public Transform m_toAffect;
    public float m_rotationSpeed = 90;
    public Vector3 m_direction;
    public Vector3 m_externalDirection;
    

    public void SetHorizontal(float speed)
    {
        m_direction.x = speed;
    }
    public void SetVertical(float speed)
    {

        m_direction.y = speed;
    }
 

	void Update ()
    {
        m_direction = m_externalDirection;
        m_direction.x = Mathf.Clamp(m_direction.x, -1f, 1f);
        m_direction.y = Mathf.Clamp(m_direction.y, -1f, 1f);


        m_toAffect.Rotate(Camera.main.transform.up, m_rotationSpeed * Time.deltaTime * -m_direction.x, Space.World);
        m_toAffect.Rotate(Camera.main.transform.right, m_rotationSpeed * Time.deltaTime * m_direction.y, Space.World);
        m_toAffect.Rotate(Camera.main.transform.forward, m_rotationSpeed * Time.deltaTime * -m_direction.z, Space.World);


        m_externalDirection = Vector3.zero;
    }
}
