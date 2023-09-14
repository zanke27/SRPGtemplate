using UnityEngine;
using UnityEditor;
using System;
using PlasticPipe.PlasticProtocol.Messages;

public enum EditMode
{ 
    None = 0,
    Create,
    Edit
}

public class ToolEditor : EditorWindow
{
    public EditMode CurrentMode = EditMode.None;

    public Vector2Int cellCount;
    public Vector2 cellSize;

    public CustomGrid targetGrid;

    private bool isCraeteable => cellCount.x > 0 && cellCount.y > 0 && cellSize.x > 0 && cellSize.y > 0;

    [MenuItem("Tool/Generate Map Tool")]
    static void Open()
    {
        var window = GetWindow<ToolEditor>();
        window.title = "Generate Map Tool";
    }

    private void OnEnable()
    {
        ChangeMode(EditMode.Create);

        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnSceneGUI(SceneView obj)
    {
        //var mousePos = Event.current.mousePosition;
        //var ray = HandleUtility.GUIPointToWorldRay(mousePos);

        //EditorHelper.RayCast(ray.origin, ray.origin + ray.direction * 300, out var hitPos);
        //Debug.Log(FindObjectOfType<CustomGrid>().GetCellPos(hitPos));
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        if (CurrentMode == EditMode.Create)
            DrawCreateMode();
        else
            DrawEditMode();
    }

    private void DrawCreateMode()
    {
        EditorHelper.DrawCenterLabel(new GUIContent("크리에이트 모드"), Color.green, 20, FontStyle.Normal);
        
        using (var scope = new GUILayout.VerticalScope(GUI.skin.window))
        {
            //cellCount = EditorGUILayout.Vector2IntField("Cell 개수", cellCount);
            //cellSize = EditorGUILayout.Vector2Field("Cell 크기", cellSize);
        }

        GUI.enabled = isCraeteable;
        if (EditorHelper.DrawCenterButton("생성하기", new Vector2(100, 50)))
        {
            // 현재 버튼을 눌러도 생성이 되지 않는 버그 발생, 이유 찾기
            targetGrid = BuildGrid(this.cellCount, this.cellSize);
            ChangeMode(EditMode.Edit);
        }
        GUI.enabled = true;
    }

    private CustomGrid BuildGrid(Vector2Int cellCount, Vector2 cellSize)
    {
        var existings = FindObjectsOfType<CustomGrid>();

        if (existings != null)
            for (int i = 0;i < existings.Length; i++)
                GameObject.DestroyImmediate(existings[i].gameObject);
    
        var grid = new GameObject("Grid").AddComponent<CustomGrid>();

        grid.config = new CustomGridConfig();
        grid.config.CellCount = cellCount;
        grid.config.CellSize = cellSize;

        return grid;
    }

    private void DrawEditMode()
    {

    }

    private void ChangeMode(EditMode newMode)
    {
        if (CurrentMode == newMode) return;

        switch (newMode)
        {
            case EditMode.None:

            break;
            case EditMode.Create:
                Debug.Log("Chage To Create Mode!");
                CurrentMode = newMode;
            break;
            case EditMode.Edit:
                Debug.Log("Chage To Edit Mode!");
                CurrentMode = newMode;
            break;
        }
    }
}