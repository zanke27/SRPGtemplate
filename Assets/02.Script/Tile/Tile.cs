using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타일 상태를 보고 갈 수 있는 타일인지 판단하기 위해서
public enum TileState
{
    None, // 타일에 아무도 없음
    Have  // 타일에 누군가 있음
}

public class Tile : MonoBehaviour
{
    [SerializeField] private Vector2 tilePos;
    public Vector2 TilePos
    { 
        get { return tilePos; }
    }

    public TileState TileState;
    public bool IsCantMoveTile; // 이거 왜있지 -> 벽판정 하려고
    public Tile ParentTile;
    public int moveCost;

    public int x, y, G, H;
    public int F { get { return G + H; } }

    public GameObject selectTile;

    // 움직일 수 있는 타일인가? (이동 범위 안쪽인가?)
    [SerializeField] private bool isSelectMove;
    public bool IsSelectMove
    {
        get { return isSelectMove; }
    }

    private void Awake()
    {
        SetPosition();
        SetName();
    }

    public void SelectTile()
    {
        selectTile.SetActive(true);
        isSelectMove = true;
    }

    public void ReleaseTile()
    {
        selectTile.SetActive(false);
        isSelectMove = false;
    }

    public void Reset()
    {
        G = 0;
        H = 0;
    }

    [ContextMenu("SetPosition")]
    public void SetPosition()
    {
        tilePos.x = transform.position.x - 0.5f;
        tilePos.y = transform.position.z - 0.5f; 
        x = (int)tilePos.x;
        y = (int)tilePos.y;
    }

    [ContextMenu("SetName")]
    public void SetName()
    {
        gameObject.name = "";
        gameObject.name = $"{tilePos.x} {tilePos.y}";
    }
}
