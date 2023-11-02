using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject goalPrefab;

    public void Draw(Cell[] data, Cell goal)
    {
        foreach (Cell cell in data)
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * CellVisualizer.CellSize, 0, -cell.Y * CellVisualizer.CellSize);
            obj.gameObject.name = $"Cell_({cell.X},{cell.Y})";

            CellVisualizer displayer = obj.GetComponent<CellVisualizer>();
            displayer.RefreshWall(cell.Path);
        }

        GameObject goalObj = Instantiate(goalPrefab, transform);
        goalObj.transform.Translate(goal.X * CellVisualizer.CellSize, 0, -goal.Y * CellVisualizer.CellSize);
        goalObj.gameObject.name = "Cell_Goal";
        CellVisualizer goalDisplayer = goalObj.GetComponent<CellVisualizer>();
        goalDisplayer.RefreshWall(goal.Path);
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
