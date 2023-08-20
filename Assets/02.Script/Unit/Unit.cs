using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unit이 해야 할 일
// 1. 이동, 공격 등 행동을 제어 해야 함
// 2. 공격력, 체력, 이동 거리 등 데이터를 가져야함

public class Unit : MonoBehaviour
{
    // 유닛의 현재 위치
    [SerializeField]
    private Vector2 unitPos;

    #region 데이터

    // 체력 / 공격 데미지 / 이동 거리
    private int hp = 100;
    private int damage = 10;
    private int moveDistance = 5;

    #endregion

    private void Update()
    {
        
    }

    [ContextMenu("Move")]
    public void Move()
    {
        TileManager.Instance.MoveReady((int)unitPos.x, (int)unitPos.y, moveDistance);
    }
}
