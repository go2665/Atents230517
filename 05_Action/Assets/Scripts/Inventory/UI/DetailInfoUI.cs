using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DetailInfoUI : MonoBehaviour
{
    /// <summary>
    /// 아이템의 아이콘을 표시할 이미지
    /// </summary>
    Image itemIcon;

    /// <summary>
    /// 아이템의 이름을 출력할 텍스트
    /// </summary>
    TextMeshProUGUI itemName;

    /// <summary>
    /// 아이템의 가격을 출력할 텍스트
    /// </summary>
    TextMeshProUGUI itemPrice;

    /// <summary>
    /// 아이템의 설명을 출력할 텍스트
    /// </summary>
    TextMeshProUGUI itemDescription;

    /// <summary>
    /// 디테일창 전체의 알파를 조절할 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;

    /// <summary>
    /// 디테일창의 일시 정지 여부를 표시하는 변수
    /// </summary>
    bool isPause = false;

    /// <summary>
    /// 일시정지 여부를 확인 및 설정하는 프로퍼티
    /// </summary>
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if(isPause)
            {
                Close();    // 일시 정지가 되면 열려 있던 것도 닫는다.
            }
        }
    }

    /// <summary>
    /// 디테일창이 열리고 닫히는 속도
    /// </summary>
    public float alphaChangeSpeed = 10.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        itemPrice = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        itemDescription = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 상세정보창을 여는 함수
    /// </summary>
    /// <param name="data">상세정보창에서 표시할 아이템의 데이터</param>
    public void Open(ItemData data)
    {
        if(!IsPause && data != null)    // 일시정지 상태가 아니고, 아이템 데이터가 있을 때만 열기
        {
            itemIcon.sprite = data.itemIcon;                // 아이콘 설정
            itemName.text = data.itemName;                  // 이름 설정
            itemPrice.text = data.price.ToString("N0");     // 가격 설정(3자리마다 콤마 추가)
            itemDescription.text = data.itemDescription;    // 설명 설정

            StopAllCoroutines();
            StartCoroutine(FadeIn());                       // 알파를 점점 1이 되도록 설정해서 보이게 만들기

            MovePosition(Mouse.current.position.ReadValue());   // 열릴 때 마우스 커서 위치에 열리기

        }
    }

    /// <summary>
    /// 상세정보창을 닫는 함수
    /// </summary>
    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());  // 알파를 점점 0이 되도록 설정해서 안보이게 만들기
    }

    /// <summary>
    /// 상세정보창을 움직이는 함수
    /// </summary>
    /// <param name="screenPos">위치할 스크린 좌표</param>
    public void MovePosition(Vector2 screenPos) 
    { 
        if( canvasGroup.alpha > 0.0f )      // 보이는 상황일 때만
        {
            RectTransform rectTransform = (RectTransform)transform;     // rectTransform 가져오기

            int overX = (int)(screenPos.x + rectTransform.sizeDelta.x) - Screen.width;  // 화면 밖으로 넘친 정도를 계산
            overX = Mathf.Max(0, overX);        // 음수 제거(음수면 정상 범위)
            screenPos.x -= overX;               // 넘친만큼 왼쪽으로 이동시키기

            transform.position = screenPos;     // 이동 시키기
        }
    }

    /// <summary>
    /// 디테일창을 점점 보이게 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1.0f)    // 알파가 1이 될 때까지 매 프레임마다 조금씩 증가
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
    }

    /// <summary>
    /// 디테일창이 점점 안보이게 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0.0f)    // 알파가 0이 될 때까지 매프레임마다 조금씩 감소
        {
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
    }
}
