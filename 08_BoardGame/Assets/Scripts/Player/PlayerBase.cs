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
    /// 성공한 공격 회수
    /// </summary>
    int successAttackCount;
    public int SuccessAttackCount => successAttackCount;

    /// <summary>
    /// 실패한 공격 회수
    /// </summary>
    int failAttackCount;
    public int FailAttackCount => failAttackCount;

    /// <summary>
    /// 대전 상대
    /// </summary>
    protected PlayerBase opponent;

    /// <summary>
    /// 일반 공격 후보 인덱스
    /// </summary>
    List<int> attackIndices;

    /// <summary>
    /// 우선 순위가 높은 공격 후보 인덱스
    /// </summary>
    List<int> attackHighIndices;

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
    /// 공격 후보지역 표시용 프리팹
    /// </summary>
    public GameObject highMarkPrefab;

    /// <summary>
    /// 공격 후보지역 표시용 오브젝트의 부모 트랜스폼
    /// </summary>
    Transform highMarksParent;

    /// <summary>
    /// 공격 후보지역을 모아 놓은 딕셔너리
    /// </summary>
    Dictionary<int, GameObject> highMarks;

    /// <summary>
    /// 이번 공격으로 상대방의 배가 침몰했는지 알려주는 변수
    /// </summary>
    bool opponentShipDestroyed = false;


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

    /// <summary>
    /// 초기화가 되었다는 표시
    /// </summary>
    bool isInitialized = false;




    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();
        highMarksParent = transform.GetChild(1);            // 공격 후보지역의 부모 찾기
        highMarks = new Dictionary<int, GameObject>(10);    // 딕셔너리 생성
    }

    protected virtual void Start()
    {
        Initialize();
    }

    protected void Initialize()
    {
        if(!isInitialized)
        {
            // 배 생성
            int shipTypeCount = ShipManager.Inst.ShipTypeCount;
            ships = new Ship[shipTypeCount];
            for (int i = 0; i < shipTypeCount; i++)
            {
                ShipType shipType = (ShipType)(i + 1);
                ships[i] = ShipManager.Inst.MakeShip(shipType, transform);  // 배 종류별로 만들기

                ships[i].onSinking += OnShipDestroy;                        // 함선이 침몰하면 OnShipDestroy 실행

                board.onShipAttacked[shipType] = () => opponent.successAttackCount++;   // 내 보드가 맞았으니까 상대방의 공격이 성공했다.
                board.onShipAttacked[shipType] += ships[i].OnHitted;        // 배가 맞을 때마다 실행될 함수 등록
            }
            remainShipCount = shipTypeCount;                    // 배 갯수 설정

            // 보드 초기화
            Board.ResetBoard(ships);

            // 일반 공격 후보 지역 만들기
            int fullSize = Board.BoardSize * Board.BoardSize;
            int[] temp = new int[fullSize];
            for (int i = 0; i < fullSize; i++)
            {
                temp[i] = i;        // 순서대로 0~99까지 채우기
            }
            Util.Shuffle(temp);     // 채운것 섞기
            attackIndices = new List<int>(temp);   // 섞은 것을 기준으로 리스트만들기

            // 우선 순위가 높은 공격 후보지역 만들기(비어있음)
            attackHighIndices = new List<int>(10);

            // 공격 관련 변수(이전에 공격이 성공한 적 없다고 표시)
            lastAttackSuccessPosition = NOT_SUCCESS;

            // 전투 씬일 때만 턴매니저 사용
            if (GameManager.Inst.GameState == GameState.Battle)
            {
                // 행동이 완료되면 턴 진행 체크
                onActionEnd += TurnManager.Inst.CheckTurnEnd;

                // 패배하면 턴 메니저를 정지 시키기
                onDefeat += (_) => TurnManager.Inst.TurnStop();

                TurnManager temp2 = TurnManager.Inst;

                // 턴 시작 초기화 함수와 종로 함수 연결
                TurnManager.Inst.onTurnStart += OnPlayerTurnStart;
                TurnManager.Inst.onTurnEnd += OnPlayerTurnEnd;
            }

            successAttackCount = 0;     // 결과 UI가 뜨는 타이밍 수정 필요
            failAttackCount = 0;

            OnPlayerTurnStart(0);
            isInitialized = true;
        }
    }

    // 턴 관리용 함수 ------------------------------------------------------------------------------

    /// <summary>
    /// 턴이 시작될 때 플레이어가 처리해야 할 일을 수행하는 함수
    /// </summary>
    /// <param name="_">현재 몇번째 턴인지. 사용안함.</param>
    protected virtual void OnPlayerTurnStart(int _)
    {
        isActionDone = false;
    }

    /// <summary>
    /// 턴이 종료될 때 플레이어가 처리해야 할 일을 수행하는 함수
    /// </summary>
    protected virtual void OnPlayerTurnEnd()
    {
        if(!IsActionDone)
        {
            AutoAttack();
        }
    }

    // 공격 관련 함수 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 적의 특정 위치를 공격하는 함수
    /// </summary>
    /// <param name="attackGridPos">공격하는 위치</param>
    public void Attack(Vector2Int attackGridPos)
    {
        // 행동을 안했고, 공격 지점이 보드 안이고, 공격 가능한 위치일때만 처리
        if (!IsActionDone && Board.IsInBoard(attackGridPos) && opponent.Board.IsAttackable(attackGridPos))  
        {
            Debug.Log($"{gameObject.name}가 ({attackGridPos.x},{attackGridPos.y})를 공격했습니다.");
            bool result = opponent.Board.OnAttacked(attackGridPos);     // 상대방 보드에 공격
            if (result)  // 공격 성공
            {
                if (opponentShipDestroyed)
                {
                    // 이번 공격으로 적의 함선이 침몰했으면
                    RemoveAllHigh();                // 후보지역 전부 제거
                    opponentShipDestroyed = false;  // 표시 리셋
                }
                else
                {
                    // 이번 공격으로 적의 함선이 침몰하지 않은 경우 

                    // 이전 턴의 공격 성공 여부 확인
                    if (lastAttackSuccessPosition != NOT_SUCCESS)
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
            }
            else
            {
                failAttackCount++;
                lastAttackSuccessPosition = NOT_SUCCESS;
                onAttackFail?.Invoke(this);
            }

            int attackIndex = Board.GridToIndex(attackGridPos);
            RemoveHigh(attackIndex);            // 공격한 위치는 더 이상 후보지역이 아님
            attackIndices.Remove(attackIndex);

            isActionDone = true;    // 행동 완료했다고 표시
            onActionEnd?.Invoke();  // 행동 완료했다고 알림
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
        if (!IsActionDone)
        {
            // 확인할 순서 : 한줄로 공격이 성공했나? -> 이전 공격이 성공했나? -> 무작위로 공격
            // 1. 무작위로 공격(중복은 안되어야 함)
            // 2. 공격이 성공했을 때 다음 공격은 공격 성공 위치의 위아래좌우 4방향 중 하나를 공격.
            // 3. 공격이 한줄로 성공했을 때 다음 공격은 줄의 양끝 바깥 중 하나를 공격

            // 4. 함선을 침몰시키면 우선순위가 높은 후보지역은 모두 제거한다.

            int target;
            if (attackHighIndices.Count > 0)  // 우선 순위 높은 후보가 있는지 확인
            {
                target = attackHighIndices[0]; // 우선 순위가 높은 후보가 있으면 첫번째것 사용
                RemoveHigh(target);             // 높은 우선 순위 목록에서 제거
                attackIndices.Remove(target);  // 일반 우선 순위 목록에서 제거
            }
            else
            {
                target = attackIndices[0];     // 일반 우선 순위 목록의 첫번째 것 사용.
                attackIndices.RemoveAt(0);     // 일반 우선 순위 목록에서 제거
            }

            Attack(target);
        }
    }

    /// <summary>
    /// 높은 우선 순위 목록에 인덱스를 추가하는 함수
    /// </summary>
    /// <param name="index">추가할 인덱스</param>
    private void AddHigh(int index)
    {
        if( !attackHighIndices.Contains(index) )   // 안들어 있을 때만 추가
        {
            attackHighIndices.Insert(0, index);    // 항상 앞에 추가(새로 추가되는 위치가 성공 확률이 더 높아보임)

            // 후보지역 표시
            GameObject obj = Instantiate(highMarkPrefab, highMarksParent);  // 프리팹 생성
            obj.transform.position = opponent.Board.IndexToWorld(index) + Vector3.up * 0.5f;    // 위치 배치
            Vector2Int grid = Board.IndexToGrid(index);
            obj.gameObject.name = $"High_({grid.x},{grid.y})";  // 이름 알아보기 쉽게 변경

            obj.SetActive(GameManager.Inst.IsTestMode);         // 테스트 모드일 때만 보이기

            highMarks[index] = obj;                             // 딕셔너리에 기록
        }
    }

    /// <summary>
    /// 현재 성공지점의 양 끝을 우선순위 높은 후보지역으로 만드는 함수
    /// </summary>
    /// <param name="now">지금 공격한 위치</param>
    /// <param name="last">이전에 공격한 위치</param>
    private void AddHighFromTwoPoint(Vector2Int now, Vector2Int last)
    {
        if(InSuccessLine(last, now, true))
        {
            // 가로 방향으로 붙어있는 한 줄이다. ((연속으로 공격이 성공 && 붙어있다) == 같은배다)

            // 같은 가로선 상에 있지 않은 높은 후보지역 모두 제거
            Vector2Int grid;
            List<int> dels = new List<int>();
            foreach(var index in attackHighIndices)
            {
                grid = Board.IndexToGrid(index);
                if(grid.y != now.y)
                {
                    dels.Add(index);    // foreach안에서 컬랙션 노드를 제거할 수 없다.
                }                    
            }
            foreach(var index in dels)  
            {
                RemoveHigh(index);      // 따로 제거
            }

            // now에서 왼쪽 오른쪽으로 이동하며 양끝을 찾아 높은 후보지역에 추가
            grid = now;
            for(int i = now.x - 1; i>-1; i--)   // i는 now의 왼쪽칸에서 보드 끝(0)까지 진행
            {
                grid.x = i;
                if(!Board.IsInBoard(grid))                      // 우선 보드 안쪽
                    break;
                if (opponent.Board.IsAttackFailPosition(grid))  // 공격실패가 아니여야 함
                    break;
                if(opponent.Board.IsAttackable(grid))           // 공격 가능한 지점이여야 한다.
                {
                    AddHigh(Board.GridToIndex(grid));           // 높은 후보지역에 추가
                    break;
                }
            }

            for (int i = now.x + 1; i < Board.BoardSize; i++)   // i는 now의 오른쪽칸에서 보드 끝(Board.BoardSize)까지 진행
            {
                grid.x = i;
                if (!Board.IsInBoard(grid))                     // 우선 보드 안쪽
                    break;
                if (opponent.Board.IsAttackFailPosition(grid))  // 공격실패가 아니여야 함
                    break;
                if (opponent.Board.IsAttackable(grid))          // 공격 가능한 지점이여야 한다.
                {
                    AddHigh(Board.GridToIndex(grid));           // 높은 후보지역에 추가
                    break;
                }
            }

        }
        else if(InSuccessLine(last, now, false))
        {
            // 세로 방향으로 붙어있는 한 줄이다. ((연속으로 공격이 성공 && 붙어있다) == 같은배다)

            // 같은 세로선 상에 있지 않은 높은 후보지역 모두 제거
            Vector2Int grid;
            List<int> dels = new List<int>();
            foreach (var index in attackHighIndices)
            {
                grid = Board.IndexToGrid(index);
                if (grid.x != now.x)
                {
                    dels.Add(index);    // foreach안에서 컬랙션 노드를 제거할 수 없다.
                }
            }
            foreach (var index in dels)
            {
                RemoveHigh(index);      // 따로 제거
            }

            // now에서 아래쪽 위쪽으로 이동하며 양끝을 찾아 높은 후보지역에 추가
            grid = now;
            for (int i = now.y - 1; i > -1; i--)   // i는 now의 왼쪽칸에서 보드 끝(0)까지 진행
            {
                grid.y = i;
                if (!Board.IsInBoard(grid))                      // 우선 보드 안쪽
                    break;
                if (opponent.Board.IsAttackFailPosition(grid))  // 공격실패가 아니여야 함
                    break;
                if (opponent.Board.IsAttackable(grid))           // 공격 가능한 지점이여야 한다.
                {
                    AddHigh(Board.GridToIndex(grid));           // 높은 후보지역에 추가
                    break;
                }
            }

            for (int i = now.y + 1; i < Board.BoardSize; i++)   // i는 now의 오른쪽칸에서 보드 끝(Board.BoardSize)까지 진행
            {
                grid.y = i;
                if (!Board.IsInBoard(grid))                     // 우선 보드 안쪽
                    break;
                if (opponent.Board.IsAttackFailPosition(grid))  // 공격실패가 아니여야 함
                    break;
                if (opponent.Board.IsAttackable(grid))          // 공격 가능한 지점이여야 한다.
                {
                    AddHigh(Board.GridToIndex(grid));           // 높은 후보지역에 추가
                    break;
                }
            }
        }
        else
        {
            // 같은 줄이 아니다 == 다른 배다
            AddHighFromNeighbors(now);  // 다른 배니까 주변 지역 모두 추가
        }
    }

    /// <summary>
    /// start에서 end까지 모두 공격 성공이었는지 체크하는 함수
    /// </summary>
    /// <param name="start">확인 시작점</param>
    /// <param name="end">확인 종료지점</param>
    /// <param name="isHorizontal">true면 가로로 체크, false면 세로로 체크</param>
    /// <returns>true면 같은 줄이고 그 사이는 모두 공격 성공, false면 다른 줄이거나 중간에 공격 실패가 있다.</returns>
    private bool InSuccessLine(Vector2Int start, Vector2Int end, bool isHorizontal)
    {
        bool result = true;         // 함수 최종 결과값
        Vector2Int pos = start;     // start에서 end까지 순차적으로 위치를 저장할 임시 변수
        int dir = 1;                // 진행방향이 정방향인지 역방향인지 표시하는 변수(1 or -1)

        if(isHorizontal)
        {
            // 가로 방향의 선
            if( start.y == end.y )      // y가 같아야 가로
            {
                if( start.x > end.x )   // 정방향인지 역방향인지 확인
                {
                    dir = -1;           // start가 end보다 크면 역방향
                }

                start.x *= dir;         // for문에서 비교를 할 때 공통으로 처리하기 위해 설정
                end.x *= dir;
                end.x++;                // end 위치까지 확인하기 위해 1더함

                for (int i = start.x; i < end.x; i++)   // start에서 end로 i가 증가
                {
                    pos.x = i * dir;    // 이미 변수 갱신(dir이 -1일 때는 다시 곱해서 뒤집기)
                    if (!opponent.Board.IsAttackSuccessPosition(pos))   // 공격이 성공한 지점인지 확인
                    {
                        result = false; // 하나라도 실패했다면 결과는 false
                        break;
                    }
                }
            }
            else
            {
                result = false;         // y가 다르면 가로로 된 직선이 아니다. 그래서 실패
            }
        }
        else
        {
            // 세로 방향의 선
            if (start.x == end.x)       // x가 같아야 세로
            {
                if (start.y > end.y)    // 역방향인지 확인
                {
                    dir = -1;
                }

                start.y *= dir;         // for문에서 비교를 할 때 공통으로 처리하기 위해 설정
                end.y *= dir;
                end.y++;                // end 위치까지 확인하기 위해 1더함

                for (int i = start.y; i < end.y; i ++)  // start에서 end로 i가 증가
                {
                    pos.y = i * dir;                                    // 이미 변수 갱신(dir이 -1일 때는 다시 곱해서 뒤집기)
                    if (!opponent.Board.IsAttackSuccessPosition(pos))   // 공격이 성공한 지점인지 확인
                    {
                        result = false; // 하나라도 실패했다면 결과는 false
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }
        }

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
        if(attackHighIndices.Contains(index))  // 있으면
        {
            attackHighIndices.Remove(index);   // 제거한다.

            if(highMarks.ContainsKey(index))
            {
                Destroy(highMarks[index]);
                highMarks[index] = null;
                highMarks.Remove(index);
            }            
        }
    }

    /// <summary>
    /// 모든 후보지역 표시를 삭제하는 함수
    /// </summary>
    private void RemoveAllHigh()
    {
        foreach(var index in attackHighIndices)     // 인덱스 전부 확인
        {
            Destroy(highMarks[index]);              // 오브젝트 삭제하고
            highMarks[index] = null;
        }
        highMarks.Clear();                          // 딕셔너리 정리

        attackHighIndices.Clear();                  // 인덱스 리스트도 정리
        lastAttackSuccessPosition = NOT_SUCCESS;    // 마지막 공격 성공도 초기화
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
                ship.gameObject.SetActive(isShowShips);

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
        opponent.opponentShipDestroyed = true;              // 상대방에게 (상대방의 상대방(나)) 배가 파괴되었다고 표시
        opponent.lastAttackSuccessPosition = NOT_SUCCESS;   // 상대방의 마지막 공격 성공 위치를 초기화(배가 파괴되면 필요없음)

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
    protected virtual void OnDefeat()
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
        remainShipCount = ShipManager.Inst.ShipTypeCount;

        opponentShipDestroyed = false;
        isActionDone = false;

        Board.ResetBoard(ships);
        RemoveAllHigh();
    }

    /// <summary>
    /// 게임 상태가 변경되었을 때 실행되는 델리게이트에 연결될 함수
    /// </summary>
    /// <param name="state">변화된 상태</param>
    public virtual void OnStateChange(GameState state)
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

    public bool Test_InSuccessLine(Vector2Int start, Vector2Int end, bool isHorizontal)
    {
        return InSuccessLine(start, end, isHorizontal);
    }
}
