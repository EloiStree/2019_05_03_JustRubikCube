using UnityEngine;
using UnityEngine.Events;

public class TagRubikCubeOfficialColor : MonoBehaviour {

    public RubikOfficialColor m_color;

    public UnityEvent<RubikOfficialColor> m_onColorChanged;




    [ContextMenu("Set As random color")]
    public void SetRandomColor()
    {

        int randomColor = Random.Range(0, 6);
        switch (randomColor)
        {
            case 0: SetTheColor(RubikOfficialColor.Red); break;
            case 1: SetTheColor(RubikOfficialColor.Green); break;
            case 2: SetTheColor(RubikOfficialColor.Blue); break;
            case 3: SetTheColor(RubikOfficialColor.Yellow); break;
            case 4: SetTheColor(RubikOfficialColor.Orange); break;
            default: SetTheColor(RubikOfficialColor.White); break;
        }
    }

    [ContextMenu("Set As next Color")]
    public void SetNextColor()
    {
        if (m_color == RubikOfficialColor.Red) SetTheColor(RubikOfficialColor.Green);
        else if (m_color == RubikOfficialColor.Green) SetTheColor(RubikOfficialColor.Blue);
        else if (m_color == RubikOfficialColor.Blue) SetTheColor(RubikOfficialColor.Yellow);
        else if (m_color == RubikOfficialColor.Yellow) SetTheColor(RubikOfficialColor.Orange);
        else if (m_color == RubikOfficialColor.Orange) SetTheColor(RubikOfficialColor.White);
        else if (m_color == RubikOfficialColor.White) SetTheColor(RubikOfficialColor.Red);

    }



    private void Awake()
    {
        SetTheColor(m_color);
    }
    public void SetTheColor(RubikOfficialColor color) { 
        m_color = color;

        m_onColorChanged?.Invoke(color);
    }
    public void GetTheColor(out RubikOfficialColor color)
    {
        color = m_color;
    }
    public void GetTheColor(out Color color)
    {

        RubikColor.GetTheColorOfUnity(m_color, out color);    

    }
   
}
public class RubikColor {

    public static void GetTheColorOfUnity(RubikOfficialColor from, out Color color)
    {
        switch (from)
        {

            case RubikOfficialColor.Red: color = Color.red; break;
            case RubikOfficialColor.Green: color = Color.green; break;
            case RubikOfficialColor.Blue: color = Color.blue; break;
            case RubikOfficialColor.White: color = Color.white; break;

            case RubikOfficialColor.Yellow: color = Color.yellow; break;
            case RubikOfficialColor.Orange: color = Color.orange; break;

            default: color = Color.white; break;
        }

    }
}