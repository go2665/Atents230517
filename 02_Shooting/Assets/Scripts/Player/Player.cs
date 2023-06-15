using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // public으로 만든 변수는 인스팩터 창에서 수정이 가능하다.

    /// <summary>
    /// 플레이어의 이동 속도
    /// </summary>
    public float speed = 2.0f;

    /// <summary>
    /// 총알 프리팹
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float fireInterval = 0.5f;

    /// <summary>
    /// 총알 연사용 코루틴
    /// </summary>
    IEnumerator fireCoroutine;

    /// <summary>
    /// 총알 발사 위치들
    /// </summary>
    Transform[] fireTransforms;

    /// <summary>
    /// 코루틴에서 시간 대기용 캐싱(총알 발사 간격)
    /// </summary>
    WaitForSeconds fireWait;

    /// <summary>
    /// 총알 발사 이팩트용 그림
    /// </summary>
    GameObject fireFlash;

    /// <summary>
    /// 코루틴에서 시간 대기용 캐싱(플래시 이팩트)
    /// </summary>
    WaitForSeconds flashWait;

    /// <summary>
    /// 부스트 상태를 나타내는 변수(일반 상황일 때는 1, 부스트 상황일 때는 2)
    /// </summary>
    float boost = 1.0f;

    /// <summary>
    /// 플레이어의 입력된 이동 방향
    /// </summary>
    Vector3 direction = Vector3.zero;   // 초기값으로 방향은 지정이 안되어있다.

    /// <summary>
    /// 애니메이터 컨트롤러 컴포넌트
    /// </summary>
    Animator anim;

    readonly int InputY_String = Animator.StringToHash("InputY");

    /// <summary>
    /// 입력 액션 에셋
    /// </summary>
    PlayerInputAction inputAction;

    /// <summary>
    /// 리지드바디2D용 컴포넌트
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// 플레이어의 현재 점수
    /// </summary>
    int score = 0;

    /// <summary>
    /// 점수 확인하고 설정할 수 있는 프로퍼티
    /// </summary>
    public int Score
    {
        get => score;
        private set
        {
            if( score != value )    // 점수가 변하면
            {
                score = value;
                onScoreChange?.Invoke( score ); // 변했다고 알람 전송
                //Debug.Log($"Score : {score}");
            }
        }
    }

    /// <summary>
    /// 점수 변화 알림용 델리게이트
    /// </summary>
    public Action<int> onScoreChange;

    /// <summary>
    /// 파워가 최대일때 파워 아이템을 먹으면 얻는 보너스 점수
    /// </summary>
    public int powerBonus = 300;

    /// <summary>
    /// 현재 플레이어의 파워 단계
    /// </summary>
    private int power = 0;

    /// <summary>
    /// 파워 확인 및 설정용 프로퍼티
    /// </summary>
    private int Power
    {
        get => power;
        set
        {
            if( power != value )    // 파워가 변경되면
            { 
                power = value;

                if( power > 3 )     // 파워 최대치가 넘어서면
                {
                    AddScore(powerBonus);   // 보너스 점수 추가
                }

                power = Mathf.Clamp(power, 1, 3);   // 파워는 1~3 사이로 유지

                RefreshFirePositions(power);        // 파워에 따라 총알 발사구 조정

                //Debug.Log($"Power : {power}");
            }
        }
    }

    /// <summary>
    /// 총알간의 발사 각도
    /// </summary>
    public float fireAngle = 30.0f;

    /// <summary>
    /// 플레이어의 수명
    /// </summary>
    private int life = 0;

    /// <summary>
    /// 플레이어의 수명 확인 및 수정용 프로퍼티
    /// </summary>
    private int Life
    {
        get => life;
        set
        {
            life = value;
            if (life > 0)
            {
                // life가 0보다 크면
                OnHit();    // 적에게 맞았을 때 처리해야 할 기능이 있는 함수
            }
            else
            {
                // life가 0보다 작거나 같으면 
                OnDie();    // 사망 처리
            }
            onLifeChange?.Invoke( life );   // 플레이어 사망 알림
            //Debug.Log($"Life : {life}");
        }
    }

    /// <summary>
    /// 플레이어의 생존 여부를 알려주는 프로퍼티
    /// </summary>
    private bool IsAlive => life > 0;

    /// <summary>
    /// 시작할 때의 플레이어의 수명
    /// </summary>
    public int initialLife = 3;

    /// <summary>
    /// 피격 당했을 때 무적 시간
    /// </summary>
    public float invincibleTime = 2.0f;

    /// <summary>
    /// 플레이어의 수명이 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<int> onLifeChange;

    /// <summary>
    /// 스프라이트 랜더러
    /// </summary>
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 무적모드인지 아닌지 확인용 변수
    /// </summary>
    bool isInvincibleMode = false;

    /// <summary>
    /// 무적모드에서 사용할 시간 누적용 변수
    /// </summary>
    float timeElapsed = 0.0f;

    /// <summary>
    /// 죽었음을 알리는 델리게이트. 파라메터는 최종 점수
    /// </summary>
    public Action<int> onDie;


    // 게임 오브젝트가 생성이 완료되면 호출되는 함수
    private void Awake()
    {
        //Debug.Log("Player의 Awake");
        inputAction = new PlayerInputAction();  // 입력 액션 객체 만들기

        anim = GetComponent<Animator>();        // Animator 컴포넌트를 찾아서 리턴하는 함수. 없으면 null
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        fireCoroutine = FireCoroutine();        // 코루틴 함수를 저장하기

        Transform fireRoot = transform.GetChild(0);     // 총알 발사위치의 루트 찾기
        fireTransforms = new Transform[fireRoot.childCount];    // 루트의 자식수만큼 배열 확보
        for( int i = 0; i < fireTransforms.Length; i++ )
        {
            fireTransforms[i] = fireRoot.GetChild(i);   // 총알 발사할 트랜스폼 미리 찾아놓기
        }
        
        fireWait = new WaitForSeconds(fireInterval);    // 코루틴에서 대기용 사용할 것 미리 만들어 놓기

        fireFlash = transform.GetChild(1).gameObject;   // 총알 발사 이팩트 찾기
        flashWait = new WaitForSeconds(0.1f);
    }

    // 게임 오브젝트를 활성화 할 때 호출되는 함수
    private void OnEnable()
    {
        // started;      // 액션이 일어난 직후(버튼을 눌렀을 때)
        // performed;    // 확실하게 액션이 일어났을 때(버튼을 확실하게 눌렀을 때)
        // canceled;     // 액션이 취소 되었을 때(버튼을 땠을 때)

        //Debug.Log("Player의 활성화");
        inputAction.Player.Enable();                        // 입력 활성화
        inputAction.Player.Move.performed += OnMove;        // 액션별 함수 연결
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.Boost.performed += OnBoost;
        inputAction.Player.Boost.canceled += OnBoost;
        inputAction.Player.Fire.performed += OnFireStart;
        inputAction.Player.Fire.canceled += OnFireStop;
    }

    // 게임 오브젝트가 비활성화 될 때 호출되는 함수
    private void OnDisable()
    {
        //Debug.Log("Player의 비활성화");
        inputAction.Player.Fire.canceled -= OnFireStop;
        inputAction.Player.Fire.performed -= OnFireStart;
        inputAction.Player.Boost.canceled -= OnBoost;
        inputAction.Player.Boost.performed -= OnBoost;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }

    // Start는 첫번째 프레임 업데이트가 일어나기 전에 호출된다.(시작할 때 호출된다.)
    void Start()
    {
        //Debug.Log("Player의 Start");
        //Vector3 newPosition = new Vector3(1, 2, 3); // 새 위치는 (1,2,3)
        //newPosition.x = 3;  // (3,2,3)
        //this.gameObject.transform.position = newPosition; // 아래와 똑같은 코드
        //transform.position = newPosition;           // 위치를 newPosition으로 변경

        //transform.position = Vector3.zero;           // 위치를 (0,0,0)으로 변경
        Power = 1;
        Life = initialLife;
    }

    // Update는 매 프레임마다 한번씩 호출된다.(게임 진행 중에 계속 호출된다.)
    void Update()
    {
        //Debug.Log("Player의 Update"); 
        // 플레이어를 계속 오른쪽으로 움직이게 하기
        // Time.deltaTime : 이전 Update함수가 호출되고 이번 Update 함수가 호출될 때까지 걸린 시간

        // 초당 speed의 속도(부스트인 상황은 2배)로 dir방향으로 움직이기
        //transform.position += (Time.deltaTime * speed * boost * direction);   // 아래와 같은 코드
        //transform.Translate(Time.deltaTime * speed * boost * direction);
        //Debug.Log(Time.deltaTime);

        // InputManager : 옛날 방식
        //if( Input.GetKey(KeyCode.Space) ) {}

        if(isInvincibleMode)        // 무적 모드일 때만 처리
        {
            timeElapsed += Time.deltaTime * 30;     // 시간 변화를 증폭시켜서(30배) 누적시키기
            float alpha = (Mathf.Cos(timeElapsed) + 1.0f) * 0.5f;   // 코사인 결과를 0~1사이로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);       // 코사인으로 계산된 알파값 설정
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log($"FixedUpdate : {Time.fixedDeltaTime}");
        //transform.Translate(Time.fixedDeltaTime * speed * boost * direction);
        
        if(IsAlive)
        {
            // 살아있을 때만 움직이기
            rigid.MovePosition(rigid.position + (Vector2)(Time.fixedDeltaTime * speed * boost * direction));
        }
        //else
        //{
        //    // 죽으면 계속 회전하게 만들기
        //    //rigid.AddTorque(30.0f);     // 회전력 더하기. z축을 기준으로 30도씩 회전
        //    //rigid.AddForce(Vector2.left * 0.3f, ForceMode2D.Impulse);   // 특정 방향으로 힘을 더하기. 왼쪽 방향으로 0.3만큼 힘을 더함
        //}
    }

    // WASD 입력이 있을 때 실행되는 함수
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();   // 입력 값 받아오기(Action type이 value인 경우만 가능)
        //Debug.Log($"이동 입력 - {value}");

        direction = value;      // 입력 받은 값을 맴버변수인 direction에 저장하기
        
        //anim.SetFloat("InputY", direction.y);     // 아래 코드와 같은 결과
        anim.SetFloat(InputY_String, direction.y);

        //dir.x = value.x;
        //dir.y = value.y;
        //dir.z = 0.0f;
    }

    // 쉬프트 키가 눌러졌을 때 실행되는 함수
    private void OnBoost(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            // 버튼이 떨어져서 실행된 경우
            boost = 1.0f;
        }
        else
        {
            // 버튼이 눌러져서 실행된 경우
            boost = 2.0f;
        }
    }

    private void OnFireStart(InputAction.CallbackContext _)
    {
        //Debug.Log("발사");

        // 이름으로 찾기. 씬 전부를 찾는다. 이름으로 비교해서 느리다.
        //GameObject temp1 = GameObject.Find("FireTransform");
        //Debug.Log(temp1);

        // 태그로 찾기. 씬 전부를 찾는다. 태그로 찾는다.(태그는 숫자로 변경될 수 있어서 조금 더 빠르다.)
        //GameObject temp2 = GameObject.FindGameObjectWithTag("Fire");

        // 특정 컴포넌트를 가진 게임 오브젝트 찾기.
        //GameObject.FindObjectOfType<Transform>();


        StartCoroutine(fireCoroutine);  // 코루틴 시작 -> 총알을 계속 만든다.
    }

    private void OnFireStop(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);   // 코루틴 끝내기 -> 총알을 더 이상 안만든다.
    }

    IEnumerator FireCoroutine()
    {
        while(true)
        {
            for(int i=0;i<Power;i++)                    // 파워 단계에 맞게 총알 생성
            {
                Transform firePos = fireTransforms[i];
                Factory.Inst.GetObject(
                    PoolObjectType.PlayerBullet,
                    firePos.position,                   // fireTransform 위치로 옮기기
                    firePos.rotation.eulerAngles.z);    // fireTransform의 회전을 적용하기
            }

            StartCoroutine(FlashEffect());  // 발사효과 표시용 코루틴 실행

            yield return fireWait;
        }
    }

    /// <summary>
    /// 점수를 추가하는 함수
    /// </summary>
    /// <param name="getScore">추가될 점수</param>
    public void AddScore(int getScore)
    {
        Score += getScore;
    }

    /// <summary>
    /// 총알 발사 효과
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashEffect()
    {
        fireFlash.SetActive(true);      // 게임 오브젝트 활성화하기
        yield return flashWait;         // 잠깐 기다리고
        fireFlash.SetActive(false);     // 게임 오브젝트 비활성화하기
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // 트리거 영역안에 들어갔다.
    //    // Collider2D collision : 상대방의 컬라이더
    //    Debug.Log($"{collision.gameObject.name}의 트리거 영역에 들어갔다.");
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    // 트리거 영역안에서 움직이고 있다.
    //    //Debug.Log($"{collision.gameObject.name}의 트리거 영역에서 움직이고 있다.");
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    // 트리거 영역에서 나왔다.
    //    Debug.Log($"{collision.gameObject.name}의 트리거 영역에서 나왔다.");
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 다른 컬라이더와 충돌했다.(컬라이더는 겹칠 수 없다.)        
        //Debug.Log($"{collision.gameObject.name}와 충돌했다.");
        if (collision.gameObject.CompareTag("Enemy") 
            || collision.gameObject.CompareTag("EnemyBullet"))
        {
            Life--;     // 적과 부딪치면 수명 감소
        }
        else if (collision.gameObject.CompareTag("PowerUp"))
        {
            Power++;    // 파워업 아이템과 부딪치면 파워 증가
            collision.gameObject.SetActive(false);  // 파워업 오브젝트 비활성화(풀로 되돌리기)
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    // 다른 컬라이더와 접촉한채로 움직이고 있다.
    //    //Debug.Log($"{collision.gameObject.name}와 접촉한채로 움직이고 있다.");
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // 다른 컬라이더와 떨어졌다.
    //    Debug.Log($"{collision.gameObject.name}와 떨어졌다.");
    //}

    /// <summary>
    /// 파워에 따라 발사위치의 갯수와 위치를 조절하는 함수
    /// </summary>
    /// <param name="power">현재 파워 단계</param>
    private void RefreshFirePositions(int power)
    {
        // 우선 모든 발사 위치를 비활성화
        for(int i = 0; i < fireTransforms.Length; i++)
        {
            fireTransforms[i].gameObject.SetActive(false);
        }

        // power 단계에 맞게 활성화하기
        for(int i=0;i<power;i++)
        {
            // 총알 간의 사이각은 30도
            // power 1 : 0도 회전
            // power 2 : -15도 회전, 15도 회전
            // power 3 : -30도, 0도, 30도 회전

            // power로 시작각 정하고 추가로 i * 발사각만큼 추가
            fireTransforms[i].rotation = Quaternion.Euler(0, 0,
                (power - 1) * (fireAngle * 0.5f) + (i * -fireAngle) );

            // 총알간에 너무 붙어서 나오는 것 방지
            fireTransforms[i].localPosition = Vector3.zero;
            fireTransforms[i].Translate(0.5f, 0, 0);

            fireTransforms[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 플레이어가 맞았을 때
    /// </summary>
    private void OnHit()
    {
        Power--;                                // 파워 한단계 떨어트리고 
        StartCoroutine(EnterInvincibleMode());  // 무적모드 진입
    }

    /// <summary>
    /// 무적 모드로 들어가고 시간끝나면 원상복귀 되는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator EnterInvincibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible"); // 레이어 변경해서 적과 충돌 안되게 만듬
        isInvincibleMode = true;    // 무적모드 들어갔다고 표시
        timeElapsed = 0.0f;         // 알파값 변화를 위한 시간 누적 변수 초기화

        yield return new WaitForSeconds(invincibleTime);        // 무적 시간이 끝날 때까지 대기

        spriteRenderer.color = Color.white;     // 알파값을 원상 복귀 시키기
        isInvincibleMode = false;               // 무적 모드 끝났다고 표시
        gameObject.layer = LayerMask.NameToLayer("Player");     // 레이어도 원상복귀
    }

    /// <summary>
    /// 죽었을 때 실행되는 함수
    /// </summary>
    private void OnDie()
    {
        Collider2D bodyCollider = GetComponent<Collider2D>();
        bodyCollider.enabled = false;   // 더이상 충돌하지 않기 위해 컬라이더 끄기

        Factory.Inst.GetObject(PoolObjectType.Explosion, transform.position);   // 터지는 이팩트 추가

        inputAction.Player.Disable();   // 플레이어 입력 막기

        direction = Vector3.zero;       // 이동 초기화
        StopAllCoroutines();            // 총알 연사 코루틴 정지

        rigid.gravityScale = 1.0f;      // 중력 다시 적용
        rigid.freezeRotation = false;   // 회전 풀기

        rigid.AddTorque(10000);         // 플레이어 회전 시키기
        rigid.AddForce(Vector2.left * 10.0f, ForceMode2D.Impulse);  // 플레이어를 뒤로 밀기

        onDie?.Invoke(Score);           // 죽었다고 신호 보내기
    }

    public void TestDie()
    {
        Life = 0;
    }
}
