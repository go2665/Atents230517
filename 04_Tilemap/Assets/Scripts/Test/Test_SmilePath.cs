using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_SmilePath : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Slime slime;

    GridMap map;

    private void Start()
    {
        map = new GridMap(background, obstacle);

        if(slime == null)
        {
            slime = FindObjectOfType<Slime>();
        }
        slime.Initialize(map, Vector3.zero);
    }

    protected override void TestClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (map.IsValidPosition(gridPos) && !map.IsWall(gridPos) && !map.IsMonster(gridPos))
        {
            slime.SetDestination(gridPos);
        }
    }
}