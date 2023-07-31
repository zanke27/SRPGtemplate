using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Vector3 leftBottomLocation = new Vector3 (0, 0, 0);
    public GameObject[,] tileArr;
    public int rows = 5;
    public int columns = 5;

    private void Awake()
    {
        tileArr = new GameObject[columns + 1, rows + 1];
    }

    [ContextMenu("SetTile")]
    public void SetTile()
    {
        //tileArr = new GameObject[columns + 1, rows + 1];

        foreach (Transform child in transform)
        {
            string[] splitXY = child.name.Split(' ');

            int x = int.Parse(splitXY[0]);
            int y = int.Parse(splitXY[1]);

            tileArr[x, y] = child.GetComponent<GameObject>();
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
