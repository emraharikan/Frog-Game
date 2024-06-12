using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager gridManager = (GridManager)target;

        GUILayout.Label("Grid Dimensions");
        gridManager.rows = EditorGUILayout.IntField("Rows", gridManager.rows);
        gridManager.cols = EditorGUILayout.IntField("Cols", gridManager.cols);

        GUILayout.Label("Cell Color and Child Type");
        gridManager.selectedColor = (CellColor)EditorGUILayout.EnumPopup("Cell Color", gridManager.selectedColor);
        gridManager.selectedChildType = (ChildType)EditorGUILayout.EnumPopup("Child Type", gridManager.selectedChildType);

        if (GUILayout.Button("Create Grid"))
        {
            gridManager.CreateGrid();
        }
    }

    void OnSceneGUI()
    {
        GridManager gridManager = (GridManager)target;

        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Cell"))
                {
                    gridManager.AddCell(hit.point, hit.collider.transform);
                    e.Use();
                }
            }
        }
    }
}
#endif