using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Vector2 cellPos;

    private void Awake()
    {
        SetPosition();
    }

    [ContextMenu("SetPosition")]
    public void SetPosition()
    {
        cellPos.x = transform.position.x;
        cellPos.y = transform.position.z;
    }

    [ContextMenu("SetName")]
    public void SetName()
    {
        gameObject.name = "";
        gameObject.name = $"{cellPos.x} {cellPos.y}";
    }
}
