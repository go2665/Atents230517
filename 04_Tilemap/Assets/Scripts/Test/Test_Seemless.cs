using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Seemless : TestBase
{
    public int x = 0;
    public int y = 0;

    WorldManager world;

    private void Start()
    {
        world = GameManager.Inst.World;
    }

#if UNITY_EDITOR
    protected override void Test1(InputAction.CallbackContext context)
    {
        world.TestLoadScene(x, y);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        world.TestUnloadScene(x, y);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        // (x, y)에 플레이어가 있다고 가정하고 Refresh 실행
        world.TestRefreshScenes(x, y);

        // 실습
        // WorldManager.RefreshScenes 함수 구현하기
    }
#endif
}
