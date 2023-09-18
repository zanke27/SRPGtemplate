using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CustomGrid : MonoBehaviour
{
    public CustomGridConfig config;

    public Dictionary<Vector2Int, MapObject> Items = new Dictionary<Vector2Int, MapObject>();

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

    private Vector3 GetWorldPos(Vector2Int cellPos)
    {
        return new Vector3(cellPos.x * config.CellSize.x + config.CellSize.x * 0.5f, 0, cellPos.y * config.CellSize.y + config.CellSize.y * 0.5f);
    }

    public void RetreiveAll()
    {
        Items.Clear();

        var mapObjs = FindObjectsOfType<MapObject>();
        if(mapObjs != null)
        {
            for (int i = 0; i < mapObjs.Length; i++)
            {
                Items.Add(mapObjs[i].cellPos, mapObjs[i]);
            }
        }
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

    public bool Contains(Vector2Int cellPos)
    {
        return cellPos.x >= 0 && cellPos.x < config.CellCount.x && cellPos.y >= 0 && cellPos.y < config.CellCount.y;
    }

    public bool IsItemExist(Vector2Int cellPos)
    {
        return Items.ContainsKey(cellPos);
    }
    public MapObject GetItem(Vector2Int cellPos)
    {
        if (Items.ContainsKey(cellPos) == false)
        {
            return null;
        }

        return Items[cellPos];
    }
    public MapObject AddItem(Vector2Int cellPos, CustomGridPaletteItem paletteItem)
    {
        if (Items.ContainsKey(cellPos))
        {
            Debug.LogError("already exist! delete first!");
            return null;
        }

        var target = GameObject.Instantiate(paletteItem.targetObject, transform);
        target.transform.position = GetWorldPos(cellPos);
        var comp = target.AddComponent<MapObject>();
        comp.id = paletteItem.id;
        comp.cellPos = cellPos;

        Items.Add(cellPos, comp);

        return comp;
    }

    public void RemoveItem(Vector2Int cellPos)
    {
        if(Items.ContainsKey(cellPos) != false)
        {
            Items.Remove(cellPos);
        }
    }

    public byte[] Serialize()
    {
        byte[] bytes = null; // 담을 바이트 초기화
        using (var ms = new MemoryStream()) // 쓰여진 메모리를 담을 스트림
        {
            using ( var writer = new BinaryWriter(ms)) // 메모리에 쓸 라이터
            {

                writer.Write(Items.Count); // 아이템의 갯수 넣고

                foreach(var item in Items)
                {
                    writer.Write(item.Key.x); // 아이템의 x
                    writer.Write(item.Key.y); // y값 넣기
                    writer.Write(item.Value.id); // id도 넣어줌
                }

                bytes = ms.ToArray(); // ToArray롤 해서 바이트에 넣어주면됨
            }
        }

        return bytes;
    }

    public void Import(byte[] buffer, CustomGridPalette targetPalette)
    {
        foreach(var item in Items)
        {
            GameObject.DestroyImmediate(item.Value.gameObject);
        }

        Items.Clear();

        using (var ms = new MemoryStream(buffer))
        {
            using (var reader = new BinaryReader(ms))
            {
                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    var xPos = reader.ReadInt32();
                    var yPos = reader.ReadInt32();
                    var id = reader.ReadInt32();

                    var pos = new Vector2Int(xPos, yPos);

                    AddItem(pos, targetPalette.GetItem(id));
                }
            }
        }
    }
}
