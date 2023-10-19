using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;

    public void Draw(Cell[] data)
    {
        foreach (Cell cell in data)
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * CellVisualizer.CellSize, 0, -cell.Y * CellVisualizer.CellSize);
            obj.gameObject.name = $"Cell_({cell.X},{cell.Y})";

            CellVisualizer displayer = obj.GetComponent<CellVisualizer>();
            displayer.RefreshWall(cell.Path);
        }
    }

    public void Clear()
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }
}
