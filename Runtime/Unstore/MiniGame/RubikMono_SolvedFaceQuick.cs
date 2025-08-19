using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class RubikMono_SolvedFaceQuick : MonoBehaviour
{

    public RubikMono_FaceColorStateFromTag m_player;
    public RubikMono_FaceColorStateFromTag m_objective;

    public float m_timeToCompleteGame = 60 * 3;
    public float m_timeBeforeChangingColor = 1;

    public UnityEvent m_onRequestNewColorObjective;
    public UnityEvent<int> m_onScoreChanged;
    public UnityEvent m_onGameRestart;
    public UnityEvent<int> m_onGameOverWithScore;
    public UnityEvent<float> m_onUpdateTimeLeft;


    public int m_score;
    public float m_countdownTime;
     

    public bool IsPlayerSameAsTarget()
    {

        if (m_player.m_faceN .m_color != m_objective.m_faceN .m_color) return false;
        if (m_player.m_faceNO.m_color != m_objective.m_faceNO.m_color) return false;
        if (m_player.m_faceO .m_color != m_objective.m_faceO .m_color) return false;
        if (m_player.m_faceSO.m_color != m_objective.m_faceSO.m_color) return false;
        if (m_player.m_faceS .m_color != m_objective.m_faceS .m_color) return false;
        if (m_player.m_faceSE.m_color != m_objective.m_faceSE.m_color) return false;
        if (m_player.m_faceE .m_color != m_objective.m_faceE .m_color) return false;
        if (m_player.m_faceNE.m_color != m_objective.m_faceNE.m_color) return false;
        if (m_player.m_faceC.m_color != m_objective.m_faceC.m_color) return false;
        return true;
    }

    private void Awake()
    {
        RestartGame();
    }

    public void Update()
    {
        float previous = m_countdownTime;
        m_countdownTime -=Time.deltaTime;

        if (m_countdownTime <= 0 && previous > 0) {

            m_onGameOverWithScore.Invoke(m_score);
        }

        if (m_countdownTime < 0 ) { 
        m_countdownTime = 0;

        }
        m_onUpdateTimeLeft.Invoke(m_countdownTime);
    }



    [ContextMenu("Restart the game")]
    public void RestartGame()
    {
        m_score = 0;
        m_countdownTime = m_timeToCompleteGame;
        m_onGameRestart.Invoke();
        m_onScoreChanged.Invoke(m_score);
        m_onUpdateTimeLeft.Invoke(m_countdownTime);
    }

    public void OnEnable()
    {

        StartCoroutine(ManageTimeAndCheckPoint());
    }

    private IEnumerator ManageTimeAndCheckPoint()
    {
        while (true) {

            yield return new WaitForSeconds(0.1f);
            bool isFaceTheSame = IsPlayerSameAsTarget();
            if (isFaceTheSame) {

                m_score++;
                m_onScoreChanged.Invoke(m_score);
                yield return new WaitForSeconds(m_timeBeforeChangingColor);
                m_onRequestNewColorObjective.Invoke();
            }
        }
    }
}
