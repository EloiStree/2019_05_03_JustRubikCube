using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class RubikCubePointer : MonoBehaviour
{
    public LayerMask m_layerFilter;

    public Transform m_direction;
    public float m_rayLength = 9999;

    [Header("UI Raycaster")]
    GraphicRaycaster [] m_raycasters;
    PointerEventData m_pointerEventData;
    EventSystem m_eventSystem;

    void Awake()
    {
        m_raycasters = GameObject.FindObjectsByType< GraphicRaycaster>(FindObjectsSortMode.None);
        m_eventSystem = GetComponent<EventSystem>();
    }




    [Header("Raycasted")]
    public bool m_useUIRaycasting=true;
    public RubikCubeInstance m_rubikCubeManager;
    public RubikCubeEngineMono m_firstRubikCube;
    public List<RubikCubeEngineMono> m_rubikCubes = new List<RubikCubeEngineMono>();


    public TagRubikCubeFace m_firstFaceInfo;
    public List<TagRubikCubeFace> m_facesInfo = new List<TagRubikCubeFace>();

    public StoredSequence m_firstStoredSequence;
    public List<StoredSequence> m_storedSequences = new List<StoredSequence>();

    [System.Serializable]
    public class NearestRaycast<T> where T : Component {

        public NearestRaycast() {
            ToRefactor.Later(Piority.B, ToRefactor.PotentialExplicit.ToolboxPackage);
            throw new ToDo.LaterException(ToDo.PiorityExplicit.Minor);

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
    List<RaycastResult> uiHits = new List<RaycastResult>();
    RaycastHit[] physicHits = new RaycastHit[0];
    public List<GameObject> objHits = new List<GameObject>();

    private void CastForRubikCubeInformation()
    {

        ClearAll();
        objHits.Clear();
        if (m_useUIRaycasting)
        {
            CastUI();
            Add(ref objHits, uiHits);
        }
        physicHits = Physics.RaycastAll(m_direction.position, m_direction.forward, m_rayLength, m_layerFilter);
        Add(ref objHits, physicHits);

        foreach (GameObject hit in objHits)
        {
            TagRubikCubeFace face = hit.GetComponent<TagRubikCubeFace>();
            RubikCubeEngineMono rubikCube = hit.GetComponent<RubikCubeEngineMono>();
            StoredSequence sequence = hit.GetComponent<StoredSequence>();
            if (rubikCube == null)
                rubikCube = hit.GetComponentInParent<RubikCubeEngineMono>();

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
        if (physicHits.Length == 0 && uiHits.Count==0)
            ClearAll();
    }

    private void Add(ref List<GameObject> objHits, RaycastHit[] physicHits)
    {
        foreach (var item in physicHits)
        {
            objHits.Add( item.collider.gameObject);
        }
    }

    private void Add(ref List<GameObject> objHits, List<RaycastResult> uiHits)
    {
        foreach (RaycastResult item in uiHits)
        {
            objHits.Add(item.gameObject);

        }
    }
    

    public int m_rayCastCount;
    public int m_uiFound;
    private void CastUI()
    {
        uiHits.Clear();
        m_pointerEventData = new PointerEventData(m_eventSystem);
        m_pointerEventData.position = Camera.main.WorldToScreenPoint(m_direction.position);
       
        for (int i = 0; i < m_raycasters.Length; i++)
        {
            List<RaycastResult> r = new List<RaycastResult>();
            m_raycasters[i].Raycast(m_pointerEventData, r);
            uiHits.AddRange(r);
        }
        m_rayCastCount = m_raycasters.Length;
            m_uiFound= uiHits.Count;
    }

    public Transform GetPointOfView()
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

    public TagRubikCubeFace GetSelectedFace()
    {
        return m_firstFaceInfo;
    }
}
