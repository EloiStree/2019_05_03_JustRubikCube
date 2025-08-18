using System;
using UnityEngine;


public class TagRubikCubeFaceMatDrawer : MonoBehaviour 
{
    public TagRubikCube m_toAffectFaces;
    public Material m_materialToCopy;
    public bool m_setRandomColorAtStart = true;
    public bool m_setDefaultColorAtStart = true;
    public float m_transparence = 0.8f;

    private void Awake()
    {
        foreach (var face in m_toAffectFaces.m_faces) { 
        
            MeshRenderer meshRenderer = face.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null) { 
            
                Material copy = new Material(meshRenderer.material);
                meshRenderer.material = copy;
                meshRenderer.material.color = new Color(0, 0, 0, 0);
            }
           
        }
        if (m_setRandomColorAtStart) {
            SetWithRandomColor();
        }
        if (m_setDefaultColorAtStart) {
            SetWithDefaultColor();
        }
    }

    [ContextMenu("Set random color")]
    public void SetWithRandomColor()
    {

        foreach (var face in m_toAffectFaces.m_faces)
        {

            MeshRenderer meshRenderer = face.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, m_transparence);
            }

        }

    }
    [ContextMenu("Set Default color")]
    public void SetWithDefaultColor()
    {

        foreach (var face in m_toAffectFaces.m_faces)
        {

            MeshRenderer meshRenderer = face.GetComponentInChildren<MeshRenderer>();
            Color color = Color.black;
            switch(face.m_belongToFace)
            {
                case RubikCubeFace.Face: color = Color.green; break;
                case RubikCubeFace.Left: color = Color.white; break;
                case RubikCubeFace.Right: color = Color.yellow; break;
                case RubikCubeFace.Back: color = Color.blue; break;
                case RubikCubeFace.Up: color = Color.red; break;
                case RubikCubeFace.Down: color = Color.orange; break;

            }
            if (meshRenderer != null)
            {
                meshRenderer.material.color = new Color(color.r, color.g, color.b, m_transparence);
            }

        }

    }

    public void SetColorFor(RubikCubeFace faceType, RubikCubeFaceDirection direction, Color color)
    {
        foreach (TagRubikCubeFace face in m_toAffectFaces.m_faces)
        {
            if (face.m_faceDirection == direction && face.m_belongToFace == faceType)
            {
                MeshRenderer meshRenderer = face.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.material.color = new Color(color.r, color.g, color.b, m_transparence);
                }
            }
        }

    }
    public void SetColorFor(RubikCubeFace faceType,Color color)
    {
        foreach (TagRubikCubeFace face in m_toAffectFaces.m_faces)
        {
            if (face.m_belongToFace == faceType)
            {
                MeshRenderer meshRenderer = face.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.material.color = new Color(color.r, color.g, color.b, m_transparence);
                }
            }
        }
    }
}
