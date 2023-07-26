using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailInfo : MonoBehaviour
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
        if(data != null)    // 아이템 데이터가 있을 때만 열기
        {
            itemIcon.sprite = data.itemIcon;                // 아이콘 설정
            itemName.text = data.itemName;                  // 이름 설정
            itemPrice.text = data.price.ToString("N0");     // 가격 설정(3자리마다 콤마 추가)
            itemDescription.text = data.itemDescription;    // 설명 설정

            canvasGroup.alpha = 1.0f;                       // 알파를 1로 설정해서 보이게 만들기
        }
    }

    /// <summary>
    /// 상세정보창을 닫는 함수
    /// </summary>
    public void Close()
    {
        canvasGroup.alpha = 0.0f;   // 알파를 0으로 설정해서 안보이게 만들기
    }

    /// <summary>
    /// 상세정보창을 움직이는 함수
    /// </summary>
    /// <param name="screenPos">위치할 스크린 좌표</param>
    public void MovePosition(Vector2 screenPos) 
    { 
        if( canvasGroup.alpha > 0.0f )      // 보이는 상황일 때만
        {
            transform.position = screenPos; // 이동 시키기
        }
    }
}

// 실습
// 1. 디테일창이 화면을 벗어나지 않게 만들기
// 2. 드래그를 시작하면 디테일 창이 닫히게 만들기
// 3. 디테일창의 알파 변화를 부드럽게 만들기
