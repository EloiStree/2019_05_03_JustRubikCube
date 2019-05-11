using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubeRotationInput : MonoBehaviour
{
    public RubikCubeRotation m_rotor;

    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPeriod))
        {
            if (Input.GetKeyDown(KeyCode.Keypad4)) {
                m_rotor.RotateBy90Degree(RubikCubeRotation.EquatorDirection.CounterEquator);
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
                m_rotor.RotateBy90Degree(RubikCubeRotation.EquatorDirection.Equator);
            if (Input.GetKeyDown(KeyCode.Keypad8))
                m_rotor.RotateBy90Degree(RubikCubeRotation.EquatorDirection.CounterMiddle);
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad5))
                m_rotor.RotateBy90Degree(RubikCubeRotation.EquatorDirection.Middle);
            if (Input.GetKeyDown(KeyCode.Keypad7))
                m_rotor.RotateBy90Degree(RubikCubeRotation.EquatorDirection.CounterStanding);
            if (Input.GetKeyDown(KeyCode.Keypad9))
                m_rotor.RotateBy90Degree(RubikCubeRotation.EquatorDirection.Standing);
        }
        else
        {


            if (IsDown(KeyCode.Keypad5, KeyCode.Keypad2))
                m_rotor.SetRotationAxes(1, RubikCubeRotation.ArrowDirection.Down);
            if (Input.GetKey(KeyCode.Keypad8))
                m_rotor.SetRotationAxes(1, RubikCubeRotation.ArrowDirection.Up);


            if (Input.GetKey(KeyCode.Keypad4))
                m_rotor.SetRotationAxes(1, RubikCubeRotation.ArrowDirection.Left);
            if (Input.GetKey(KeyCode.Keypad6))
                m_rotor.SetRotationAxes(1, RubikCubeRotation.ArrowDirection.Right);

            if (Input.GetKey(KeyCode.Keypad9))
                m_rotor.SetRotationAxes(1, RubikCubeRotation.EquatorDirection.Standing);
            if (Input.GetKey(KeyCode.Keypad7))
                m_rotor.SetRotationAxes(1, RubikCubeRotation.EquatorDirection.CounterStanding);

        }

    }

    private static bool IsDown(params KeyCode [] codes)
    {
        for (int i = 0; i < codes.Length; i++)
        {
            if (Input.GetKey(codes[i]))
                return true;
        }
        return false;
    }

    private void OnValidate()
    {
        m_rotor = GetComponent<RubikCubeRotation>();
    }
}
