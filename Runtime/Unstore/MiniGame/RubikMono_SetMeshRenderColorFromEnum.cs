using UnityEngine;

public class RubikMono_SetMeshRenderColorFromEnum:MonoBehaviour {


    public MeshRenderer[] m_renderer;
    public Material[] m_materials;


    public void Reset() {

        m_renderer = GetComponents<MeshRenderer>();
    }

    public void SetColor(RubikOfficialColor color)
    {

        RubikColor.GetTheColorOfUnity(color, out Color rgbColor);
        SetColor(rgbColor);
    }
    public void SetColor(Color color)
    {
        // Set color on all renderers' materials
        foreach (var renderer in m_renderer)
        {
            foreach (var mat in renderer.materials)
            {
                mat.color = color;
            }
        }

        // Set color on explicitly stored materials
        foreach (var material in m_materials)
        {
            if (material != null)
            {
                material.color = color;
            }
        }
    }

}
