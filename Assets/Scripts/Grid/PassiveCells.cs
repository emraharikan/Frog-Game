using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCells : MonoBehaviour
{
    public CellColor cellColor;
    public ColorData colorDatabase;

    public void SetColor(CellColor color)
    {
        cellColor = color;
        Material[] materials = colorDatabase.GetCellMaterials(cellColor); 
        GetComponent<Renderer>().sharedMaterials = materials; 
    }
}
