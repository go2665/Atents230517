using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetSingleton<GameManager>
{
    /// <summary>
    /// 로거(텍스트 출력용. 채팅창으로도 사용됨)
    /// </summary>
    Logger logger;

    /// <summary>
    /// 내 플레이어(이 실행(클라이언트 프로그램)에서 접속한 플레이어)
    /// </summary>
    NetPlayer player;
    NetPlayerDecoration deco;

    /// <summary>
    /// 내 플레이어 확인용 프로퍼티
    /// </summary>
    public NetPlayer Player => player;
    public NetPlayerDecoration PlayerDeco => deco;

    /// <summary>
    /// 현재 동시접속자수
    /// </summary>
    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);

    /// <summary>
    /// 동시접속자수가 변경되면 실행될 델리게이트
    /// </summary>
    public Action<int> onPlayersInGameChange;

    /// <summary>
    /// 현재 접속자의 이름이 될 변수
    /// </summary>
    string userName = "디폴트";
    public string UserName
    {
        get => userName;
        set
        {
            userName = value;
            onUserNameChange?.Invoke(userName); // 이름이 변경되면 델리게이트로 알림
        }
    }
    /// <summary>
    /// 이름이 변경되었을 때 알람을 보낼 델리게이트
    /// </summary>
    public Action<string> onUserNameChange;

    /// <summary>
    /// 현재 접속자의 색상이 될 변수
    /// </summary>
    Color userColor = Color.clear;
    public Color UserColor
    {
        get => userColor;
        set
        {
            userColor = value;
            onUserColorChange?.Invoke(userColor);
        }
    }
    /// <summary>
    /// 색상이 변경되었을 때 알람을 보낼 델리게이트
    /// </summary>
    public Action<Color> onUserColorChange;

    /// <summary>
    /// 내 캐릭터를 따라다닐 가상카메라
    /// </summary>
    CinemachineVirtualCamera virtualCamera;

    /// <summary>
    /// 내 캐릭터를 따라다닐 가상카메라를 확인하기 위한 프로퍼티
    /// </summary>
    public CinemachineVirtualCamera VCam => virtualCamera;
        

    protected override void OnInitialize()
    {
        logger = FindObjectOfType<Logger>();    // 로거는 로컬에서 사용되는 것이니까 그냥 찾기
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;      // 어떤 클라이언트가 접속할 때마다 실행될 함수 등록
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;  // 어떤 클라이언트가 접속 해제할 때마다 실행될 함수 등록

        playersInGame.OnValueChanged += (_,newValue) => onPlayersInGameChange?.Invoke(newValue);
    }

    /// <summary>
    /// 어떤 클라이언트가 접속할 때마다 실행될 함수
    /// </summary>
    /// <param name="id">접속한 대상의 클라이언트ID</param>
    private void OnClientConnect(ulong id)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);    // id를 이용해서 네트워크 오브젝트 가져오기
        if (netObj.IsOwner)
        {
            player = netObj.GetComponent<NetPlayer>();  // 네트워크 오브젝트의 오너가 true면 내 캐릭터라는 의미 => 따로 저장
            player.gameObject.name = $"Player_{id}";    // 내 게임 오브젝트 이름 바꾸기

            deco = netObj.GetComponent<NetPlayerDecoration>();
            if (UserColor != Color.clear)
            {
                deco.SetColor(userColor);               // 색상이 지정되어 있으면 지정된 색상으로 설정
            }

            deco.SetName($"{userName}_{id}");     // 게임 매니저가 로컬로 가지고 있던 이름을 자신의 이름으로 설정(네트워크로 공유)

            foreach (var net in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList)    // 네트워크에서 스폰된 모든 오브젝트 순회
            {
                NetPlayer netPlayer = net.GetComponent<NetPlayer>();
                if(netPlayer != null && player != netPlayer)            // NetPlayer이고 내 Player와 다르다.
                {
                    netPlayer.gameObject.name = $"OtherPlayer_{id}";    // 내 게임 오브젝트가 아닌 것들의 이름 변경하기
                }

                NetPlayerDecoration netDeco = net.GetComponent<NetPlayerDecoration>();
                if(netDeco != null && deco != netDeco)
                {
                    netDeco.RefreshNamePlate();         // 이름판에 쓰여진 이름을 네트워크상에서 공유되는 이름으로 변경
                }
            }
        }
        else
        {
            // 내가 접속한 이후에 다른 사람이 접속하면 실행될 부분
            NetPlayer netPlayer = netObj.GetComponent<NetPlayer>();
            if (netPlayer != null && player != netPlayer)           // NetPlayer이고 내 Player와 다르다.
            {
                netObj.gameObject.name = $"OtherPlayer_{id}";       // 다른 사람의 게임 오브젝트 이름 변경하기
            }
        }

        if (IsServer)
        {
            playersInGame.Value++;  // 서버에서만 이 값을 증가시키기
            //Log($"PlayerInGame : {playersInGame.Value}");
        }
    }

    /// <summary>
    /// 어떤 클라이언트가 접속 해제할 때마다 실행될 함수
    /// </summary>
    /// <param name="id"></param>
    private void OnClientDisconnect(ulong id)
    {
        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);    // id를 이용해서 네트워크 오브젝트 가져오기
        if (netObj.IsOwner)
        {
            player = null;
        }

        if (IsServer)
        {
            playersInGame.Value--;  // 서버에서만 이 값을 감소시키기
            //Log($"PlayerInGame : {playersInGame.Value}");
        }
    }

    /// <summary>
    /// 로그만 남기는 함수(클라이언트 용)
    /// </summary>
    /// <param name="message">로그 텍스트</param>
    public void Log(string message)
    {
        logger.Log(message);
    }

}
