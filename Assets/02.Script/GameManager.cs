using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private Unit selectUnit;
    public Unit SelectUnit
    {
        get { return selectUnit; }
    }

    public void UnitSelect(Unit unit)
    {
        selectUnit = unit;
    }

    public void UnUnitSelect()
    {
        selectUnit = null;
    }
}
