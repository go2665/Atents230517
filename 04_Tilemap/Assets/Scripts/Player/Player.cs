using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 현재 입력받은 이동 방향
    /// </summary>
    Vector2 inputDir = Vector2.zero;

    /// <summary>
    /// 공격을 시작했을 때 백업 해놓은 이동 방향
    /// </summary>
    Vector2 oldInputDir = Vector2.zero;

    /// <summary>
    /// 현재 이동중인지 표시하는 변수(공격이 끝났을 때 여전히 이동키를 누르고 있었을 때만 복구를 위해)
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// 플레이어가 현재 위치하고 있는 맵의 그리드 좌표
    /// </summary>
    Vector2Int currentMapPos;
    Vector2Int CurrentMapPos
    {
        get => currentMapPos;
        set
        {
            if (value != currentMapPos)
            {
                currentMapPos = value;
                onMapMoved?.Invoke(currentMapPos);  // currentMapPos가 변경되는 순간 델리게이트 실행
            }
        }
    }

    /// <summary>
    /// 플레이어가 있는 서브맵이 변경되었을 때 실행디는 델리게이트(파라메터:진입한 맵의 그리드 좌표)
    /// </summary>
    public Action<Vector2Int> onMapMoved;

    /// <summary>
    /// 플레이어의 최대 수명
    /// </summary>
    public float maxLifeTime = 10.0f;

    /// <summary>
    /// 플레이어의 현재 수명
    /// </summary>
    float lifeTime;

    /// <summary>
    /// 플레이어의 수명 확인 및 설정용 프로퍼티
    /// </summary>
    public float LifeTime
    {
        get => lifeTime;
        set
        {
            lifeTime = value;
            if(lifeTime < 0.0f && !isDead)  
            {
                Die();  // 수명이 0 미만이고 살아있는 상태일때만 죽여라
            }
            else
            {
                // 살아있는 상태면 수명을 최소 0, 최대 maxLifeTime으로 클램프
                lifeTime = Mathf.Clamp(lifeTime, 0.0f, maxLifeTime);
            }
            onLifeTimeChange?.Invoke(lifeTime/maxLifeTime); // 수명이 변했음을 알림
        }
    }

    /// <summary>
    /// 수명이 변경되었을 때 실행되는 델리게이트
    /// </summary>
    public Action<float> onLifeTimeChange;

    /// <summary>
    /// 플레이어가 살았는지 죽었는지 표시하는 변수
    /// </summary>
    bool isDead = false;

    /// <summary>
    /// 플레이어가 죽었을 때 실행될 델리게이트, (파라메터:전체플레이시간, 킬 카운트)
    /// </summary>
    public Action<float, int> onDie;

    /// <summary>
    /// 현재 공격 중인지 아닌지 표시용
    /// </summary>
    bool isAttacking = false;

    /// <summary>
    /// 현재 남아있는 쿨타임. 0 이하일 때만 공격 가능
    /// </summary>
    float currentAttackCoolTime = 0.0f;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attackCoolTime = 1.0f;

    /// <summary>
    /// 쿨타임이 다 됬는지 확인하는 프로퍼티(코드 가독성을 위한 것)
    /// </summary>
    bool IsAttackReady => currentAttackCoolTime < 0;

    /// <summary>
    /// 인풋 액션
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 공격이 현재 유효한지 표시하는 변수(true면 공격이 유효, false면 유효하지 않음)
    /// </summary>
    bool isAttackValid = false;

    /// <summary>
    /// 플레이어의 공격 범위안에 들어와 있는 모든 슬라임
    /// </summary>
    List<Slime> attackTargetList;

    /// <summary>
    /// 공격 영역의 회전 중심 축
    /// </summary>
    Transform attackSensorAxis;

    /// <summary>
    /// 전체 플레이 시간
    /// </summary>
    float totalPlayTime = 0.0f;

    /// <summary>
    /// 잡은 슬라임 수
    /// </summary>
    int killCount = int.MinValue;
    int KillCount
    {
        get => killCount;
        set
        {
            if(killCount != value)
            {
                killCount = value;
                onKillCountChange?.Invoke(killCount);   // 값이 변경되었을 때만 델리게이트 실행
            }
        }
    }
    /// <summary>
    /// 잡은 슬라임 수가 변경될 때 실행될 델리게이트
    /// </summary>
    public Action<int> onKillCountChange;

    /// <summary>
    /// 월드 매니저
    /// </summary>
    WorldManager world;

    // 컴포넌트들    
    Animator animator;
    Rigidbody2D rigid;    

    // 애니메이터 파라메터 해쉬들
    readonly int InputX_Hash = Animator.StringToHash("InputX");
    readonly int InputY_Hash = Animator.StringToHash("InputY");
    readonly int IsMove_Hash = Animator.StringToHash("IsMove");
    readonly int Attack_Hash = Animator.StringToHash("Attack");

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        attackSensorAxis = transform.GetChild(0);

        attackTargetList = new List<Slime>(4);
        AttackSensor sensor = attackSensorAxis.GetComponentInChildren<AttackSensor>();
        sensor.onEnemyEnter += (slime) =>
        {
            if(isAttackValid)
            {
                slime.Die();
            }
            else
            {
                attackTargetList.Add(slime);
                slime.ShowOutline();
            }
        };
        sensor.onEnemyExit += (slime) =>
        {
            attackTargetList.Remove(slime);
            slime.ShowOutline(false);
        };
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        world = GameManager.Inst.World;
        LifeTime = maxLifeTime;
        KillCount = 0;
    }

    private void Update()
    {
        currentAttackCoolTime -= Time.deltaTime;
        LifeTime -= Time.deltaTime;
        totalPlayTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // 이동 처리
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * inputDir);
        
        CurrentMapPos = world.WorldToGrid(rigid.position);
    }

    /// <summary>
    /// Move 액션을 발동시켰을 때 실행되는 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();   // 입력 받고

        if(isAttacking)
        {
            oldInputDir = input;                        // 공격 중일 때는 백업에 입력 저장하기
        }
        else
        {
            // 공격 중이 아닐 때(일반적인 상황)
            inputDir = input;                               // 방향 기록하고
            animator.SetFloat(InputX_Hash, inputDir.x);     // 애니메이터 파라메터 변경(블랜드 트리 조정)
            animator.SetFloat(InputY_Hash, inputDir.y);

            AttackSensorRotate(inputDir);
        }

        isMove = true;                                  // 이동 중이라고 표시
        animator.SetBool(IsMove_Hash, true);            // 이동 애니메이션 시작
    }

    /// <summary>
    /// Move 액션 발동이 끝났을 때 실행되는 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnStop(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        inputDir = Vector2.zero;                // 방향 초기화

        isMove = false;                         // 이동 끝났다고 표시
        animator.SetBool(IsMove_Hash, false);   // Idle 애니메이션으로 돌리기
    }

    /// <summary>
    /// Attack 액션을 발동 시켰을 때 실행되는 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if( IsAttackReady )   // 쿨타임이 다됬는지 확인
        {
            isAttacking = true;
            animator.SetTrigger(Attack_Hash);   // 공격 애니메이션 시작
            oldInputDir = inputDir;             // 이동 방향을 백업
            inputDir = Vector2.zero;            // 이동 방향 zero로 설정

            currentAttackCoolTime = attackCoolTime; // 쿨타임 초기화
        }
    }

    /// <summary>
    /// 이동 방향을 복구 시키는 함수(PlayerAttackBlendTree에서 상태가 끝날 때 실행)
    /// </summary>
    public void RestorInputDir()
    {
        if( isMove )                    // 여전히 이동 키를 누르고 있을 때만
        {
            inputDir = oldInputDir;                     // 이동 방향 복구
            animator.SetFloat(InputX_Hash, inputDir.x); // 캐릭터 방향 조정(보이는 방향)
            animator.SetFloat(InputY_Hash, inputDir.y);

            AttackSensorRotate(inputDir);
        }

        isAttacking = false;
    }

    /// <summary>
    /// 공격 애니메이션 중에 공격이 유효하기 시작하면 실행
    /// </summary>
    public void AttackValid()
    {
        isAttackValid = true;

        foreach(var slime in attackTargetList)
        {
            slime.Die();
        }
        attackTargetList.Clear();
    }

    /// <summary>
    /// 공격 애니메이션 중에 공격이 유효하지 않게 되면 실행
    /// </summary>
    public void AttackNotValid()
    {
        isAttackValid = false;
    }

    void AttackSensorRotate(Vector2 dir)
    {
        // 4방향 구분하기
        // 대각선일 경우 위 아래를 우선하기

        if( dir.y < 0 )
        {
            attackSensorAxis.rotation = Quaternion.identity;
        }
        else if( dir.y > 0)
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, 180.0f);
        }
        else if( dir.x > 0 )
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, 90.0f);
        }
        else if( dir.x < 0 )
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, -90.0f);
        }
        else
        {
            attackSensorAxis.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// 플레이어가 죽을 때 실행되는 함수
    /// </summary>
    private void Die()
    {
        isDead = true;                  // 죽었다고 표시
        lifeTime = 0.0f;                // 수명을 깔끔하게 0으로 세팅
        inputActions.Player.Disable();  // 입력막기
        onDie?.Invoke(totalPlayTime, killCount);    // 죽었다고 알리기

        Debug.Log("플레이어 사망");
    }

    /// <summary>
    /// 몬스터를 잡았을 때 실행될 함수
    /// </summary>
    /// <param name="bonus">몬스터를 죽임으로써 얻는 추가시간</param>
    public void MonsterKill(float bonus)
    {
        if ( !isDead ) 
        {
            LifeTime += bonus;
            KillCount++;
        }
    }
}