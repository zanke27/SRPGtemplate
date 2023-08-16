using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

// G: 지금까지 이동한 거리
// H: 목표까지의 예상 거리
// F: G + H 한 값

// OpenTileList: 최단 경로를 찾기 위해 계속 갱신되는 타일 리스트
// ClosedTileList: 거리 계산을 완료한 더이상 필요 없는 타일 리스트
// completeTileList: 계산이 완료된 최단 경로 타일 리스트

public class TileManager : MonoBehaviour
{

    public List<Tile> completeTileList;

    [SerializeField] private int arrX = 5;
    [SerializeField] private int arrY= 5;
    
    private Tile[,] tileArr;
    private Tile startTile, endTile, currentTile;
    List<Tile> openTileList, closedTileList;

    private void Start()
    {
        tileArr = new Tile[arrX + 1, arrY + 1];
        SetTile();
    }

    public void AStar(int startX, int startY, int endX, int endY)
    {
        // StartTile이랑 EndTile은 이렇게 가져오는 것 말고 x, y 위치 받아서 tileArr에서 빼오는 것도 좋을 듯?
        startTile = tileArr[startX, startY];
        endTile = tileArr[endX, endY];

        openTileList = new List<Tile>();
        closedTileList = new List<Tile>();
        completeTileList = new List<Tile>();

        openTileList.Add(startTile);

        while(openTileList.Count > 0) 
        {
            // 시작타일을 현재 선택 타일로 / 시작 타일은 무조건 인덱스 0
            currentTile = openTileList[0];

            // 열린 타일 리스트에서 F가 가장 작으며, H가 가장 작은 타일을 현재 선택 타일로 하고, 닫힌 리스트로 옮긴다.
            for (int i = 1; i < openTileList.Count; i++) 
                if (openTileList[i].F <= currentTile.F && openTileList[i].H < currentTile.H) 
                    currentTile = openTileList[i];

            openTileList.Remove(currentTile);
            closedTileList.Add(currentTile);

            // 만약 현재 선택한 타일이 도착지점 타일이라면
            if (currentTile == endTile)
            {
                Tile endCurrentTile = endTile;
                while (endCurrentTile != startTile)
                {
                    // 하나씩 거슬러 올라가면서 completeTileList에 최단 경로 추가
                    completeTileList.Add(endCurrentTile);
                    endCurrentTile = endCurrentTile.ParentTile;
                }
                // 마지막으로 시작 지점 넣어주고
                completeTileList.Add(startTile);

                // 거꾸로 경로를 추가했으니까 반전 시켜주기
                completeTileList.Reverse();

                // 끝내기
                return;
            }

            AddToOpenList(currentTile.x, currentTile.y + 1); // 상
            AddToOpenList(currentTile.x, currentTile.y - 1); // 하
            AddToOpenList(currentTile.x - 1, currentTile.y); // 좌
            AddToOpenList(currentTile.x + 1, currentTile.y); // 우
                                                             // 를 리스트에 넣어주기
        }

    }

    private void AddToOpenList(int checkX, int checkY)
    {
        // 상하 좌우 범위를 벗어나지 않고
        if (checkX >= 0 && checkX < arrX && checkY >= 0 && checkY < arrY)
        {
            // 이동할 수 없는 타일이 아니며
            if (tileArr[checkX, checkY].IsCantMoveTile == false)
            {
                // 이미 처리가 끝난 닫힌 타일 리스트에 현재 타일이 있지 않다면
                if (closedTileList.Contains(tileArr[checkX, checkY]) == false)
                {
                    // 인접한 타일로 추가하고, moveCost(G) 설정
                    Tile neighborTile = tileArr[checkX, checkY];
                    int moveCost = currentTile.G + currentTile.moveCost;

                    // 이동 비용이 인접한 타일의 G보다 작거나 / 열린 리스트에 인접한 타일가 없다면
                    // G, H, Parent를 설정한 인접 타일을 열린 타일 리스트에 넣어주기
                    if (moveCost < neighborTile.G || openTileList.Contains(neighborTile) == false)
                    {
                        neighborTile.G = moveCost;
                        neighborTile.H = Mathf.Abs(neighborTile.x - endTile.x) + Mathf.Abs(neighborTile.x - endTile.x);
                        neighborTile.ParentTile = currentTile;

                        openTileList.Add(neighborTile);
                    }
                }
            }
        }
    }

    public void SetTile()
    {
        foreach (Transform child in transform)
        {
            string[] splitXY = child.name.Split(' ');

            int x = int.Parse(splitXY[0]);
            int y = int.Parse(splitXY[1]);

            tileArr[x, y] = child.GetComponent<Tile>();
        }
    }

    #region 정렬

    private GameObject parentObj;
    private GameObject[] childrenObjArr;

    [ContextMenu("ReOrder")]
    public void ReOrder()
    {
        parentObj = transform.gameObject;
        childrenObjArr = GetChildren(parentObj);
        childrenObjArr = childrenObjArr.OrderBy(go => go.name).ToArray();

        for (int i = 0; i < childrenObjArr.Length; i++)
        {
            childrenObjArr[i].transform.SetSiblingIndex(i);
        }
    }

    GameObject[] GetChildren(GameObject parent)
    {
        GameObject[] children = new GameObject[parent.transform.childCount];

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i).gameObject;
        }

        return children;
    }
    #endregion
}
