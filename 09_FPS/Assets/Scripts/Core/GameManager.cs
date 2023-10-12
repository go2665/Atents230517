using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    /// <summary>
    /// 플레이어
    /// </summary>
    public Player Player => player;

    CinemachineVirtualCamera vcamera;
    /// <summary>
    /// 플레이어를 찍는 가상카메라
    /// </summary>
    public CinemachineVirtualCamera VCamera => vcamera;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        player = FindAnyObjectByType<Player>();

        GameObject obj = GameObject.Find("PlayerFollowCamera");
        if(obj != null) 
        {
            vcamera = obj.GetComponent<CinemachineVirtualCamera>();
        }
    }
}
