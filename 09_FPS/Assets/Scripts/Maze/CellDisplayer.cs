using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDisplayer : MonoBehaviour
{
    /// <summary>
    /// 바닥 한변의 크기
    /// </summary>
    public const int CellSize = 5;  
 
    GameObject[] walls;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        walls = new GameObject[child.childCount];
        for(int i=0;i<walls.Length;i++)
        {
            walls[i] = child.GetChild(i).gameObject;
        }
    }

    public void RefreshWall(int data)
    {
        // data에 자리수별로 비트가 1이면 경로가 있다(=벽이 없다). 0이면 경로가 없다(=벽이 있다).
        for(int i=0;i<walls.Length;i++)                     // 모든 벽을 체크
        {
            int mask = 1 << i;                              // 마스크 만들기(1,2,4,8 순서)
            walls[i].SetActive( !((data & mask) != 0) );    // &시켜서 비트가 세팅되어 있는지 확인하고 세팅되어 있으면 벽을 제거
        }
    }
}
