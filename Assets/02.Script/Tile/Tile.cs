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

    public TileState TileState;
    public bool IsCantMoveTile;
    public Tile ParentTile;
    public int moveCost;

    public int x, y, G, H;
    public int F { get { return G + H; } }

    public Material redMat;

    // 움직일 수 있는 타일인가? (이동 범위 안쪽인가?)
    private bool isSelectMove;

    private void Awake()
    {
        SetPosition();
        SetName();
    }

    public void SelectTile()
    {
        MeshRenderer meshRen = GetComponent<MeshRenderer>();
        meshRen.material = redMat;
        isSelectMove = true;
    }

    public void ReleaseTile()
    {
        isSelectMove = false;
    }

    [ContextMenu("SetPosition")]
    public void SetPosition()
    {
        tilePos.x = transform.position.x;
        tilePos.y = transform.position.z;
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
