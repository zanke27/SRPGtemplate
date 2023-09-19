using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayCheck : MonoBehaviour
{
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Unit selectUnit = hit.transform.GetComponent<Unit>();
                Tile selectTile = hit.transform.GetComponent<Tile>();
                if (selectUnit != null)
                {
                    selectUnit.MoveReady();
                    GameManager.Instance.UnitSelect(selectUnit);
                }
                else if (selectTile != null && selectTile.IsSelectMove)
                {
                    TileManager.Instance.AStar(
                            (int)(GameManager.Instance.SelectUnit.UnitPosX - 0.5f),
                            (int)(GameManager.Instance.SelectUnit.UnitPosY - 0.5f),
                            selectTile.x, selectTile.y);
                    GameManager.Instance.SelectUnit.Move();
                }
                else
                {
                    TileManager.Instance.ReleaseMoveableTile();
                    GameManager.Instance.UnUnitSelect();
                }
            }
        }
    }
}
