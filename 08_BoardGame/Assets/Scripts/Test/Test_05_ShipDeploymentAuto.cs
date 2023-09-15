using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_05_ShipDeploymentAuto : Test_04_ShipDeployment
{
    public Button reset;
    public Button random;
    public Button resetAndRandom;

    protected override void Start()
    {
        base.Start();

        reset.onClick.AddListener(ClearBoard);
        random.onClick.AddListener(AutoShipDeplyment);
        resetAndRandom.onClick.AddListener(() =>
        {
            ClearBoard();
            AutoShipDeplyment();
        });
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

        // 배를 하나씩 배치 시작
        foreach(var ship in testShips)
        {
            if(!ship.IsDeployed)    // 배가 배치되지 않은 것만 처리
            {
                ship.RandomRotate();            // 배를 적당히 회전 시키기

                bool failDeployment = true;
                int counter = 0;
                Vector2Int grid;
                Vector2Int[] shipPositions;

                // 우선 우선순위가 높은 곳을 선택
                do
                {
                    int headIndex = high[0];                // high의 첫번째 꺼내서 headIndex에 저장
                    high.RemoveAt(0);

                    grid = Board.IndexToGrid(headIndex);

                    // headIndex에 배치가 가능한지 확인
                    failDeployment = !board.IsShipDeplymentAvailable(ship, grid, out shipPositions);    
                    if (failDeployment)
                    {
                        high.Add(headIndex);    // 불가능하면 headIndex를 high에 되돌리기
                    }
                    else
                    {                        
                        // 몸통부분이 모두 high에 있는지 확인
                        for(int i=1;i<shipPositions.Length;i++)
                        {
                            int bodyIndex = Board.GridToIndex(shipPositions[i]);    // 몸통부분 인덱스 가져와서
                            if(!high.Contains(bodyIndex))   // high에 몸통부분이 있는지 확인
                            {
                                high.Add(headIndex);        // high에 몸통부분이 없으면 실패로 처리
                                failDeployment = true;
                                break;
                            }
                        }
                    }
                    counter++;  // 무한루프 방지용
                
                    // 실패하고, 시도횟수가 10번 미만이고, high에 후보지역이 남아있으면 반복
                } while (failDeployment && counter < 10 && high.Count > 0);

                // 필요할 경우 우선순위가 낮은 곳 처리
                counter = 0;
                while(failDeployment && counter < 1000)
                {
                    int headIndex = low[0];                 // low에서 하나 꺼내고
                    low.RemoveAt(0);
                    grid = Board.IndexToGrid(headIndex);

                    failDeployment = !board.IsShipDeplymentAvailable(ship, grid, out shipPositions);    // 배치 시도하고
                    if( failDeployment)
                    {
                        low.Add(headIndex);                 // 실패하면 low에 되돌리기
                    }
                    counter++;
                }

                // high, low 둘다 실패했을 때
                if(failDeployment)
                {
                    Debug.LogWarning("함선 자동배치 실패!!!!!");    // 이건 문제가 있다(맵을 키우거나, 배 종류를 줄여야 함)
                    return;
                }

                // 실제 배치
                board.ShipDeployment(ship, grid);
                ship.gameObject.SetActive(true);

                // 배치된 위치를 high와 low에서 제거
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach(var pos in shipPositions)
                {
                    tempList.Add(Board.GridToIndex(pos));   // 배치된 위치를 인덱스로 변환해서 저장
                }
                foreach(var index in tempList)
                {
                    high.Remove(index);     // high에서 인덱스 제거
                    low.Remove(index);      // low에서 인덱스 제거
                }

                // 함선 주변 위치를 low로 보내기
                List<int> toLow = GetShipAroundPositions(ship); // 배치된 배 주변 위치 구하기
                foreach(var index in toLow)
                {
                    if(high.Contains(index))        // high에 해당 위치가 있으면
                    {
                        low.Add(index);             // low에 넣고
                        high.Remove(index);         // high에서 제거
                    }
                }
            }
        }
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
        board.ResetBoard(testShips);
    }

    // 실습
    // 1. 배치초기화 버튼을 누르면 배치되어있는 모든 배가 배치 해제 된다.
    // 2. 랜덤 버튼을 누르면 아직 배치되어 있지 않은 모든 배가 자동으로 배치된다.
    // 3. 랜덤 배치되는 위치는 우선순위가 높은 위치와 낮은 위치가 있다.(우선 순위가 높은 위치가 주가 되어야 한다.)
    // 4. 랜덤 배치시 보드의 가장자리와 다른 배의 주변 위치는 우선순위가 낮다.
}
