using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 생성할 셀의 프리팹
    /// </summary>
    public GameObject cellPrefab;

    /// <summary>
    /// 보드의 가로 길이(셀 개수)
    /// </summary>
    private int width = 16;

    /// <summary>
    /// 보드의 세로 길이(셀 개수)
    /// </summary>
    private int height = 16;

    /// <summary>
    /// 보드에 설치될 지뢰 개수
    /// </summary>
    private int mineCount = 10;

    /// <summary>
    /// 셀 한변의 길이
    /// </summary>
    const float Distance = 1.0f;

    /// <summary>
    /// 이 보드가 가지는 모든 셀
    /// </summary>
    Cell[] cells;

    /// <summary>
    /// 현재 닫혀있는 셀의 개수
    /// </summary>
    int closeCellCount = 0;

    /// <summary>
    /// 보드가 클리어되었는지 확인하는 프로퍼티
    /// </summary>
    public bool IsBoardClear => (closeCellCount == mineCount) && !isBoardOver;

    /// <summary>
    /// 보드가 실패했는지 표시하는 변수
    /// </summary>
    bool isBoardOver = false;

    /// <summary>
    /// 현재 마우스 커서가 있는 셀
    /// </summary>
    Cell currentCell = null;
    Cell CurrentCell
    {
        get => currentCell;
        set
        {
            if(currentCell != value)
            {
                currentCell?.RestoreCovers();   // 이전 셀은 원래 상태로 되돌리기
                currentCell = value;
                currentCell?.CellLeftPress();   // 새 셀은 눌려진 상태로 만들기
            }
        }
    }

    /// <summary>
    /// 열려있는 셀에 표시될 이미지
    /// </summary>
    public Sprite[] openCellImage;
    public Sprite this[OpenCellType type] => openCellImage[(int)type];

    /// <summary>
    /// 닫혀있는 셀에 표시될 이미지
    /// </summary>
    public Sprite[] closeCellImage;
    public Sprite this[CloseCellType type] => closeCellImage[(int)type];

    /// <summary>
    /// 찾은 지뢰 개수
    /// </summary>
    private int foundMineCount = 0;

    /// <summary>
    /// 찾은 지뢰 개수 확인용 프로퍼티
    /// </summary>
    public int FoundMineCount => foundMineCount;

    /// <summary>
    /// 못찾은 지뢰 개수 확인용 프로퍼티
    /// </summary>
    public int NotFoundMineCount => mineCount - foundMineCount;

    /// <summary>
    /// 보드에 있는 셀이 눌려졌을 때 실행(왼클릭만)
    /// </summary>
    public Action onBoardLeftPress;

    /// <summary>
    /// 눌렀던 셀에서 버튼을 땠을 때 실행(왼클릭만)
    /// </summary>
    public Action onBoardLeftRelease;

    /// <summary>
    /// 인풋액션
    /// </summary>
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.LeftClick.performed += OnLeftPress;
        inputActions.Player.LeftClick.canceled += OnLeftRelease;
        inputActions.Player.RightClick.performed += OnRightPress;
        inputActions.Player.MouseMove.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        inputActions.Player.MouseMove.performed -= OnMouseMove;
        inputActions.Player.RightClick.performed -= OnRightPress;
        inputActions.Player.LeftClick.canceled -= OnLeftRelease;
        inputActions.Player.LeftClick.performed -= OnLeftPress;
        inputActions.Player.Disable();
    }

    /// <summary>
    /// 이 보드가 가질 모든 셀을 생성하고 배치하는 함수.(초기화)
    /// </summary>
    /// <param name="newWidth">보드의 가로 길이</param>
    /// <param name="newHieght">보드의 세로 길이</param>
    /// <param name="newMineCount">보드에 배치될 지뢰수</param>
    public void Initialize(int newWidth, int newHieght, int newMineCount)
    {
        width = newWidth;
        height = newHieght; 
        mineCount = newMineCount;

        if (cells != null)  // 혹시 만들어 진 셀이 있으면
        {
            foreach (var cell in cells)     // 모조리 삭제
            {
                Destroy(cell.gameObject);
            }
            cells = null;
        }

        cells = new Cell[width * height];   // 셀이 들어갈 배열 만들기

        GameManager gameManager = GameManager.Inst; // 사용할 일이 많을 것 같아 미리 가져오기
        // 셀을 하나씩 생성하기
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);    // 게임 오브젝트 생성
                Cell cell = cellObj.GetComponent<Cell>();
                cell.Board = this;
                cell.ID = x + y * width;                                                    // 셀의 아이디 설정
                cell.transform.localPosition = new Vector3(x * Distance, -y * Distance);    // 셀의 위치 설정

                cell.onMineSet += MineSet;                              // 지뢰 설치할 때
                cell.onFlagUse += gameManager.DecreaseFlagCount;        // 깃발을 설치했을 때
                cell.onFlagReturn += gameManager.IncreaseFlagCount;     // 설치된 깃발을 해제했을 때
                cell.onAction += gameManager.FinishPlayerAction;        // 플레이어의 행동 한번이 끝났을 때
                cell.onExplosion += gameManager.GameOver;               // 지뢰가 터졌을 때

                cell.onExplosion += () => isBoardOver = true;           // 지뢰가 터졌을 때
                cell.onCellOpen += () => closeCellCount--;              // 셀이 열렸을 때

                cells[cell.ID] = cell;                          // 배열에 셀 저장
                cellObj.name = $"Cell_{cell.ID}_({x},{y})";     // 셀 게임 오브젝트의 이름 변경
            }
        }

        gameManager.onGameReady += ResetBoard;
        gameManager.onGameOver += OnGameOver;
        gameManager.onGameOver += FindMineCount;
        gameManager.onGameClear += FindMineCount;

        ResetBoard();
    }

    /// <summary>
    /// 보드에 존재하는 모든 셀의 데이터를 리셋하고 지뢰를 새로 배치하는 함수(게임 재시작용 함수)
    /// </summary>
    private void ResetBoard()
    {
        // 셀의 데이터 초기화
        foreach(var cell in cells)
        {
            cell.ResetData();
        }

        // 닫힌 셀의 개수 초기화
        closeCellCount = cells.Length;

        // 보드 상태 초기화
        isBoardOver = false;

        // 보드에 mineCount만큼 지뢰 배치하기
        int[] ids = new int[cells.Length];
        for(int i=0;i < cells.Length;i++)
        {
            ids[i] = i;
        }
        Shuffle(ids);
        for(int i=0;i<mineCount;i++)
        {
            cells[ids[i]].SetMine();
        }

    }

    /// <summary>
    /// 게임 오버가 되었을 때 실행할 함수
    /// </summary>
    private void OnGameOver()
    {
        // 잘못찾은 것은 잘못 찾았다고 표시
        // 못찾은 지뢰는 커버 열기
    }

    /// <summary>
    /// 배열을 섞는 함수
    /// </summary>
    /// <param name="source">섞을 배열</param>
    private void Shuffle(int[] source)
    {
        // source의 순서 섞기(피셔-예이츠 알고리즘 사용)
        int loopCount = source.Length - 1;
        for (int i=0;i<loopCount;i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, source.Length - i);   // 전체 개수에서 계속 1이 감소하는 범위
            int lastIndex = loopCount - i;  // 마지막에서 계속 1씩 감소하는 숫자

            (source[lastIndex], source[randomIndex]) = (source[randomIndex], source[lastIndex]);    // 스왑하기
        }
    }

    /// <summary>
    /// 특정 셀에 지뢰가 설치되었을 때 처리할 함수
    /// </summary>
    /// <param name="id">셀의 아이디</param>
    private void MineSet(int id)
    {
        Vector2Int grid = IndexToGrid(id);  // 셀의 위치를 찾는다.

        // 위치의 주변 셀을 찾는다.
        for (int y=-1;y<2;y++)
        {
            for(int x = -1;x<2;x++)
            {
                int index = GridToIndex(x + grid.x, y + grid.y);
                if(index != Cell.ID_NOT_VALID && !(x==0 && y==0))   // 인덱스가 valid하고 (0,0)은 아닌 경우에만 처리
                {
                    Cell cell = cells[index];                       
                    cell.IncreaseAroundMineCount(); // 주변 셀의 aroundMineCount를 1씩 증가 시킨다.
                }
            }
        }
    }

    /// <summary>
    /// 인덱스를 그리즈 좌표로 변경하는 함수
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private Vector2Int IndexToGrid(int index)
    {
        return new(index % width, index / width);
    }

    /// <summary>
    /// 그리드 좌표를 인덱스로 변경해주는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int GridToIndex(int x, int y)
    {
        int result = Cell.ID_NOT_VALID;
        if( IsValidGrid(x, y) )
            result = x + y * width;

        return result;
    }

    /// <summary>
    /// 그리드 좌표가 보드 상에 있는 좌표인지 확인하는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>true면 적합한 좌표, false면 없는 좌표</returns>
    private bool IsValidGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    /// <summary>
    /// 그리드 좌표가 보드 상에 있는 좌표인지 확인하는 함수
    /// </summary>
    /// <param name="grid"></param>
    /// <returns>true면 적합한 좌표, false면 없는 좌표</returns>
    private bool IsValidGrid(Vector2Int grid)
    {
        return IsValidGrid(grid.x, grid.y);
    }

    /// <summary>
    /// 스크린좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="screenPos">입력으로 들어오는 스크린 좌표</param>
    /// <returns>변환된 그리드 좌표</returns>
    private Vector2Int ScreenToGrid(Vector2 screenPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);   // 스크린좌표를 월드 좌표로 변환
        Vector2 diff = worldPos - (Vector2)transform.position;          // 보드에 피봇에서 얼마나 떨어져 있는지를 확인

        return new(Mathf.FloorToInt(diff.x/Distance), Mathf.FloorToInt(-diff.y/Distance));  // 차이를 한칸당 간격으로 나누어서 몇번째 그리드인지 리턴
    }

    /// <summary>
    /// 마우스 왼쪽 버튼을 눌렀을 때 실행되는 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnLeftPress(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue(); // 마우스 커서의 스크린 좌료를 받아와서
        Vector2Int grid = ScreenToGrid(screenPos);              // 그리드 좌표로 변경 시기고

        int index = GridToIndex(grid.x, grid.y);                // 그리드 좌료를 인덱스로 변환

        if( index != Cell.ID_NOT_VALID)
        {
            //Debug.Log(index);
            GameManager.Inst.GameStart();   // 셀을 누르면 게임 시작
            Cell target = cells[index];
            target.CellLeftPress();

            onBoardLeftPress?.Invoke();
        }
    }

    /// <summary>
    /// 마우스 왼쪽 버튼을 땠을 때 실행되는 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnLeftRelease(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue(); // 마우스 커서의 스크린 좌료를 받아와서
        Vector2Int grid = ScreenToGrid(screenPos);              // 그리드 좌표로 변경 시기고

        int index = GridToIndex(grid.x, grid.y);                // 그리드 좌료를 인덱스로 변환

        if (index != Cell.ID_NOT_VALID)
        {
            //Debug.Log(index);
            Cell target = cells[index];
            target.CellLeftRelease();

            onBoardLeftRelease?.Invoke();
        }
    }

    /// <summary>
    /// 마우스 오른쪽 버튼을 눌렀을 때 실행되는 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnRightPress(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue(); // 마우스 커서의 스크린 좌료를 받아와서
        Vector2Int grid = ScreenToGrid(screenPos);              // 그리드 좌표로 변경 시기고

        int index = GridToIndex(grid.x, grid.y);                // 그리드 좌료를 인덱스로 변환

        if (index != Cell.ID_NOT_VALID)
        {
            GameManager.Inst.GameStart();   // 깃발이 설치되면 게임 시작
            Cell target = cells[index];
            target.CellRightPress();
        }
    }

    /// <summary>
    /// 마우스가 움직일 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if (Mouse.current.leftButton.isPressed) // 눌려진 상태일 때
        {
            Vector2 screenPos = Mouse.current.position.ReadValue(); // 마우스 커서의 스크린 좌료를 받아와서
            Vector2Int grid = ScreenToGrid(screenPos);              // 그리드 좌표로 변경 시기고

            int index = GridToIndex(grid.x, grid.y);                // 그리드 좌료를 인덱스로 변환

            if (index != Cell.ID_NOT_VALID)
            {
                CurrentCell = cells[index]; // 커서가 있는 셀을 Current로 만들기
            }
            else
            {
                CurrentCell = null;
            }
        }
    }

    /// <summary>
    /// 특정 셀 주변에 있는 모든 셀을 리턴하는 함수
    /// </summary>
    /// <param name="id">이웃을 구할 셀의 ID</param>
    /// <returns>ID 셀의 모든 이웃</returns>
    public List<Cell> GetNeighbors(int id)
    {
        List<Cell> result = new List<Cell>(8);
        Vector2Int grid = IndexToGrid(id);
        for(int y=-1;y<2;y++)
        {
            for(int x=-1;x<2;x++)
            {
                int index = GridToIndex(x + grid.x, y + grid.y);
                if( index != Cell.ID_NOT_VALID && !(x==0 && y==0))
                {
                    result.Add(cells[index]);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 현재 찾은 지뢰 개수를 갱신하는 함수
    /// </summary>
    void FindMineCount()
    {
        foundMineCount = 0;
        foreach(Cell cell in cells)
        {
            if(cell.HasMine && cell.IsFlaged)
            {
                foundMineCount++;
            }
        }
    }

    // 테스트 함수 ---------------------------------------------------------------------------------
    public void Test_ResetBoard()
    {
        ResetBoard();
    }

    public void Test_Shuffle()
    {
        int[,] result = new int[10, 10];

        for(int i=0;i<1000000;i++)
        {
            int[] source = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Shuffle(source);

            for(int j=0;j<source.Length;j++) 
            {
                result[source[j], j]++;     // (j행, source[j]열)에 1 증가
            }
        }

        string output = "";
        for(int y = 0;y<10;y++)
        {
            output += $"숫자{y} : ";
            for(int x = 0;x<10;x++)
            {
                output += $"{result[y,x]} ";
            }
            output += "\n" ;
        }
        Debug.Log(output);
    }
}