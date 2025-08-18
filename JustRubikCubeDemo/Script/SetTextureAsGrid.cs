using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextureAsGrid : MonoBehaviour
{
    public Renderer[] m_affected;
    public Texture2D m_targetTexture;
    public int m_pixel_X_12=10;
    [Header("Debug")]
     [SerializeField]int heightSection;
    [SerializeField] int widthSection;     

    //TODO: All the color code here should be replace by a sharder.
    void Awake()
    {
        m_targetTexture = new Texture2D(12* m_pixel_X_12 , 12 * m_pixel_X_12 );
        for (int x = 0; x < m_targetTexture.width; x++)
        {
            for (int y = 0; y < m_targetTexture.height; y++)
            {
                m_targetTexture.SetPixel(x, y, Color.black);
            }
        }
        foreach (Renderer renderer in m_affected)
        {
            renderer.material = new Material(renderer.material);
            renderer.material.SetTexture("_MainTex", m_targetTexture);
        }
       // m_materialReference.SetTexture("_MainTex", m_targetTexture);
    }


    public void SetColor(Color color, int x, int y, int size = 12, bool applyDirectly =true) {
        
        int heightSection = m_targetTexture.height/size;
        int widthSection = m_targetTexture.width/size;
        int iStart=x *widthSection, jStart = y * heightSection;
        int iEnd=((x +1) *widthSection) - 1, jEnd = ((y + 1) * heightSection) -1;

        for (int i = iStart; i <=iEnd ; i++)
        {
            for (int j =jStart; j <=jEnd; j++)
            {
                if(j ==jStart || i ==iStart || j== jEnd || i == iEnd)
                    m_targetTexture.SetPixel(i, j, Color.black);
                else
                    m_targetTexture.SetPixel(i, j, color);
            }
        }
        if (applyDirectly)
            m_targetTexture.Apply();

    }
    public void Apply() {

        m_targetTexture.Apply();

    }
}
