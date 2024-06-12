using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ColorData/ColorData", order = 1)]
public class ColorData : ScriptableObject
{
    [Header("Cell Materials")]
    public Material baseMaterial;  // Base materail gri olacak
    public Material baseWhiteMaterial;  // Base material beyaz olacak
    public Material blueCellMaterial;
    public Material greenCellMaterial;
    public Material purpleCellMaterial;
    public Material redCellMaterial;
    public Material yellowCellMaterial;

    [Header("Frog Materials")]
    public Material blueFrogMaterial;
    public Material greenFrogMaterial;
    public Material purpleFrogMaterial;
    public Material redFrogMaterial;
    public Material yellowFrogMaterial;

    [Header("Berry Materials")]
    public Material blueBerryMaterial;
    public Material greenBerryMaterial;
    public Material purpleBerryMaterial;
    public Material redBerryMaterial;
    public Material yellowBerryMaterial;

    [Header("Arrow Colors")]
    public Color blueArrowColor;
    public Color greenArrowColor;
    public Color purpleArrowColor;
    public Color redArrowColor;
    public Color yellowArrowColor;


    public Material[] GetCellMaterials(CellColor color, bool useWhite = false)
    {
        Material firstMaterial;
        Material secondMaterial;

        switch (color)
        {
            case CellColor.Grey:
                firstMaterial = baseMaterial;
                secondMaterial = baseMaterial;
                break;
            case CellColor.Blue:
                firstMaterial = blueCellMaterial;
                secondMaterial = useWhite ? baseWhiteMaterial : baseMaterial;
                break;
            case CellColor.Green:
                firstMaterial = greenCellMaterial;
                secondMaterial = useWhite ? baseWhiteMaterial : baseMaterial;
                break;
            case CellColor.Purple:
                firstMaterial = purpleCellMaterial;
                secondMaterial = useWhite ? baseWhiteMaterial : baseMaterial;
                break;
            case CellColor.Red:
                firstMaterial = redCellMaterial;
                secondMaterial = useWhite ? baseWhiteMaterial : baseMaterial;
                break;
            case CellColor.Yellow:
                firstMaterial = yellowCellMaterial;
                secondMaterial = useWhite ? baseWhiteMaterial : baseMaterial;
                break;
            default:
                firstMaterial = null;
                secondMaterial = null;
                break;
        }

        return new Material[] { firstMaterial, secondMaterial };
    }

   

    public Material GetFrogMaterial(CellColor color)
    {
        switch (color)
        {
        
            case CellColor.Blue: return blueFrogMaterial;
            case CellColor.Green: return greenFrogMaterial;
            case CellColor.Purple: return purpleFrogMaterial;
            case CellColor.Red: return redFrogMaterial;
            case CellColor.Yellow: return yellowFrogMaterial;
            default: return null;
        }
    }

    public Material GetBerryMaterial(CellColor color)
    {
        switch (color)
        {
          
            case CellColor.Blue: return blueBerryMaterial;
            case CellColor.Green: return greenBerryMaterial;
            case CellColor.Purple: return purpleBerryMaterial;
            case CellColor.Red: return redBerryMaterial;
            case CellColor.Yellow: return yellowBerryMaterial;
            default: return null;
        }
    }

    public Color GetArrowColor(CellColor color)
    {
        switch (color)
        {
            case CellColor.Blue: return blueArrowColor;
            case CellColor.Green: return greenArrowColor;
            case CellColor.Purple: return purpleArrowColor;
            case CellColor.Red: return redArrowColor;
            case CellColor.Yellow: return yellowArrowColor;
            default: return Color.white;
        }
    }



}