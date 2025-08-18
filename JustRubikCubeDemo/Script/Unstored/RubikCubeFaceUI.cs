using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubikCubeFaceUI : MonoBehaviour {

    public RubikCubeFace m_face;
    public RubikCubeFaceDirection m_direction;
    public Image m_image;
    


    public void SetColor(Color color) {
        m_image.color = color;
    }

    public void SetRandomColor()
    {
        RubikCubeFace face = (RubikCubeFace) (UnityEngine.Random.Range(0, 6));
        m_image.color = RubikCube.GetColor(RubikCube.GetColor(face));
    }

    void Awake () {
        SetColor(RubikCube.GetColor(RubikCube.GetColor(m_face)));
	}

    public void OnValidate()
    {
        SetColor(RubikCube.GetColor(RubikCube.GetColor(m_face)));
    }

    public void Reset() {
        if(m_image==null)
        m_image = GetComponent<Image>();
    }

}
