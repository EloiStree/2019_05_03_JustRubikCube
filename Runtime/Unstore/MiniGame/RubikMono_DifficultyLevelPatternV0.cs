using UnityEngine;

public class RubikMono_DifficultyLevelPatternV0 : MonoBehaviour
{
    public RubikMono_FaceColorStateFromTag m_objective;
    public string[] m_levels = new string[] { "ggyggyggy", "ororororo" };

    public int m_levelIndex;
    public string m_colorLetter = "woybrg";

    [ContextMenu("Reset Index To Zero")]
    public void ResetIndexToZero()
    {
        m_levelIndex = 0;
    }

    [ContextMenu("Reset And Load Next Level")]
    public void ResetIndexToZeroAndLoadNextLevel()
    {
        m_levelIndex = 0;
        NextLevel();
    }

    [ContextMenu("Next Level")]
    public void NextLevel()
    {
        string level;

        if (m_levelIndex < m_levels.Length)
        {
            level = m_levels[m_levelIndex];
        }
        else
        {
            level = "";
            for (int i = 0; i < 9; i++)
            {
                char c = m_colorLetter[UnityEngine.Random.Range(0, m_colorLetter.Length)];
                level += c;
            }
        }

        if (level.Length > 0)
        {
            m_objective.m_faceNO.SetTheColor(GetColorFor(level[0]));
            m_objective.m_faceN.SetTheColor(GetColorFor(level[1]));
            m_objective.m_faceNE.SetTheColor(GetColorFor(level[2]));
            m_objective.m_faceO.SetTheColor(GetColorFor(level[3]));
            m_objective.m_faceC.SetTheColor(GetColorFor(level[4]));
            m_objective.m_faceE.SetTheColor(GetColorFor(level[5]));
            m_objective.m_faceSO.SetTheColor(GetColorFor(level[6]));
            m_objective.m_faceS.SetTheColor(GetColorFor(level[7]));
            m_objective.m_faceSE.SetTheColor(GetColorFor(level[8]));
        }

        m_levelIndex++;
    }

    private RubikOfficialColor GetColorFor(char v)
    {
        switch (v)
        {
            case 'y': return RubikOfficialColor.Yellow;
            case 'r': return RubikOfficialColor.Red;
            case 'g': return RubikOfficialColor.Green;
            case 'b': return RubikOfficialColor.Blue;
            case 'o': return RubikOfficialColor.Orange;
            case 'w': return RubikOfficialColor.White;
            default: return RubikOfficialColor.White;
        }
    }
}
