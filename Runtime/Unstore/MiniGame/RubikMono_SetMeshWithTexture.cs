using UnityEngine;

public class  RubikMono_SetMeshWithTexture:MonoBehaviour
{

    public MeshRenderer[] m_renderer;
    public Material[] m_materials;


    public void Reset()
    {

        m_renderer = GetComponents<MeshRenderer>();
    }

    public void SetColor(Texture source)
    {
        // Set color on all renderers' materials
        foreach (var renderer in m_renderer)
        {
            renderer.material.mainTexture = source;
        }

        // Set color on explicitly stored materials
        foreach (var material in m_materials)
        {
            if (material != null)
            {
                material.mainTexture = source;
            }
        }
    }



}
