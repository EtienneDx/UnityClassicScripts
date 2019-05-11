using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SceneClickUtils
{
    public static NextClick nextClick;

    static SceneClickUtils()
    {
        SceneView.onSceneGUIDelegate += SceneGUI;
    }

    private static void SceneGUI(SceneView v)
    {
        if (nextClick != null && nextClick.GetInvocationList().Length > 0)
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit, 300))
                {
                    nextClick(hit.point);
                    ClearNextClick();
                    Event.current.Use();
                }
                else
                    ClearNextClick();
            }
    }

    public delegate void NextClick(Vector3 worldPos);

    public static void ClearNextClick()
    {
        if (nextClick == null) return;
        Delegate[] nextClicks = nextClick.GetInvocationList();
        foreach (NextClick nc in nextClicks)
        {
            nextClick -= nc;
        }
    }
}
