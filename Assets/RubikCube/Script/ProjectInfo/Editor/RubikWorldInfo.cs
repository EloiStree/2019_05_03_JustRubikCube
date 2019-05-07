using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public class RubikWorldInfo : EditorWindow
{
    DateTime m_nextMilestone = new DateTime(2018, 05, 21);
    DateTime m_nextDeathline = new DateTime(2018, 05, 30);
    string m_urlGCD = "https://docs.google.com/document/d/1mojD1YjD4JLC6stRxIcl9wooiBfaj08uxH0iLRHrRB8/edit?usp=sharing";
    string m_urlToDoToRelease = "https://gitlab.com/JamsCenter/RubikWorld/boards/575345?=";
    string m_urlMilestoneToRelease = "https://gitlab.com/JamsCenter/RubikWorld/milestones";
    string m_urlSource = "https://gitlab.com/JamsCenter/RubikWorld/";
    string m_inspirationQuote = "Ernõ Rubik helped his students understand three-dimensional problems.";

    // Add menu named "My Window" to the Window menu
    [MenuItem("Rubik's World/Info")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        RubikWorldInfo window = (RubikWorldInfo)EditorWindow.GetWindow(typeof(RubikWorldInfo));
        window.minSize = new Vector2(450, 70);
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("GCD")){ Application.OpenURL(m_urlGCD); }
        if (GUILayout.Button("To Do")){ Application.OpenURL(m_urlToDoToRelease); }
        if (GUILayout.Button("Git Code")){ Application.OpenURL(m_urlSource); }
        if (GUILayout.Button("Milestone")){ Application.OpenURL(m_urlMilestoneToRelease); }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Objectif");
        GUILayout.Label("Next: " + GetTimeLeftFor(m_nextMilestone));
        GUILayout.Label("Death: " + GetTimeLeftFor(m_nextDeathline));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField(m_inspirationQuote);
    }

    public string GetTimeLeftFor(DateTime time)
    {
        TimeSpan timeLeft = new TimeSpan();

        timeLeft = time - DateTime.Now;
        return string.Format("{3:D2}D {0:D2}H {1:D2}:{2:D2}", timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds, timeLeft.Days);


    }
  
}