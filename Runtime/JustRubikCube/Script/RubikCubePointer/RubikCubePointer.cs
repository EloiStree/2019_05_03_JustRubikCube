using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikCubePointer : MonoBehaviour
{
    public LayerMask m_layerFilter;

    public Transform m_direction;
    public float m_rayLength = 9999;

    [Header("Raycasted")]
    public RubikCubeInstance m_rubikCubeManager;
    public RubikCube m_firstRubikCube;
    public List<RubikCube> m_rubikCubes = new List<RubikCube>();


    public TagRubikCubeFace m_firstFaceInfo;
    public List<TagRubikCubeFace> m_facesInfo = new List<TagRubikCubeFace>();

    public StoredSequence m_firstStoredSequence;
    public List<StoredSequence> m_storedSequences = new List<StoredSequence>();

    [System.Serializable]
    public class NearestRaycast<T> where T : Component {

        public NearestRaycast() {
            ToRefactor.Later(Piority.B, ToRefactor.PotentialExplicit.ToolboxPackage);
            throw new ToDo.Later(ToDo.PiorityExplicit.Minor);

        }
    }

    public StoredSequence GetSelectedSequence()
    {
        return m_firstStoredSequence;
    }
    public Transform GetRoot()
    {
        return m_direction;
    }


    void Update()
    {
        CastForRubikCubeInformation();

    }

    private void CastForRubikCubeInformation()
    {

        ClearAll();

        RaycastHit[] hits = Physics.RaycastAll(m_direction.position, m_direction.forward, m_rayLength, m_layerFilter);
        foreach (RaycastHit hit in hits)
        {
            TagRubikCubeFace face = hit.collider.GetComponent<TagRubikCubeFace>();
            RubikCube rubikCube = hit.collider.GetComponent<RubikCube>();
            StoredSequence sequence = hit.collider.GetComponent<StoredSequence>();
            if (rubikCube == null)
                rubikCube = hit.collider.GetComponentInParent<RubikCube>();

            if (face != null)
                m_facesInfo.Add(face);
            if (rubikCube != null)
                m_rubikCubes.Add(rubikCube);
            if (sequence != null)
                m_storedSequences.Add(sequence);


            m_firstFaceInfo = GetNearest(m_firstFaceInfo, face);
            m_firstRubikCube = GetNearest(m_firstRubikCube, rubikCube);
            m_firstStoredSequence = GetNearest(m_firstStoredSequence, sequence);

            if (m_firstRubikCube != null)
                m_rubikCubeManager = m_firstRubikCube.GetComponent<RubikCubeInstance>();
        }
        if (hits.Length == 0)
            ClearAll();
    }

    internal Transform GetPointOfView()
    {
        return m_direction;
    }

    public RubikCubeInstance GetCubeManager()
    {
        return m_rubikCubeManager;
    }

    public bool HasFaceSelected()
    {
        return m_firstFaceInfo != null;
    }

    public bool HasCubeSelected()
    {
        return m_firstRubikCube != null;
    }

    private void ClearAll()
    {
        m_rubikCubes.Clear();
        m_facesInfo.Clear();
        m_storedSequences.Clear();
        m_firstFaceInfo = null;
        m_firstRubikCube = null;
        m_rubikCubeManager = null;
        m_firstStoredSequence = null;
    }

    private T GetNearest<T>(T firstRubikCube, T newRubikCube) where T : Component
    {
        if (firstRubikCube == null)
            return newRubikCube;
        if (newRubikCube == null)
            if (firstRubikCube != null)
                return firstRubikCube;
            else
                return null;

        float oldCube = Vector3.Distance(firstRubikCube.transform.position, m_direction.position);
        float newCube = Vector3.Distance(newRubikCube.transform.position, m_direction.position);
        if (newCube < oldCube)
            return newRubikCube;
        else
            return firstRubikCube;
    }

    internal TagRubikCubeFace GetSelectedFace()
    {
        return m_firstFaceInfo;
    }
}
