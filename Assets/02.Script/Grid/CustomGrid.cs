using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    public CustomGridConfig config;

    public Vector3[] horLines;
    public Vector3[] verLines;

    public bool reposition;

    private void OnValidate()
    {
        reposition = true;
    }

    public Vector2Int GetCellPos(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x / config.CellSize.x), Mathf.FloorToInt(worldPos.z / config.CellSize.y));
    }

    public void RefreshPoints()
    {
        int verLineCnt = config.CellCount.x * 2 + 2;
        int horLineCnt = config.CellCount.y * 2 + 2;

        horLines = new Vector3[horLineCnt];
        verLines = new Vector3[verLineCnt];

        for (int i = 0; i <= config.CellCount.x; i++)
        {
            verLines[i * 2] = new Vector3(i * config.CellSize.x, 0, config.CellSize.y * config.CellCount.y);
            verLines[i * 2 + 1] = new Vector3(i * config.CellSize.x, 0, 0);
        }

        for (int i = 0; i <= config.CellCount.y; i++)
        {
            horLines[i * 2] = new Vector3(0, 0, i * config.CellSize.y);
            horLines[i * 2 + 1] = new Vector3(config.CellSize.x * config.CellCount.x, 0, i * config.CellSize.y);
        }
    }
}
