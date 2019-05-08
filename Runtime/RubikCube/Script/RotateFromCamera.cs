using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFromCamera : MonoBehaviour {

    public enum Direction { Up,Down,Left,Right,Center}

    public float m_rotationSpeed = 90;
    public Vector3 m_direction;
    public Vector3 m_externalDirection;

    public float m_lerpFactor = 3;

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

        if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Keypad3))
            m_direction.y -= 1;
        if (Input.GetKey(KeyCode.Keypad8))
            m_direction.y += 1;
        if (Input.GetKey(KeyCode.Keypad4))
            m_direction.x -= 1;
        if (Input.GetKey(KeyCode.Keypad6))
            m_direction.x += 1;
        if (Input.GetKey(KeyCode.Keypad7))
            m_direction.z -= 1;
        if (Input.GetKey(KeyCode.Keypad9) || Input.GetKey(KeyCode.Keypad1))
            m_direction.z += 1;

        m_direction.x = Mathf.Clamp(m_direction.x, -1f, 1f);
        m_direction.y = Mathf.Clamp(m_direction.y, -1f, 1f);

    
        transform.Rotate(Camera.main.transform.up, m_rotationSpeed * Time.deltaTime * -m_direction.x, Space.World);
        transform.Rotate(Camera.main.transform.right, m_rotationSpeed * Time.deltaTime * m_direction.y, Space.World);
        transform.Rotate(Camera.main.transform.forward, m_rotationSpeed * Time.deltaTime * -m_direction.z, Space.World);


        m_externalDirection = Vector3.zero;
        // m_direction.y = Mathf.Lerp(m_direction.y, 0, Time.deltaTime* m_lerpFactor);
        //m_direction.x = Mathf.Lerp(m_direction.x, 0, Time.deltaTime* m_lerpFactor);
    }
}
