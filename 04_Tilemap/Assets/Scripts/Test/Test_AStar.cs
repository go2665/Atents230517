using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_AStar : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public PathLine pathline;

    public Vector2Int start;
    public Vector2Int end;

    GridMap map;

    List<int> list = new List<int>();
    List<Node> listNode = new List<Node>();

    private void Start()
    {
        map = new GridMap(background, obstacle);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.TestRClick.performed += TestRClick;
    }

    protected override void OnDisable()
    {
        inputActions.Test.TestRClick.performed -= TestRClick;
        base.OnDisable();
    }

    protected override void TestClick(InputAction.CallbackContext _)
    {
        // 왼쪽 클릭하면 start 변수에 해당 그리드 좌표 설정
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if( map.IsValidPosition(gridPos) && !map.IsWall(gridPos) && !map.IsMonster(gridPos) ) 
        { 
            start = gridPos;
        }
    }

    private void TestRClick(InputAction.CallbackContext _)
    {
        // 우클릭하면 end변수에 해당 그리드 좌표 설정
        // A* 알고리즘으로 경로 찾고
        // PathLine으로 표시하기

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (map.IsValidPosition(gridPos) && !map.IsWall(gridPos) && !map.IsMonster(gridPos))
        {
            end = gridPos;

            List<Vector2Int> path = AStar.PathFind(map, start, end);            
            pathline.DrawPath(map, path);
        }
    }

    private void Test_PathFind()
    {
        GridMap map = new GridMap(4, 3);
        Node node = map.GetNode(1, 0);
        node.nodeType = Node.NodeType.Wall;
        node = map.GetNode(2, 2);
        node.nodeType = Node.NodeType.Wall;

        List<Vector2Int> path = AStar.PathFind(map, new Vector2Int(0, 0), new Vector2Int(3, 2));

        string pathStr = "Path : ";
        foreach (var pos in path)
        {
            pathStr += $" ({pos.x}, {pos.y}) ->";
        }
        pathStr += "끝";
        Debug.Log(pathStr);
    }

    private void Test_Node()
    {
        list.Clear();
        list.Add(10);
        list.Add(40);
        list.Add(30);
        list.Add(90);
        list.Add(60);
        list.Add(70);
        list.Add(20);
        list.Add(100);
        list.Add(50);
        list.Add(80);

        listNode.Clear();
        Node a = new Node(0, 0);
        a.H = 10;
        a.G = 0;
        Node b = new Node(0, 0);
        b.H = 5;
        b.G = 0;
        Node c = new Node(0, 0);
        c.H = 15;
        c.G = 0;
        listNode.Add(a);
        listNode.Add(b);
        listNode.Add(c);

        int i = 0;

        listNode.Sort();

        i = 20;

        Vector2Int dest = new Vector2Int(3, 5);
        Node destNode = new Node(5, 2);
        if (destNode == dest)
            Debug.Log("같다");
    }

    void PrintList(List<int> list)
    {
        string str = "";
        foreach (int item in list)
        {
            str += $"{item} -> ";
        }
        Debug.Log(str +"End");
    }


    protected override void Test1(InputAction.CallbackContext context)
    {
        List<Vector2Int> path = AStar.PathFind(map, start, end);
        pathline.DrawPath(map, path);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        //list.Sort();
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        
    }
}
