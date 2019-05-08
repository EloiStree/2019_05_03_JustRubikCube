using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikFocusPoint : MonoBehaviour {

    [Header("Params")]
    public Transform m_pointerRotation;
    public LayerMask m_pieceLayer = ~0;

    public Transform m_meshLaser;

    [Header("Debug")]
    public RubikCube m_cubeTargeted;
    public Collider m_collision;
    public RubikCubeSpot m_cubeSpot;
    public RubikPiece m_pieceFocused;
    public RubikCubeFaceInfo m_faceFocused;



    void Update() {

        RaycastHit hit;
        Ray ray = new Ray(m_pointerRotation.position, m_pointerRotation.forward);

        bool isFocusingPiece = Physics.Raycast(ray, out hit, float.MaxValue, m_pieceLayer);
       

        if (isFocusingPiece)
        {
            m_cubeTargeted = hit.collider.GetComponentInParent<RubikCube>();
            m_collision = hit.collider;
            RubikPiece piece = hit.collider.GetComponent<RubikPiece>();
            if (piece != null)
            {
                m_pieceFocused = piece;
            }
            else m_pieceFocused = null;

            RubikCubeFaceInfo face = hit.collider.GetComponent<RubikCubeFaceInfo>();
            if (face != null)
            {
                m_faceFocused = face;
            }
            else m_faceFocused = null;

            RubikCubeSpot spot = hit.collider.GetComponent<RubikCubeSpot>();
            if (spot != null)
            {
                m_cubeSpot = spot;
            }
            else m_cubeSpot = null;
        }
        else
        {
            m_cubeTargeted = null;
            m_pieceFocused = null;
            m_faceFocused = null;
            m_cubeSpot = null;
            m_collision = null;

        }
        float m_laserDistance = 100;
        //Draw ray;
        if (isFocusingPiece) {
            Debug.DrawLine(m_pointerRotation.position, hit.point, Color.green);
            m_laserDistance = Vector3.Distance(m_pointerRotation.position, hit.point);
        }
        else
            Debug.DrawRay(ray.origin, ray.direction*100, Color.red);
       
        Vector3 scale = m_meshLaser.localScale;
        scale.y = m_laserDistance;
        m_meshLaser.localScale = scale;

    }
}
