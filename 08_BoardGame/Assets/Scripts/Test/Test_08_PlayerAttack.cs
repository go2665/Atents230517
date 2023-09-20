using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_08_PlayerAttack : TestBase
{
    public Button reset;
    public Button random;
    public Button resetAndRandom;
    public PlayerBase user;
    public PlayerBase enemy;

    Board userBoard;
    Board enemyBoard;

    private void Start()
    {
        reset.onClick.AddListener(enemy.UndoAllShipDeployment);
        random.onClick.AddListener(() => enemy.AutoShipDeployment(true));
        resetAndRandom.onClick.AddListener(() =>
        {
            enemy.UndoAllShipDeployment();
            enemy.AutoShipDeployment(true);
        });

        userBoard = user.Board;
        enemyBoard = enemy.Board;

        user.Test_SetOpponent(enemy);
        enemy.Test_SetOpponent(user);

        enemy.AutoShipDeployment(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.R_Click.performed += OnR_Click;
    }

    protected override void OnDisable()
    {
        inputActions.Test.R_Click.performed -= OnR_Click;
        base.OnDisable();
    }

    protected override void TestClick(InputAction.CallbackContext context)
    {
        // 클릭한 지점 공격(유저->적)
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        user.Attack(world);
    }

    protected virtual void OnR_Click(InputAction.CallbackContext context)
    {
        user.AutoAttack();
    }

}
