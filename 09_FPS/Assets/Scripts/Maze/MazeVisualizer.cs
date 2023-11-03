using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject goalPrefab;

    public int width;
    public int height;

    public void Draw(MazeGenerator maze)
    {
        width = maze.Width;
        height = maze.Height;
        Cell[] data = maze.Cells;
        Cell goal = maze.Goal;

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

    public Vector3 GridToWorld(int x, int y)
    {
        Vector3 result = new(CellVisualizer.CellSize * x + 2.5f, 0, -CellVisualizer.CellSize * y - 2.5f);
        return result;
    }
}
