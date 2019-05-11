using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RubikCubeRotation : MonoBehaviour
{

   

    public Transform m_affected;

    public void RotateBy90Degree(EquatorDirection rotation)
    {
            RotateWithAngle( 90, rotation);
        
    }

    public Transform m_pointOfView;
    public float m_rotationSpeed = 90;
    public float m_rotationSpeedForAngles = 270;
    public bool m_tryToUseCameraAtStart = true;


    public Vector3 m_direction;
    public Vector3 m_stackRotation;


    public void UseMainCamera() {
        if (Camera.main)
            m_pointOfView = Camera.main.transform;
    }
    public void UsePointOfView(Transform pointOfView) {
        m_pointOfView = pointOfView;
    }

    public void Reset()
    {
        m_affected = transform;
        if (Camera.main)
            m_pointOfView = Camera.main.transform;
    }


    public void SetSpeed(float speed)
    {
        m_rotationSpeed = speed;
    }
  
    public void SetRotationAxes(float value, ArrowDirection direction) {
        value = Mathf.Clamp(value, 0, 1);
        switch (direction)
        {
            case ArrowDirection.Up:
                m_direction.y = value;
                break;
            case ArrowDirection.Down:
                m_direction.y = -value;
                break;
            case ArrowDirection.Right:
                m_direction.x = value;
                break;
            case ArrowDirection.Left:
                m_direction.x = -value;
                break;
            default:
                break;
        }
    }
    public void SetRotationAxes(float value, EquatorDirection direction) {
        value = Mathf.Clamp(value, 0, 1);
        switch (direction)
        {
            case EquatorDirection.Equator:
                m_direction.y = +value;
                break;
            case EquatorDirection.CounterEquator:
                m_direction.y = -value;
                break;
            case EquatorDirection.Middle:
                m_direction.x = value;
                break;
            case EquatorDirection.CounterMiddle:
                m_direction.x = -value;
                break;
            case EquatorDirection.Standing:
                m_direction.z = value;
                break;
            case EquatorDirection.CounterStanding:
                m_direction.z = -value;
                break;
            default:
                break;
        }

    }
    public void StopRotating(ArrowDirection direction)
    {
        SetRotationAxes(0f, direction);
    }
    public void StopRotating(EquatorDirection direction) {

        SetRotationAxes(0f, direction);
    }

    private void Awake()
    {
        if (m_tryToUseCameraAtStart)
            UseMainCamera();
    }

    void Update()
    {

        m_direction.x = Mathf.Clamp(m_direction.x, -1f, 1f);
        m_direction.y = Mathf.Clamp(m_direction.y, -1f, 1f);
        m_direction.z = Mathf.Clamp(m_direction.z, -1f, 1f);

        if (m_pointOfView) {
            RotateWithDeltaTIme(m_pointOfView);
            SpendRotationStack(m_pointOfView);
        }
        else
        {
            RotateWithDeltaTIme(m_affected);
            SpendRotationStack(m_affected);

        }
        m_direction = Vector3.zero;
    }

    private void SpendRotationStack(Transform t)
    {
        m_stackRotation.x = RotateOnAxisReturnRest( m_stackRotation.x, t.right);
        m_stackRotation.y = RotateOnAxisReturnRest( m_stackRotation.y, t.up);
        m_stackRotation.z = RotateOnAxisReturnRest( m_stackRotation.z, t.forward);
    }

    private float RotateOnAxisReturnRest(float angle, Vector3 axi)
    {
        if (angle == 0f)
            return 0f;

        bool isPositif = angle > 0f;
        float angleToRemove = m_rotationSpeedForAngles * Time.deltaTime;
        if (Mathf.Abs(angle) < Mathf.Abs(angleToRemove))
            angleToRemove =  Mathf.Abs(angle);

        if (angle < 0) {
            angleToRemove *= -1f;
        }
        m_affected.Rotate(axi, -angleToRemove, Space.World);
        return angle - angleToRemove;
    }

    private void RotateWithDeltaTIme(Transform t)
    {
        if (m_affected && t)
        {
            m_affected.Rotate(t.up, m_rotationSpeed * Time.deltaTime * -m_direction.x, Space.World);
            m_affected.Rotate(t.right, m_rotationSpeed * Time.deltaTime * m_direction.y, Space.World);
            m_affected.Rotate(t.forward, m_rotationSpeed * Time.deltaTime * -m_direction.z, Space.World);



        }
    }
    private void RotateWithAngle( float angle , EquatorDirection rotation )
    {
        Debug.Log(string.Format("{0},{1}", angle, rotation));
        if (m_affected!=null )
        {
            if (EquatorDirection.Equator == rotation)
                m_stackRotation.y += angle;
            if (EquatorDirection.CounterEquator == rotation)
                m_stackRotation.y += -angle;

            if (EquatorDirection.Middle == rotation)
                m_stackRotation.x += angle;
            if (EquatorDirection.CounterMiddle == rotation)
                m_stackRotation.x += -angle;
          
            if (EquatorDirection.Standing == rotation)
                m_stackRotation.z += angle;
            if (EquatorDirection.CounterStanding == rotation)
                m_stackRotation.z += -angle;
        }
    }
}