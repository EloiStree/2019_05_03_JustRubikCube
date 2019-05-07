using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class RubikCubeResolvedState : MonoBehaviour {


    public RubikCube m_rubikCube;

    [Header("Debug")]
    public bool m_isResolved;
    public float m_isResolvedPourcent;
    public UnityEvent m_onCubeResolved;
    public PourcentFinishEvent m_onCubePourcentChanged;
    [System.Serializable]
    public class PourcentFinishEvent : UnityEvent<float> { }


    private float m_lastPourcent;

    public void CheckIfCubeResolved()
    {
        float pourcentResolved;
        bool isResolved = IsResolved(out pourcentResolved);
        if (!m_isResolved && isResolved)
            m_onCubeResolved.Invoke();
        m_isResolved = isResolved;
        m_isResolvedPourcent = pourcentResolved;

        if (m_lastPourcent != pourcentResolved) {

            m_lastPourcent = pourcentResolved;
            m_onCubePourcentChanged.Invoke(m_lastPourcent);
        }

    }
   
	void Update () {
        CheckIfCubeResolved();

    }

    internal bool IsResolved(out float pourcent)
    {
        RubikPiece[] pieces = m_rubikCube.GetPieces();
         pourcent =0;
        int countRightPosition =0 ;
        bool isResolved = true;
        foreach (RubikCube.PieceInitialState initState in m_rubikCube.GetInitialSpots())
        {
            bool isPieceAtRightPosition = IsRightPieceClosestOfGoodPosition(initState, pieces);
            if (isPieceAtRightPosition) {
                countRightPosition++;
            }
            else isResolved = false;
        }
        pourcent = (float)countRightPosition / (float)(pieces.Length);
        return isResolved;
    }

    private bool IsRightPieceClosestOfGoodPosition(RubikCube.PieceInitialState initState, RubikPiece[] pieces)
    {
        RubikPiece closestPiece = GetClosestOf(initState.m_initialPosition, pieces);
        return initState.m_linkedPiece == closestPiece ;
    }

    private RubikPiece GetClosestOf(Vector3 m_initialPosition, RubikPiece[] pieces)
    {

        RubikPiece bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = m_initialPosition;

            foreach (RubikPiece potentialTarget in pieces)
            {
                float dSqrToTarget = Vector3.Distance(potentialTarget.m_root.localPosition , currentPosition);
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            return bestTarget;
        
    }
}
