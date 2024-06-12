using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public GameObject frog;
    public GameObject berry;
    public GameObject arrow;
    public GameObject selectedChild;

    public SkinnedMeshRenderer frogRenderer;
    public Renderer berryRenderer;
    public SpriteRenderer arrowChildSpriteRenderer;
    public CellColor cellColor;
    public ColorData colorDatabase;

    public GameObject directlyBelowCell;
    public bool haveParent;

    void Start()
    {
        if (haveParent)
        {
            CheckDirectlyBelowCell();
        }

    }

    public void SelectChild(GameObject child)
    {
        selectedChild = child;
    }

    public GameObject GetSelectedChild()
    {
        return selectedChild;
    }

    public bool IsChildSelected(GameObject child)
    {
        return selectedChild == child;
    }

    public void SetChild(ChildType childType)
    {
        frog.SetActive(false);
        berry.SetActive(false);
        arrow.SetActive(false);

        switch (childType)
        {
            case ChildType.Frog:
                frog.SetActive(true);
                SelectChild(frog);
                break;
            case ChildType.Berry:
                berry.SetActive(true);
                SelectChild(berry);
                break;
            case ChildType.ArrowRight:
                arrow.SetActive(true);
                SelectChild(arrow);
                break;
            case ChildType.ArrowLeft:
                arrow.SetActive(true);
                SelectChild(arrow);
                break;
            case ChildType.ArrowForward:
                arrow.SetActive(true);
                SelectChild(arrow);
                break;
            case ChildType.ArrowBack:
                arrow.SetActive(true);
                SelectChild(arrow);
                break;
        }
    }


    public void SetColorWithWhite(CellColor color)  // Aktive Cell
    {
        cellColor = color;
        Material[] materials = colorDatabase.GetCellMaterials(cellColor, true);  
        GetComponent<Renderer>().materials = materials;  

        if (frogRenderer != null)
        {
            frogRenderer.material = colorDatabase.GetFrogMaterial(cellColor);
        }

        if (berryRenderer != null)
        {
            berryRenderer.material = colorDatabase.GetBerryMaterial(cellColor);
        }

        if (arrowChildSpriteRenderer != null)
        {
            arrowChildSpriteRenderer.color = colorDatabase.GetArrowColor(cellColor);
        }
    }

    public void SetColor(CellColor color) // Pasif Cell
    {

        cellColor = color;
        Material[] materials = colorDatabase.GetCellMaterials(cellColor);  
        GetComponent<Renderer>().sharedMaterials = materials; 

        if (frogRenderer != null)
        {
            frogRenderer.material = colorDatabase.GetFrogMaterial(cellColor);
        }

        if (berryRenderer != null)
        {
            berryRenderer.material = colorDatabase.GetBerryMaterial(cellColor);
        }

        if (arrowChildSpriteRenderer != null)
        {
            arrowChildSpriteRenderer.color = colorDatabase.GetArrowColor(cellColor);
        }

    }


    private void CheckDirectlyBelowCell()
    {
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            CellManager parentCellManager = parentTransform.GetComponent<CellManager>();
            if (parentCellManager != null)
            {
                directlyBelowCell = parentTransform.gameObject;
                if (parentCellManager.selectedChild != null)
                {
                    parentCellManager.selectedChild.transform.localScale = new Vector3(0, 0, 0);
                    parentCellManager.selectedChild.SetActive(false);
                }
                Debug.Log("Parent cell found: " + directlyBelowCell.name);
            }
            else
            {
                Debug.LogWarning("Parent does not have a CellManager component.");
            }
        }
        else
        {
            Debug.LogWarning("No parent transform found.");
        }
    }
}