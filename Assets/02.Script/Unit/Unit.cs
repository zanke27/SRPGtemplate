using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// Unit이 해야 할 일
// 1. 이동, 공격 등 행동을 제어 해야 함
// 2. 공격력, 체력, 이동 거리 등 데이터를 가져야함

public class Unit : MonoBehaviour
{
    // 유닛의 현재 위치
    private int unitPosX, unitPosY;
    public int UnitPosX { get { return unitPosX; } }
    public int UnitPosY { get { return unitPosY; } }

    private bool moveing = false;

    #region 데이터

    // 체력 / 공격 데미지 / 이동 거리
    private int hp = 100;
    private int damage = 10;
    private int moveDistance = 3;

    #endregion

    private Tile nowTile;

    private void Awake()
    {
        unitPosX = (int)transform.position.x;
        unitPosY = (int)transform.position.z;
    }

    private void FixedUpdate()
    {
        if (moveing == true)
        {
            float vecX = nowTile.transform.position.x - unitPosX;
            float vecZ = nowTile.transform.position.z - unitPosY;
            transform.Translate(new Vector3(vecX, 0, vecZ).normalized * 5 * Time.deltaTime);
            if (Mathf.Abs(nowTile.transform.position.x - transform.position.x) < 0.01f
                && Mathf.Abs(nowTile.transform.position.z - transform.position.z) < 0.01f)
            {
                moveing = false;
                transform.position = new Vector3(nowTile.transform.position.x, transform.position.y, nowTile.transform.position.z);
                unitPosX = (int)transform.position.x;
                unitPosY = (int)transform.position.z;
            }
        }
    }

    public void Move()
    {
        StartCoroutine(MoveCor());
    }

    public IEnumerator MoveCor()
    {
        List<Tile> moveList = TileManager.Instance.completeTileList;
        foreach(Tile list in moveList)
        {
            moveing = true;
            nowTile = list;

            yield return new WaitUntil(()=> moveing == false);
        }

        TileManager.Instance.ReleaseMoveableTile();
        GameManager.Instance.UnUnitSelect();
    }

    public void MoveReady()
    {
        TileManager.Instance.MoveReady(unitPosX, unitPosY, moveDistance);
    }
}
