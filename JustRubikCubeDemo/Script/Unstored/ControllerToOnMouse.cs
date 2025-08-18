using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerToOnMouse : MonoBehaviour {

    public string m_name;
    public Controller m_controller;
    public LayerMask m_mask;

    public bool m_on;

    void Update()
    {
        if (m_controller == null)
            m_controller = Controller.FindActiveOne(m_name);
        if (m_controller == null)
            m_controller = Controller.FindActiveOne(m_name.Split(new char[] { ' ', ',' }));
        if (m_controller == null)
            return;
        bool oldValue = m_on;
        m_on = m_controller.IsPressing();

        if (oldValue != m_on ) {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(m_controller.GetPosition(), m_controller.GetFowardDirection()), out hit, float.MaxValue, m_mask)) {
                hit.collider.SendMessage(m_on? "OnMouseDown": "OnMouseUp", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Hey mon ami:"+hit.collider.gameObject, hit.collider.gameObject);
            }
        }
    }
}
