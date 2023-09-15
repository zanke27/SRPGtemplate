using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorHelper
{
    public static void DrawCenterLabel(GUIContent content, Color color, int size, FontStyle style)
    {
        var guiStyle = new GUIStyle();
        guiStyle.fontSize = size;
        guiStyle.fontStyle = style;
        guiStyle.normal.textColor = color;
        guiStyle.padding.top = 10;
        guiStyle.padding.bottom = 10;

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(content, guiStyle);
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }

    public static bool DrawCenterButton(string text, Vector2 size)
    {
        bool clicked;

        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            clicked = GUILayout.Button(text, GUILayout.Width(size.x), GUILayout.Height(size.y));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        return clicked;
    }

    public static void RayCast(Vector3 rayOriPos, Vector3 rayDestPos, out Vector3 hitPos)
    {
        Vector3 planePos01 = Vector3.forward;
        Vector3 planePos02 = Vector3.right;
        Vector3 planePos03 = Vector3.back;

        Vector3 planeDir = Vector3.Cross((planePos02 - planePos01).normalized, (planePos03 - planePos01).normalized);
        Vector3 lineDir = (rayDestPos - rayOriPos).normalized;

        float dotLinePlace = Vector3.Dot(lineDir, planeDir);
        float t = Vector3.Dot(planePos01 - rayOriPos, planeDir) / dotLinePlace;

        hitPos = rayOriPos + (lineDir * t);
    }
}
