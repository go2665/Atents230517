using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    /// <summary>
    /// 이 플레이어의 보드
    /// </summary>
    protected Board board;
    public Board Board => board;

    /// <summary>
    /// 이 플레이어가 가지고 있는 함선들
    /// </summary>
    protected Ship[] ships;
    public Ship[] Ships => ships;

    /// <summary>
    /// 아직 침몰하지 않은 함선의 수
    /// </summary>
    protected int remainShipCount;
    public bool IsDepeat => remainShipCount < 1;

    /// <summary>
    /// 이번 턴의 행동이 완료되었는지 여부
    /// </summary>
    bool isActionDone = false;
    public bool IsActionDone => isActionDone;

    /// <summary>
    /// 대전 상대
    /// </summary>
    protected PlayerBase opponent;

    /// <summary>
    /// 일반 공격 후보 인덱스
    /// </summary>
    List<int> attackIndeices;

    /// <summary>
    /// 우선 순위가 높은 공격 후보 인덱스
    /// </summary>
    List<int> attackHighIndeices;

    /// <summary>
    /// 마지막으로 공격을 성공한 위치
    /// </summary>
    Vector2Int lastAttackSuccessPosition;

    /// <summary>
    /// 이전 공격이 성공하지 않았다고 표시하는 변수
    /// </summary>
    readonly Vector2Int NOT_SUCCESS = -Vector2Int.one;

    /// <summary>
    /// 이웃 위치 확인용
    /// </summary>
    readonly Vector2Int[] neighbors = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };


    /// <summary>
    /// 이 플레이어의 공격이 실패했음을 알리는 델리게이트(파라메터:자기자신)
    /// </summary>
    public Action<PlayerBase> onAttackFail;

    /// <summary>
    /// 이 플레이어의 행동이 끝났음을 알리는 델리게이트
    /// </summary>
    public Action onActionEnd;

    /// <summary>
    /// 이 플레이어가 패배했음을 알리는 델리게이트(파라메타:자기자신)
    /// </summary>
    public Action<PlayerBase> onDefeat;


    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    protected virtual void Start()
    {
        // 배 생성
        int shipTypeCount = ShipManager.Inst.ShipTypeCount;
        ships = new Ship[shipTypeCount];
        for(int i = 0; i < shipTypeCount; i++)
        {
            ShipType shipType = (ShipType)(i + 1);
            ships[i] = ShipManager.Inst.MakeShip(shipType, transform);  // 배 종류별로 만들기

            ships[i].onSinking += OnShipDestroy;                // 함선이 침몰하면 OnShipDestroy 실행

            board.onShipAttacked[shipType] = ships[i].OnHitted; // 배가 맞을 때마다 실행될 함수 등록
        }
        remainShipCount = shipTypeCount;                    // 배 갯수 설정

        // 일반 공격 후보 지역 만들기
        int fullSize = Board.BoardSize * Board.BoardSize;
        int[] temp = new int[fullSize];
        for(int i=0;i<fullSize;i++)
        {
            temp[i] = i;        // 순서대로 0~99까지 채우기
        }
        Util.Shuffle(temp);     // 채운것 섞기
        attackIndeices = new List<int>(temp);   // 섞은 것을 기준으로 리스트만들기

        // 우선 순위가 높은 공격 후보지역 만들기(비어있음)
        attackHighIndeices = new List<int>(10);

        // 공격 관련 변수(이전에 공격이 성공한 적 없다고 표시)
        lastAttackSuccessPosition = NOT_SUCCESS;
    }

    // 턴 관리용 함수 ------------------------------------------------------------------------------

    /// <summary>
    /// 턴이 시작될 때 플레이어가 처리해야 할 일을 수행하는 함수
    /// </summary>
    /// <param name="_">현재 몇번째 턴인지. 사용안함.</param>
    public virtual void OnPlayerTurnStart(int _)
    {
        isActionDone = false;
    }

    /// <summary>
    /// 턴이 종료될 때 플레이어가 처리해야 할 일을 수행하는 함수
    /// </summary>
    public virtual void OnPlayerTurnEnd()
    {
        // 기능없음
    }

    // 공격 관련 함수 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 적의 특정 위치를 공격하는 함수
    /// </summary>
    /// <param name="attackGridPos">공격하는 위치</param>
    public void Attack(Vector2Int attackGridPos)
    {
        Debug.Log($"{gameObject.name}가 ({attackGridPos.x},{attackGridPos.y})를 공격했습니다.");
        bool result = opponent.Board.OnAttacked(attackGridPos);     // 상대방 보드에 공격
        if(result)  // 공격 성공
        {
            // 이전 턴의 공격 성공 여부 확인
            if( lastAttackSuccessPosition != NOT_SUCCESS )
            {
                // 한턴 앞의 공격이 성공했다.
                AddHighFromTwoPoint(attackGridPos, lastAttackSuccessPosition);
            }
            else
            {
                // 처음 성공한 공격이다.
                AddHighFromNeighbors(attackGridPos);
            }
            lastAttackSuccessPosition = attackGridPos;  // 공격 성공했다고 표시
        }
        else
        {
            lastAttackSuccessPosition = NOT_SUCCESS;
        }
    }

    /// <summary>
    /// 적의 특정 위치를 공격하는 함수
    /// </summary>
    /// <param name="worldPos">공격하는 위치(월드)</param>
    public void Attack(Vector3 worldPos)
    {
        Attack(opponent.Board.WorldToGrid(worldPos));
    }

    /// <summary>
    /// 적의 특정 위치를 공격하는 함수
    /// </summary>
    /// <param name="index">공격하는 위치의 인덱스</param>
    public void Attack(int index)
    {
        Attack(Board.IndexToGrid(index));
    }

    /// <summary>
    /// 자동 공격 함수. CPU플레이어나 인간플레이어가 타임아웃되었을 때 사용.
    /// </summary>
    public void AutoAttack()
    {
        // 확인할 순서 : 한줄로 공격이 성공했나? -> 이전 공격이 성공했나? -> 무작위로 공격
        // 1. 무작위로 공격(중복은 안되어야 함)
        // 2. 공격이 성공했을 때 다음 공격은 공격 성공 위치의 위아래좌우 4방향 중 하나를 공격.
        // 3. 공격이 한줄로 성공했을 때 다음 공격은 줄의 양끝 바깥 중 하나를 공격

        // 4. 함선을 침몰시키면 우선순위가 높은 후보지역은 모두 제거한다.

        int target;
        if( attackHighIndeices.Count > 0 )  // 우선 순위 높은 후보가 있는지 확인
        {
            target = attackHighIndeices[0]; // 우선 순위가 높은 후보가 있으면 첫번째것 사용
            RemoveHigh(target);             // 높은 우선 순위 목록에서 제거
            attackIndeices.Remove(target);  // 일반 우선 순위 목록에서 제거
        }
        else
        {
            target = attackIndeices[0];     // 일반 우선 순위 목록의 첫번째 것 사용.
            attackIndeices.RemoveAt(0);     // 일반 우선 순위 목록에서 제거
        }

        Attack(target);
    }

    /// <summary>
    /// 높은 우선 순위 목록에 인덱스를 추가하는 함수
    /// </summary>
    /// <param name="index">추가할 인덱스</param>
    private void AddHigh(int index)
    {
        if( !attackHighIndeices.Contains(index) )   // 안들어 있을 때만 추가
        {
            attackHighIndeices.Insert(0, index);    // 항상 앞에 추가(새로 추가되는 위치가 성공 확률이 더 높아보임)
        }
    }

    /// <summary>
    /// 현재 성공지점의 양 끝을 우선순위 높은 후보지역으로 만드는 함수
    /// </summary>
    /// <param name="now">지금 공격한 위치</param>
    /// <param name="last">이전에 공격한 위치</param>
    private void AddHighFromTwoPoint(Vector2Int now, Vector2Int last)
    {

    }

    /// <summary>
    /// start에서 end 한칸 앞까지 모두 공격 성공이었는지 체크하는 함수
    /// </summary>
    /// <param name="start">확인 시작점</param>
    /// <param name="end">확인 종료지점</param>
    /// <param name="isHorizontal">true면 가로로 체크, false면 세로로 체크</param>
    /// <returns>true면 같은 줄이고 그 사이는 모두 공격 성공, false면 다른 줄이거나 중간에 공격 실패가 있다.</returns>
    private bool InSuccessLine(Vector2Int start, Vector2Int end, bool isHorizontal)
    {
        bool result = true;
        return result;
    }

    /// <summary>
    /// grid 주변 사방을 모두 우선 순위가 높은 후보지역에 추가흔 ㄴ함수
    /// </summary>
    /// <param name="grid">중심 위치</param>
    private void AddHighFromNeighbors(Vector2Int grid)
    {
        Util.Shuffle(neighbors);    // 다양성을 위해 한번 섞기
        foreach(Vector2Int neighbor in neighbors)
        {
            Vector2Int pos = grid + neighbor;
            if( Board.IsInBoard(pos) && opponent.Board.IsAttackable(pos))   // 보드 안이고 공격 가능할 때만 추가
            {
                AddHigh(Board.GridToIndex(pos));
            }
        }
    }

    /// <summary>
    /// 높은 우선 순위 목록에서 제거하는 함수
    /// </summary>
    /// <param name="index">제거할 인덱스</param>
    private void RemoveHigh(int index)
    {
        if(attackHighIndeices.Contains(index))  // 있으면
        {
            attackHighIndeices.Remove(index);   // 제거한다.
        }
    }

    // 함선 배치용 함수 ----------------------------------------------------------------------------
    
    /// <summary>
    /// 자동으로 이 플레이어의 보드에 함선을 배치하는 함수
    /// </summary>
    /// <param name="isShowShips">함선배치 후 보이게 할 것이면 true, 아니면 false</param>
    public void AutoShipDeployment(bool isShowShips)
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
        foreach (var ship in ships)
        {
            if (ship.IsDeployed)
            {
                int[] shipIndice = new int[ship.Size];
                for (int i = 0; i < ship.Size; i++)
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
        foreach (var ship in ships)
        {
            if (!ship.IsDeployed)    // 배가 배치되지 않은 것만 처리
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
                        for (int i = 1; i < shipPositions.Length; i++)
                        {
                            int bodyIndex = Board.GridToIndex(shipPositions[i]);    // 몸통부분 인덱스 가져와서
                            if (!high.Contains(bodyIndex))   // high에 몸통부분이 있는지 확인
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
                while (failDeployment && counter < 1000)
                {
                    int headIndex = low[0];                 // low에서 하나 꺼내고
                    low.RemoveAt(0);
                    grid = Board.IndexToGrid(headIndex);

                    failDeployment = !board.IsShipDeplymentAvailable(ship, grid, out shipPositions);    // 배치 시도하고
                    if (failDeployment)
                    {
                        low.Add(headIndex);                 // 실패하면 low에 되돌리기
                    }
                    counter++;
                }

                // high, low 둘다 실패했을 때
                if (failDeployment)
                {
                    Debug.LogWarning("함선 자동배치 실패!!!!!");    // 이건 문제가 있다(맵을 키우거나, 배 종류를 줄여야 함)
                    return;
                }

                // 실제 배치
                board.ShipDeployment(ship, grid);
                ship.gameObject.SetActive(true);

                // 배치된 위치를 high와 low에서 제거
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach (var pos in shipPositions)
                {
                    tempList.Add(Board.GridToIndex(pos));   // 배치된 위치를 인덱스로 변환해서 저장
                }
                foreach (var index in tempList)
                {
                    high.Remove(index);     // high에서 인덱스 제거
                    low.Remove(index);      // low에서 인덱스 제거
                }

                // 함선 주변 위치를 low로 보내기
                List<int> toLow = GetShipAroundPositions(ship); // 배치된 배 주변 위치 구하기
                foreach (var index in toLow)
                {
                    if (high.Contains(index))        // high에 해당 위치가 있으면
                    {
                        low.Add(index);             // low에 넣고
                        high.Remove(index);         // high에서 제거
                    }
                }
            }
        }
    }

    /// <summary>
    /// 함선의 주변 위치들의 인덱스를 구하는 함수
    /// </summary>
    /// <param name="ship">주변 위치를 구할 함선</param>
    /// <returns>함선 주변 위치의 인덱스를 저장한 리스트</returns>
    private List<int> GetShipAroundPositions(Ship ship)
    {
        List<int> result = new List<int>(ship.Size * 2 + 6);

        if (ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South)
        {
            foreach (var pos in ship.Positions)
            {
                result.Add(Board.GridToIndex(pos + Vector2Int.right));
                result.Add(Board.GridToIndex(pos + Vector2Int.left));
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.North)
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

    /// <summary>
    /// 모든 함선의 배치를 취소하는 함수
    /// </summary>
    public void UndoAllShipDeployment()
    {
        board.ResetBoard(ships);
    }

    // 함선 침몰 및 패배 처리 -----------------------------------------------------------------------
    
    /// <summary>
    /// 내가 가진 특정 배가 파괴되었을 때 실행될 함수
    /// </summary>
    /// <param name="ship">파괴된 배</param>
    private void OnShipDestroy(Ship ship)
    {
        remainShipCount--;          // 남은 배 갯수 감소
        Debug.Log($"[{ship.Type}]이 침몰했습니다. {remainShipCount}척의 배가 남아있습니다.");
        if(remainShipCount < 1)     // 0이하가 되면
        {
            OnDefeat();             // 패배
        }
    }

    /// <summary>
    /// 모든 배가 침몰했을 때 실행될 함수
    /// </summary>
    private void OnDefeat()
    {
        Debug.Log($"[{this.gameObject.name}] 패배");
        onDefeat?.Invoke(this);
    }

    // 기타 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 입력 받은 종류의 함선을 리턴하는 함수
    /// </summary>
    /// <param name="shipType">리턴할 함선 종류</param>
    /// <returns>shipType이 None이면 null 리턴, 그 외는 함선에 맞는 참조 리턴</returns>
    public Ship GetShip(ShipType shipType)
    {
        return (shipType != ShipType.None) ? ships[(int)shipType - 1] : null;
    }
    
    /// <summary>
    /// 초기화 함수. 게임 시작 직전 상태로 변경
    /// </summary>
    public void Clear()
    {

    }

    /// <summary>
    /// 테스트용 함수
    /// </summary>
    /// <param name="player"></param>
    public void Test_SetOpponent(PlayerBase player)
    {
        opponent = player;
    }
}
