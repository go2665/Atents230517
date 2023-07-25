using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Base : MonoBehaviour
{
    /// <summary>
    /// 이 UI가 표현할 슬롯(연결된 슬롯)
    /// </summary>
    InvenSlot invenSlot;
    
    /// <summary>
    /// 슬롯 확인용 프로퍼티
    /// </summary>
    public InvenSlot InvenSlot => invenSlot;

    /// <summary>
    /// 슬롯이 몇번째 슬롯인지 확인하기 위한 프로퍼티
    /// </summary>
    public uint Index => invenSlot.Index;

    /// <summary>
    /// 아이템 아이콘 표시용 이미지
    /// </summary>
    Image itemIcon;

    /// <summary>
    /// 아이탬 개수 표시용 텍스트
    /// </summary>
    TextMeshProUGUI itemCount;

    protected virtual void Awake()
    {
        // 상속받은 클래스에서 추가적인 초기화가 필요하기 때문에 가상함수로 만듬
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCount = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯 초기화용 함수
    /// </summary>
    /// <param name="slot">이 UI와 연결할 슬롯</param>
    public virtual void InitializeSlot(InvenSlot slot)
    {
        invenSlot = slot;                       // 슬롯 저장
        invenSlot.onSlotItemChange = Refresh;   // 슬롯에 변화가 있을 때 실행될 함수 등록
        Refresh();                              // 초기 모습 갱신
    }

    /// <summary>
    /// 슬롯이 보이는 모습을 갱신하는 함수
    /// </summary>
    private void Refresh()
    {
        if(InvenSlot.IsEmpty)   
        {
            // 비어있으면
            itemIcon.color = Color.clear;   // 아이콘 안보이게 투명화
            itemIcon.sprite = null;         // 아이콘에서 이미지 제거
            itemCount.text = string.Empty;  // 개수도 안보이게 글자 제거
        }
        else
        {
            // 아이템이 들어있으면
            itemIcon.sprite = InvenSlot.ItemData.itemIcon;      // 아이콘에 이미지 설정
            itemIcon.color = Color.white;                       // 아이콘이 보이도록 투명도 제거
            itemCount.text = InvenSlot.ItemCount.ToString();    // 아이템 개수 설정
        }

        OnRefresh();        // 상속받은 클래스에서 개별로 실행하고 싶은 코드 실행
    }

    /// <summary>
    /// 상속받은 클래스에서 개별적으로 실행하고 싶은 코드를 모아놓은 함수
    /// </summary>
    protected virtual void OnRefresh()
    {
    }
}
