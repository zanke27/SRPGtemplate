using UnityEngine;
using UnityEditor;
using System;
using System.IO;
public enum EditMode
{ 
    None = 0,
    Create,
    Edit
}

public enum EditToolMode
{
    Paint,
    Erase
}

public class ToolEditor : EditorWindow
{
    public EditMode CurrentMode = EditMode.None;

    public EditToolMode selectedEditToolMode = EditToolMode.Paint;
    public GUIContent[] editToolModeContents;

    public CustomGridPalette targetPalette;
    public CustomGridPaletteDrawer paletteDrawer = new CustomGridPaletteDrawer();

    public Vector2Int cellCount;
    public Vector2 cellSize;

    public CustomGrid targetGrid;

    private bool isCraeteable => cellCount.x > 0 && cellCount.y > 0 && cellSize.x > 0 && cellSize.y > 0 && targetPalette != null;

    [MenuItem("Tool/Generate Map Tool")]
    static void Open()
    {
        var window = GetWindow<ToolEditor>();
        window.title = "Generate Map Tool";
    }

    private void OnEnable()
    {
        editToolModeContents = new GUIContent[]
        {
            EditorGUIUtility.TrIconContent("Grid.PaintTool", "그리기 모드"),
            EditorGUIUtility.TrIconContent("Grid.EraserTool", "지우기 모드")
        };

        ChangeMode(EditMode.Create);

        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;

        Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        Undo.undoRedoPerformed += OnUndoRedoPerformed;
    }

    private void OnUndoRedoPerformed()
    {
        targetGrid.RetreiveAll();
    }

    private void OnDisable()
    {
        ClearAll();
        Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        SceneView.duringSceneGui -= OnSceneGUI; 
    }

    private void OnSceneGUI(SceneView obj)
    {
        if (CurrentMode != EditMode.Edit)
        {
            return;
        }

        if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
        {
            var mousePos = Event.current.mousePosition;
            var ray = HandleUtility.GUIPointToWorldRay(mousePos);
            EditorHelper.RayCast(ray.origin, ray.origin + ray.direction * 300, out var hitPos);

            var cellPos = targetGrid.GetCellPos(hitPos);

            if (targetGrid.Contains(cellPos))
            {
                if (selectedEditToolMode == EditToolMode.Paint)
                {
                    Paint(cellPos);
                }
                else if (selectedEditToolMode == EditToolMode.Erase)
                {
                    Erase(cellPos);
                }
            }
        }
    }

    private void Paint(Vector2Int cellPos)
    {
        var selectedItem = paletteDrawer.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        if (targetGrid.IsItemExist(cellPos))
        {
            Undo.DestroyObjectImmediate(targetGrid.GetItem(cellPos).gameObject);
            targetGrid.RemoveItem(cellPos);
        }

        var target = targetGrid.AddItem(cellPos, selectedItem);

        Undo.RegisterCreatedObjectUndo(target.gameObject, "Create MapObject!");

        Event.current.Use();
    }

    private void Erase(Vector2Int cellPos)
    {
        if (targetGrid.IsItemExist(cellPos))
        {
            Undo.DestroyObjectImmediate(targetGrid.GetItem(cellPos).gameObject);
            targetGrid.RemoveItem(cellPos);
        }

        Event.current.Use();
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
            cellCount = EditorGUILayout.Vector2IntField("Cell 개수", cellCount);
            cellSize = EditorGUILayout.Vector2Field("Cell 크기", cellSize);

            targetPalette = (CustomGridPalette)EditorGUILayout.ObjectField("연결할 팔레트", targetPalette, typeof(CustomGridPalette));
            paletteDrawer.TargetPalette = targetPalette;
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
        ClearAll();
    
        var grid = new GameObject("Grid").AddComponent<CustomGrid>();

        grid.config = new CustomGridConfig();
        grid.config.CellCount = cellCount;
        grid.config.CellSize = cellSize;

        return grid;
    }

    private void DrawEditMode()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            if (GUILayout.Button("생성 모드", EditorStyles.toolbarButton))
            {
                ClearAll();
                ChangeMode(EditMode.Create);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("불러오기", EditorStyles.toolbarButton))
            {
                Load();
            }

            if (GUILayout.Button("저장하기", EditorStyles.toolbarButton))
            {
                Save();
            }
        }
        GUILayout.EndHorizontal();

        EditorHelper.DrawCenterLabel(new GUIContent("편집 모드"), Color.red, 20, FontStyle.Normal);

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            selectedEditToolMode = (EditToolMode)GUILayout.Toolbar((int)selectedEditToolMode, editToolModeContents, "LargeButton", GUILayout.Width(100), GUILayout.Height(40));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        var lastRect = GUILayoutUtility.GetLastRect();
        var area = new Rect(0, lastRect.yMax, position.width, position.height - lastRect.yMax - 1);

        GUI.Box(area, GUIContent.none, GUI.skin.window);

        paletteDrawer.Draw(new Vector2(position.width, position.height)); 
    }

    private void Save()
    {
        var path = EditorUtility.SaveFilePanel("맵 데이터 저장", Application.dataPath, "MapData.bin", "bin");

        if (string.IsNullOrEmpty(path) == false)
        {
            byte[] data = targetGrid.Serialize();

            File.WriteAllBytes(path, data);

            ShowNotification(new GUIContent("저장 성공 !"), 3);
        }
    }

    private void Load()
    {
        var path = EditorUtility.OpenFilePanel("맵 데이터 불러오기", Application.dataPath, "bin");

        if (string.IsNullOrEmpty(path) == false)
        {
            var bytes = File.ReadAllBytes(path);

            if (bytes != null)
            {
                targetGrid.Import(bytes, targetPalette);
            }
        }
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
                // 탑뷰로 볼 수 있는 기능 있는지 확인해서 추가하기
                //SceneView.lastActiveSceneView.in2DMode = true;
                Debug.Log("Chage To Edit Mode!");
                CurrentMode = newMode;
            break;
        }
    }

    private void ClearAll()
    {
        var existings = FindObjectsOfType<CustomGrid>();

        if (existings != null)
            for (int i = 0; i < existings.Length; i++)
                GameObject.DestroyImmediate(existings[i].gameObject);

        targetGrid = null;
    }
}