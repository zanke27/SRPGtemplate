using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Vector2 tilePos;

    public bool IsCantMoveTile;
    public Tile ParentTile;
    public int moveCost;

    public int x, y, G, H;
    public int F { get { return G + H; } }

    private void Awake()
    {
        SetPosition();
        SetName();
    }

    [ContextMenu("SetPosition")]
    public void SetPosition()
    {
        tilePos.x = transform.position.x;
        tilePos.y = transform.position.z;
    }

    [ContextMenu("SetName")]
    public void SetName()
    {
        gameObject.name = "";
        gameObject.name = $"{tilePos.x} {tilePos.y}";
    }
}
