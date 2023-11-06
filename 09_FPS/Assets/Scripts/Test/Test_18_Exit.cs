using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Test_18_Exit : TestBase
{
    public MazeVisualizer maze;

    protected override void Start()
    {
        if(seed != -1)
        {
            Random.InitState(seed);
        }
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        // 1번 키를 누르면 플레이어는 미로상의 랜덤한 위치에 배치됨
        // 배치되면 플레이어는 4방향 중 랜덤한 방향을 바라본다.
        // 배치된 셀의 벽이 없는 쪽을 바라봐야 한다.
        if (seed != -1)
        {
            Random.InitState(seed);
        }

        int width = (int)(maze.width * 0.2f);
        int height = (int)(maze.height * 0.2f);

        int widthMin = (int)((maze.width - width) * 0.5f);
        int widthMax = (int)((maze.width + width) * 0.5f);
        int heightMin = (int)((maze.height - height) * 0.5f);
        int heightMax = (int)((maze.height + height) * 0.5f);

        int x = Random.Range(widthMin, widthMax);
        int y = Random.Range(heightMin, heightMax);

        Debug.Log($"{x}, {y}");        

        Vector3 world = maze.GridToWorld(x, y);

        Player player = GameManager.Inst.Player;
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.transform.position = world;
        controller.enabled = true;

        Ray ray = new(world + Vector3.up, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 10.0f))
        {
            CellVisualizer cell = hitInfo.collider.gameObject.GetComponentInParent<CellVisualizer>();
            Direction paths = cell.GetPaths();
            Debug.Log(paths);
            List<Vector3> dirList = new List<Vector3>(4);
            if( (paths & Direction.North) != 0)
            {
                dirList.Add(Vector3.forward);
            }
            if ((paths & Direction.East) != 0)
            {
                dirList.Add(Vector3.right);
            }
            if ((paths & Direction.South) != 0)
            {
                dirList.Add(Vector3.back);
            }
            if ((paths & Direction.West) != 0)
            {
                dirList.Add(Vector3.left);
            }

            Vector3 dir = dirList[Random.Range(0, dirList.Count)];
            player.transform.LookAt(player.transform.position + dir);
        }
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        int width = (int)(maze.width * 0.2f);
        int height = (int)(maze.height * 0.2f);
        int widthMin = (int)((maze.width - width) * 0.5f);
        int widthMax = (int)((maze.width + width) * 0.5f);
        int heightMin = (int)((maze.height - height) * 0.5f);
        int heightMax = (int)((maze.height + height) * 0.5f);

        Vector3 p0 = new(widthMin * CellVisualizer.CellSize, 0, -heightMin * CellVisualizer.CellSize);
        Vector3 p1 = new(widthMax * CellVisualizer.CellSize, 0, -heightMin * CellVisualizer.CellSize);
        Vector3 p2 = new(widthMax * CellVisualizer.CellSize, 0, -heightMax * CellVisualizer.CellSize);
        Vector3 p3 = new(widthMin * CellVisualizer.CellSize, 0, -heightMax * CellVisualizer.CellSize);

        Handles.color = Color.red;
        Handles.DrawLine(p0,p1,5.0f);
        Handles.DrawLine(p1,p2,5.0f);
        Handles.DrawLine(p2,p3,5.0f);
        Handles.DrawLine(p3,p0,5.0f);
#endif
    }
}
