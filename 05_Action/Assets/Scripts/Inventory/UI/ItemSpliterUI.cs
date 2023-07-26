using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpliterUI : MonoBehaviour
{
    /// <summary>
    /// 아이템을 덜어낼 슬롯
    /// </summary>
    InvenSlot targetSlot;

    /// <summary>
    /// 덜어낼 수 있는 아이템 개수의 최소값
    /// </summary>
    const uint MinItemCount = 1;

    /// <summary>
    /// 덜어낼 아이템 개수
    /// </summary>
    uint itemSplitCount = MinItemCount;

    /// <summary>
    /// 덜어낼 아이템 개수를 확인하고 설정하는 프로퍼티
    /// </summary>
    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            // 덜어낼 개수의 범위 = 1 ~ (대상슬롯이 가진 아이템 개수 - 1)
            itemSplitCount = Math.Clamp(value, MinItemCount, targetSlot.ItemCount - 1);
            
            inputField.text = itemSplitCount.ToString();    // 인풋 필드에 적용
            slider.value = itemSplitCount;                  // 슬라이더에 적용
        }
    }

    /// <summary>
    /// OK 버튼이 눌려졌을 때 실행될 델리게이트(파라메터: 덜어낼 대상 슬롯의 인덱스, 덜어낼 개수)
    /// </summary>
    public Action<uint, uint> onOkClick;

    /// <summary>
    /// Cancel버튼이 눌려졌을 때 실행될 델리게이트
    /// </summary>
    public Action onCancel;

    // 컴포넌트들
    Image itemIcon;
    TMP_InputField inputField;
    Slider slider;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();

        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onValueChanged.AddListener( (text) =>
        {
            // inputField의 값이 변경되었을 때 실행될 함수
            if( uint.TryParse(text, out uint result) )
            {
                ItemSplitCount = result;        // 텍스트를 숫자로 바꿔서 저장
            }
            else
            {
                ItemSplitCount = MinItemCount;  // 잘못된 입력일 경우는 최소값으로 설정
            }
        });

        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener((ratio) => 
        { 
            // 슬라이더의 값이 변경되었을 때 실행될 함수
            ItemSplitCount = (uint)ratio;   // 최대최소가 적절히 변경되어있어서 바로 대입 가능
        });

        child = transform.GetChild(2);
        Button plus = child.GetComponent<Button>();
        plus.onClick.AddListener(() =>
        {
            ItemSplitCount++;   // plus버튼이 눌려지면 덜어낼 개수 1증가
        });

        child = transform.GetChild(3);
        Button minus = child.GetComponent<Button>();
        minus.onClick.AddListener(() =>
        {
            ItemSplitCount--;   // minus버튼이 눌려지면 덜어낼 개수 1감소
        });

        child = transform.GetChild(5);
        Button ok = child.GetComponent<Button>();
        ok.onClick.AddListener(() =>
        {
            onOkClick?.Invoke(targetSlot.Index, ItemSplitCount);    // ok 버튼이 눌려지면 신호보내고 닫기
            Close();
        });

        child = transform.GetChild(6);
        Button cancel = child.GetComponent<Button>();
        cancel.onClick.AddListener( () =>
        {
            onCancel?.Invoke();     // cancel버튼이 눌려지면 신호보내고 닫기
            Close();                
        });
    }

    /// <summary>
    /// 아이템 분리창을 여는 함수
    /// </summary>
    /// <param name="target">아이템을 분리할 대상 슬롯</param>
    public void Open(InvenSlot target)
    {
        if( !target.IsEmpty && target.ItemCount > MinItemCount )    // 대상에 아이템이 있고 최소 개수 이상으로 있을 때만 창 열기
        {
            targetSlot = target;                            // 대상 저장
            itemIcon.sprite = targetSlot.ItemData.itemIcon; // 아이콘 변경
            slider.minValue = MinItemCount;                 // 슬라이더 최소/최대 수정
            slider.maxValue = targetSlot.ItemCount - 1;
            ItemSplitCount = MinItemCount;                  // 분리할 개수를 최소값으로 설정
            gameObject.SetActive(true);                     // 활성화해서 보여주기
        }
    }

    /// <summary>
    /// 아이템 분리창을 닫는 함수
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);    // 비활성화해서 안보여주기
    }
}
