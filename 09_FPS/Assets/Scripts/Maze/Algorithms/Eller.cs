using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public class EllerCell : Cell
{
    public EllerCell(int x, int y) : base(x, y)
    {

    }
}

public class Eller : MazeGenerator
{
    protected override void OnSpecificAlgorithmExcute()
    {
        // 1. 한 줄 만들기
        //  1.1. 첫번째 줄은 가로 길이만큼 셀을 만들고 각각 유니크한 집합에 포함시킨다.
        //  1.2. 나머지 줄은 위쪽줄에 벽이 없으면 위쪽 셀의 집합과 같은 집합을 사용한다.
        //  1.3. 위쪽줄에 벽이 있으면 고유한 집합에 포함시킨다.

        // 2. 옆칸끼리 합치는 작업을 진행한다.
        //  2.1. 서로 집합이 다르면 랜덤하게 벽을 제거하고 같은 집합으로 만든다.
        //       (오른쪽에 있는 셀과 같은 집합을 가지는 셀은 모두 집합이 변경된다. 현재 줄에서만 변경.)
        //  2.2. 서로 같은 집합일 경우는 패스

        // 3. 아래쪽 벽 제거하기
        //  3.1. 한 집합당 최소 하나의 벽이 뚫려야 한다.

        // 4. 마지막 줄이 될 때까지 1번으로 돌아가 반복

        // 5. 마지막 줄 처리
        //  5.1. 줄 생성은 똑같이 처리
        //  5.2. 합치는 작업은 세트가 다르면 무조건 합침.
    }
}
