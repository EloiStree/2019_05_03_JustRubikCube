using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryToApplySequence : MonoBehaviour
{
    public RubikCubePointer m_focus;
    public RubikCubeInstance m_rubik;
    public StoredSequence m_storedSequence;
    public Transform m_from;


    public void Update()
    {

            m_rubik = RubikCubeInstance.m_mainCube;
            if (m_rubik == null) return;

            m_storedSequence = m_focus.GetSelectedSequence();
            if (m_storedSequence == null) return;

        bool leftClick = Input.GetMouseButtonDown(0) ;
        bool rightClick= Input.GetMouseButtonDown(1);

        if (leftClick || rightClick )
        {
            if (m_storedSequence.m_sequenceType == RotationPointOfViewType.DefaultCamera)
                m_from = Camera.main.transform;
            else if (m_storedSequence.m_sequenceType == RotationPointOfViewType.PointOfView)
                m_from = m_focus.GetRoot();
            else m_from = null;

            if (m_rubik)
            {


                RotationSequence sequence = new RotationSequence(m_storedSequence.m_sequence);
                  if(rightClick)
                    sequence = new RotationSequence (RubikCube.InverseOf(sequence.GetSequenceAsShort()));
              
                    m_rubik.Rotate(sequence.GetSequenceAsString(), m_from);
            }
        }
    }
}
