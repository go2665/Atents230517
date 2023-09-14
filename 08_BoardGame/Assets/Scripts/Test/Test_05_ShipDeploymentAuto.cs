using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_05_ShipDeploymentAuto : Test_04_ShipDeployment
{
    public Button reset;
    public Button random;

    protected override void Start()
    {
        base.Start();

        reset.onClick.AddListener(ClearBoard);
        random.onClick.AddListener(AutoShipDeplyment);
    }

    private void AutoShipDeplyment()
    {
        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> high = new(maxCapacity);
        List<int> low = new(maxCapacity);

        // 가장자리 부분을 low로 설정
        for (int i = 0; i < maxCapacity; i++)
        {
            if (i % Board.BoardSize == 0                            // 0,10,20,30,40,50,60,70,80,90
                || i % Board.BoardSize == (Board.BoardSize - 1)     // 9,19,29,39,49,59,69,79,89,99
                || i > 0 && i < (Board.BoardSize - 1)                 // 1~8
                || (Board.BoardSize * (Board.BoardSize - 1) < i && i < (Board.BoardSize * Board.BoardSize - 1))) // 91~98
            {
                low.Add(i);
            }
            else
            {
                high.Add(i);
            }
        }

        // 이미 배치된 배 주변을 low로 설정
        foreach(var ship in testShips)
        {
            if(ship.IsDeployed)
            {
                int[] shipIndice = new int[ship.Size];
                for(int i=0;i<ship.Size; i++)
                {
                    shipIndice[i] = Board.GridToIndex(ship.Positions[i]);
                }

                foreach (var index in shipIndice)
                { 
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPositions(ship);
                foreach (var index in toLow)
                {
                    high.Remove(index);
                    low.Add(index);
                }
            }
        }

        // high와 low 내부 순서 섞기(각각)
        int[] temp = high.ToArray();
        Util.Shuffle(temp);
        high = new(temp);
        temp = low.ToArray();
        Util.Shuffle(temp);
        low = new(temp);
    }

    private List<int> GetShipAroundPositions(Ship ship)
    {
        List<int> result = new List<int>(ship.Size * 2 + 6);

        if(ship.Direction == ShipDirection.North ||  ship.Direction == ShipDirection.South)
        {
            foreach(var pos in ship.Positions)
            {
                result.Add( Board.GridToIndex(pos + Vector2Int.right) );
                result.Add( Board.GridToIndex(pos + Vector2Int.left) );
            }

            Vector2Int head;
            Vector2Int tail;
            if( ship.Direction == ShipDirection.North )
            {
                head = ship.Positions[0] + Vector2Int.down;
                tail = ship.Positions[^1] + Vector2Int.up;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.up;
                tail = ship.Positions[^1] + Vector2Int.down;
            }
            result.Add(Board.GridToIndex(head));
            result.Add(Board.GridToIndex(head + Vector2Int.left));
            result.Add(Board.GridToIndex(head + Vector2Int.right));
            result.Add(Board.GridToIndex(tail));
            result.Add(Board.GridToIndex(tail + Vector2Int.left));
            result.Add(Board.GridToIndex(tail + Vector2Int.right));
        }
        else
        {
            foreach (var pos in ship.Positions)
            {
                result.Add(Board.GridToIndex(pos + Vector2Int.up));
                result.Add(Board.GridToIndex(pos + Vector2Int.down));
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.East)
            {
                head = ship.Positions[0] + Vector2Int.right;
                tail = ship.Positions[^1] + Vector2Int.left;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.left;
                tail = ship.Positions[^1] + Vector2Int.right;
            }
            result.Add(Board.GridToIndex(head));
            result.Add(Board.GridToIndex(head + Vector2Int.up));
            result.Add(Board.GridToIndex(head + Vector2Int.down));
            result.Add(Board.GridToIndex(tail));
            result.Add(Board.GridToIndex(tail + Vector2Int.up));
            result.Add(Board.GridToIndex(tail + Vector2Int.down));
        }
        result.RemoveAll((x) => x == Board.NOT_VALID_INDEX);

        return result;
    }

    private void ClearBoard()
    {
        foreach(var ship in testShips)
        {
            board.UndoShipDeployment(ship);
        }
    }

    // 실습
    // 1. 배치초기화 버튼을 누르면 배치되어있는 모든 배가 배치 해제 된다.
    // 2. 랜덤 버튼을 누르면 아직 배치되어 있지 않은 모든 배가 자동으로 배치된다.
    // 3. 랜덤 배치되는 위치는 우선순위가 높은 위치와 낮은 위치가 있다.(우선 순위가 높은 위치가 주가 되어야 한다.)
    // 4. 랜덤 배치시 보드의 가장자리와 다른 배의 주변 위치는 우선순위가 낮다.
}
