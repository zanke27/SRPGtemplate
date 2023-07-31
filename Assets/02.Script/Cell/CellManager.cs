using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public Vector3 leftBottomLocation = new Vector3 (0, 0, 0);
    public GameObject[,] cellArr;
    public int rows = 5;
    public int columns = 5;

    private void Awake()
    {
        cellArr = new GameObject[columns + 1, rows + 1];
    }

    [ContextMenu("SetCell")]
    public void SetCell()
    {
        for (int i = 0; i < columns; i++)
            for (int j = 0; j < rows; j++)
                cellArr[i, j] = transform.GetChild(i*columns + j).gameObject.GetComponent<GameObject>();
    }

    #region Á¤·Ä

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
