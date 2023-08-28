using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

    /// <summary>
    /// 내 플레이어 확인용 프로퍼티
    /// </summary>
    public NetPlayer Player => player;

    /// <summary>
    /// 현재 동시접속자수
    /// </summary>
    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);

    protected override void OnInitialize()
    {
        logger = FindObjectOfType<Logger>();    // 로거는 로컬에서 사용되는 것이니까 그냥 찾기

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;      // 어떤 클라이언트가 접속할 때마다 실행될 함수 등록
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;  // 어떤 클라이언트가 접속 해제할 때마다 실행될 함수 등록
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

            foreach(var net in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList)    // 네트워크에서 스폰된 모든 오브젝트 순회
            {
                NetPlayer netPlayer = net.GetComponent<NetPlayer>();
                if(netPlayer != null && player != netPlayer)            // NetPlayer이고 내 Player와 다르다.
                {
                    netPlayer.gameObject.name = $"OtherPlayer_{id}";    // 내 게임 오브젝트가 아닌 것들의 이름 변경하기
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
