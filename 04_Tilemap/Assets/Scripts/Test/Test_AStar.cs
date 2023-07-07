using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AStar : TestBase
{
    List<int> list = new List<int>();
    List<Node> listNode = new List<Node>();

    private void Start()
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

        Vector2Int dest = new Vector2Int(3,5);
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
        PrintList(list);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        list.Sort();
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        
    }
}
