using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab, activeCellPrefab;
    public int rows = 6;
    public int cols = 6;
    public float cellSize = 1f;
    public CellColor selectedColor = CellColor.Grey;
    public ChildType selectedChildType = ChildType.None;
    public ColorData colorDatabase;

    public GameObject splinePrefab;

    public void CreateGrid()  // Gri Cell içeren grid Oluştur
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        float gridWidth = cols * cellSize;
        float gridHeight = rows * cellSize;
        Vector3 gridOrigin = new Vector3(-gridWidth / 2 + cellSize / 2, 0, -gridHeight / 2 + cellSize / 2);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 cellPosition = new Vector3(i * cellSize, 0, j * cellSize) + gridOrigin;
                GameObject newCell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
                PassiveCells passiveCell = newCell.GetComponent<PassiveCells>();
                passiveCell.colorDatabase = colorDatabase;
                passiveCell.SetColor(CellColor.Grey);  
            }
        }
    }

    public void AddCell(Vector3 position, Transform parentCell)  // Gri Cell üstüne aktif cell ekle
    {
        Collider parentCollider = parentCell.GetComponent<Collider>();
        float newCellHeight = parentCollider != null ? parentCollider.bounds.size.y : 1;

        // Calculate new cell position based on parent's position and height
        Vector3 newCellPosition = parentCell.position + new Vector3(0, newCellHeight, 0);

        // Instantiate the new cell and set its parent
        GameObject newCell = Instantiate(activeCellPrefab, newCellPosition, Quaternion.identity, parentCell);
        newCell.transform.SetParent(parentCell);

        CellManager cellManager = newCell.GetComponent<CellManager>();
        if (cellManager != null)
        {
            cellManager.colorDatabase = colorDatabase;
            cellManager.SetColorWithWhite(selectedColor);
            cellManager.SetChild(selectedChildType);

            // Adjust rotation based on selected child type
            switch (selectedChildType)
            {
                case ChildType.ArrowRight:
                    cellManager.selectedChild.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case ChildType.ArrowLeft:
                    cellManager.selectedChild.transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case ChildType.ArrowForward:
                    cellManager.selectedChild.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case ChildType.ArrowBack:
                    cellManager.selectedChild.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
            }
        }
    }
}