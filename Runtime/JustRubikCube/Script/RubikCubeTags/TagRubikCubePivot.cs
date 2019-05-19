using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagRubikCubePivot : TagRubikAbstract
{
    [SerializeField] RubikCubePivotable m_pivotType;


    public TagRubikCube m_linkedCube;

    public void OnValidate()
    {
        RefreshLink();
    }

    private void RefreshLink()
    {


        if (m_root == null)
            m_root = this.transform;
        if (m_linkedCube == null)
            m_linkedCube = this.GetComponentInParent<TagRubikCube>();
    }
    public new void Reset()
    {
        base.Reset();
        RefreshLink();
        AutoJoinByName();
    }
    private void AutoJoinByName()
    { }
    
}
