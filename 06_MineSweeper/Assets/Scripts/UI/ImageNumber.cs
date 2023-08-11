using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    /// <summary>
    /// 이 UI로 표시할 숫자. 범위는 (-99~999) 
    /// </summary>
    private int number;

    /// <summary>
    /// 숫자를 확인하고 설정하는 프로퍼티
    /// </summary>
    public int Number
    {
        get => number;
        set
        {
            if( number != value)    // 값의 변화가 있을 때에만 진행
            {
                number = Mathf.Clamp(value, -99, 999);  // 범위 지정(-99~999)
                Refresh();          // 보이는 화면 갱신
            }
        }
    }

    /// <summary>
    /// 숫자가 그려져 있는 스프라이트의 배열
    /// </summary>
    public Sprite[] numberSprites;

    /// <summary>
    /// 자리수별 이미지 컴포넌트(0:제일 왼쪽(1자리), 1:중간(10자리), 2:오른쪽(100자리))
    /// </summary>
    private Image[] numberDigits;

    // 가독성을 위한 프로퍼티
    Sprite ZeroSprite => numberSprites[0];      // 0 스프라이트
    Sprite MinusSprite => numberSprites[10];    // - 스프라이트
    Sprite EmptySprite => numberSprites[11];    // 빈칸 스프라이트


    private void Awake()
    {
        numberDigits = GetComponentsInChildren<Image>();    // 숫자를 표시할 이미지 모두 찾기
    }

    /// <summary>
    /// 화면 갱신용 함수
    /// </summary>
    private void Refresh()
    {
        int temp = Mathf.Abs(number);               // 무조건 양수로 변경(우선 부호제거)
        Queue<int> digits = new Queue<int>(3);      // 자리수별로 숫자를 나누어서 저장할 큐
        
        // 자리수별로 나누어서 digits에 담기
        while(temp > 0)
        {
            digits.Enqueue(temp % 10);      // 1의 자리 값을 큐에 넣기
            temp /= 10;                     // 1의 자리 제거
        }

        // digits에 담겨진 데이터를 기반으로 이미지 표시하기
        int index = 0;
        while(digits.Count > 0)
        {
            int num = digits.Dequeue();                         // 하나씩 큐에서 꺼내고
            numberDigits[index].sprite = numberSprites[num];    // 꺼낸 값에 해당하는 스프라이트를 자리수별로 이미지에 설정하기
            index++;                                            // 다음 자리수로 이동
        }

        // 남은 칸에 이미지 설정하기
        for(int i=index;i<numberDigits.Length; i++)
        {
            numberDigits[i].sprite = ZeroSprite;    // 빈칸은 0으로 채우기
        }

        // 음수 처리
        if(number < 0)
        {
            numberDigits[numberDigits.Length - 1].sprite = MinusSprite; // 원래 음수였으면 제일 왼쪽 칸은 -로 표시
        }
    }
    
}
