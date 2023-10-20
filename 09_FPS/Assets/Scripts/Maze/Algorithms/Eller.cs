using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EllerCell : Cell
{
    public int setGroup;
    const int NotSet = -1;
    public EllerCell(int x, int y) : base(x, y)
    {
        setGroup = NotSet;
    }
}

public class Eller : MazeGenerator
{
    int serial = 0;

    protected override void OnSpecificAlgorithmExcute()
    {
        serial = 0;
        int h = height - 1;
        EllerCell[] prevLine = null;

        for(int y = 0; y < h; y++)
        {
            // 1. 한 줄 만들기
            EllerCell[] line = MakeLine(prevLine, y);

            // 2. 옆칸끼리 합치는 작업을 진행한다.
            MergeAdjacent(line, 0.7f);

            // 3. 아래쪽 벽 제거하기
            RemoveSouthWall(line);

            // 4. 생성한 줄 저장하기
            WriteLine(line, y);
            prevLine = line;
        } // 5. 마지막 줄이 될 때까지 1번으로 돌아가 반복


        // 6. 마지막 줄 처리
        //  6.1. 줄 생성은 똑같이 처리
        //  6.2. 합치는 작업은 세트가 다르면 무조건 합침.
        EllerCell[] lastLine = MakeLine(prevLine, h);
        MergeAdjacent(lastLine, 1.1f);
        WriteLine(lastLine, h);
    }

    /// <summary>
    /// 한 줄을 만드는 함수
    /// </summary>
    /// <param name="prev">이전 줄</param>
    /// <param name="row">이 줄이 몇번째 줄인지</param>
    /// <returns>만들어진 한 줄</returns>
    private EllerCell[] MakeLine(EllerCell[] prev, int row)
    {
        // 1. 한 줄 만들기
        //  1.1. 첫번째 줄은 가로 길이만큼 셀을 만들고 각각 고유한 집합에 포함시킨다.
        //  1.2. 나머지 줄은 위쪽줄에 벽이 없으면 위쪽 셀의 집합과 같은 집합을 사용한다.
        //  1.3. 위쪽줄에 벽이 있으면 고유한 집합에 포함시킨다.

        EllerCell[] line = new EllerCell[width];        // 한 줄용 배열 만들기
        for(int x = 0;x<width;x++)
        {
            line[x] = new EllerCell(x, row);            // 줄에 들어갈 칸 만들기

            if(prev != null && prev[x].IsPath(Direction.South))
            {
                // 위쪽 줄이 있고 남쪽 벽이 없다(1.2)
                line[x].setGroup = prev[x].setGroup;    // 위쪽 줄의 세트값을 사용
                line[x].MakePath(Direction.North);      // 위에서 아래로 길이 있으니, 아래에서 위쪽으로 길만들기
            }
            else
            {
                // 첫번째 줄(1.1)이거나 위쪽 칸에 남쪽벽이 있다(1.3)
                line[x].setGroup = serial;              // 고유한 값 설정
                serial++;                               // 다음 고유한 값 만들기
            }
        }

        return line;
    }

    /// <summary>
    /// 인접한 셀을 랜덤하게 합치는 함수
    /// </summary>
    /// <param name="line">작업을 처리할 줄</param>
    /// <param name="chance">합쳐질 확률</param>
    private void MergeAdjacent(EllerCell[] line, float chance)
    {
        // 2. 옆칸끼리 합치는 작업을 진행한다.
        //  2.1. 서로 집합이 다르면 랜덤하게 벽을 제거하고 같은 집합으로 만든다.
        //       (오른쪽에 있는 셀과 같은 집합을 가지는 셀은 모두 집합이 변경된다. 현재 줄에서만 변경.)
        //  2.2. 서로 같은 집합일 경우는 패스
        
        int count = 1;          // 한줄이 통체로 합쳐지는 것을 방지하기 위한 변수
        int w = width - 1;      // for안에서 같은 계산을 중복으로 하는 것을 방지하기 위해 미리 계산해 놓은 값
        for(int x=0;x<w;x++)    
        {
            if(count < w && line[x].setGroup != line[x+1].setGroup && Random.value < chance)
            {
                // 집합이 다르고 랜덤 확율을 통과한 경우(2.1) + 한줄이 통체로 뚫리는 것을 방지하기 위해 count추가
                line[x].MakePath(Direction.East);       // 합쳐지는 상황이면 서로 길만들기
                line[x+1].MakePath(Direction.West);

                int target = line[x + 1].setGroup;      // 오른쪽에 있는 셀의 집합번호를 저장
                line[x+1].setGroup = line[x].setGroup;  // 오른쪽에 있는 셀과 같은 집합번호를 가진 모든 셀의 집합번호를 왼쪽 셀과 통일
                for(int i=x+2;i<width;i++)
                {
                    if (line[i].setGroup == target)
                    {
                        line[i].setGroup = line[x].setGroup;
                    }
                }

                count++;
            }
        }
    }

    /// <summary>
    /// 랜덤으로 줄의 남쪽 벽을 제거하는 함수
    /// </summary>
    /// <param name="line">적업을 처리할 줄</param>
    private void RemoveSouthWall(EllerCell[] line)
    {
        // 3. 아래쪽 벽 제거하기
        //  3.1. 한 집합당 최소 하나의 벽이 뚫려야 한다.

        // 세트별로 리스트 만들기
        Dictionary<int, List<int>> setListDic = new Dictionary<int, List<int>>();    // 키:세트, 값:셀 x좌표의 리스트
        for(int x=0;x<width;x++)
        {
            if (!setListDic.ContainsKey(line[x].setGroup))
            {
                setListDic[line[x].setGroup] = new List<int>();
            }

            setListDic[line[x].setGroup].Add(x);
        }

        // 세트별로 배열에 저장하고, 세트별로 남쪽에 길 만들기
        Dictionary<int, int[]> setArrayDic = new Dictionary<int, int[]>();
        foreach(int key in setListDic.Keys)
        {
            int[] tempArray = setListDic[key].ToArray();        // 리스트를 배열로 변경
            Util.Shuffle(tempArray);                            // 순서 섞기
            setArrayDic[key] = tempArray;                       // 딕셔너리에 배열 저장

            int[] array = setArrayDic[key];
            int index = array[0];                               // 최소 1개는 보장(섞은 다음 첫번째이므로 랜덤한 것을 고른 것과 같다)
            line[index].MakePath(Direction.South);              // 남쪽길 만들기

            int length = array.Length;
            for (int i = 1; i < length; i++)
            {
                if(Random.value < 0.5f)                         // 남은 것은 50% 확률로 남쪽길 만들기
                {
                    line[array[i]].MakePath(Direction.South);
                }    
            }
        }
    }

    /// <summary>
    /// 한 줄을 최종 결과로 저장하기
    /// </summary>
    /// <param name="line">기록할 줄</param>
    /// <param name="row">몇번째 줄인지</param>
    private void WriteLine(EllerCell[] line, int row)
    {
        int index = GridToIndex(0, row);    // 시작 인덱스 계산해 놓기
        for (int x=0;x<width;x++)
        {
            cells[index + x] = line[x];     // cells에 기록
        }
    }
}
