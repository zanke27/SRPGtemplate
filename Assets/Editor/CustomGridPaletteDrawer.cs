using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGridPaletteDrawer
{
    public CustomGridPalette TargetPalette;
    Vector2 slotSize = new Vector2(100, 100);
    Vector2 scrollPos;

    int selectedIdx;

    public CustomGridPaletteItem SelectedItem
    {
        get
        {
            if (selectedIdx == -1)
            {
                return null;
            }

            return TargetPalette.Items[selectedIdx];
        }
    }

    public void Draw(Vector2 winSize)
    {
        if (TargetPalette == null || TargetPalette.Items.Count == 0)
        {
            EditorHelper.DrawCenterLabel(new GUIContent("데이터 없음"), Color.red, 15, FontStyle.Bold);
            return;
        }

        if (selectedIdx == -1)
        {
            selectedIdx = 0;
        }

        scrollPos = EditorHelper.DrawGridItems(scrollPos, 10, TargetPalette.Items.Count, winSize.x, slotSize, (idx) =>
        {
            bool selected = CustomGridPaletteItemDrawer.Draw(slotSize, selectedIdx == idx, TargetPalette.Items[idx]);
        
            if (selected)
            {
                selectedIdx = idx;  
            }
        });
    }
}
