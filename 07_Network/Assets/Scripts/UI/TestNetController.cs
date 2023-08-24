using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestNetController : MonoBehaviour
{
    private void Awake()
    {
        Transform child = transform.GetChild(0);
        Button startHost = child.GetComponent<Button>();
        startHost.onClick.AddListener(() =>
        {
            if( NetworkManager.Singleton.StartHost() )      // 호스트로 시작 시도
            {
                Debug.Log("호스트로 시작했습니다.");
            }
            else
            {
                Debug.Log("호스트로 시작을 실패했습니다.");
            }
        });

        child = transform.GetChild(1);
        Button startClient = child.GetComponent<Button>();
        startClient.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())     // 클라이언트로 호스트(서버)에 접속 시도
            {
                Debug.Log("클라이언트로 연결을 시작했습니다.");
            }
            else
            {
                Debug.Log("클라이언트로 연결을 실패했습니다.");
            }
        });

        child = transform.GetChild(2);
        Button disconnect = child.GetComponent<Button>();
        disconnect.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
        });
    }
}
