using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid))]
public class CustomGridEditor : Editor
{
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandle(CustomGrid grid, GizmoType type)
    {
        if (grid.reposition)
        {
            grid.RefreshPoints();
            grid.reposition = false;
        }

        Handles.DrawLines(grid.verLines);         
        Handles.DrawLines(grid.horLines);         
    }
}
