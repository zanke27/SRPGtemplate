using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomGrid/CreatePalette")]
public class CustomGridPalette : ScriptableObject
{
    public List<CustomGridPaletteItem> Items;

    public CustomGridPaletteItem GetItem(int id)
    {
        return Items.Find(x => x.id == id);
    }
}
