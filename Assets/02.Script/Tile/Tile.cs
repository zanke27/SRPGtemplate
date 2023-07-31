using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Vector2 tilePos;

    private void Awake()
    {
        SetPosition();
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
