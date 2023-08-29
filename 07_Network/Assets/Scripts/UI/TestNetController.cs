using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestNetController : MonoBehaviour
{
    TextMeshProUGUI playerInGame;
    TextMeshProUGUI userName;

    string colorText;
    string userNameText;

    private void Start()
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

        // 동접자 변경 적용용
        child = transform.GetChild(3);
        playerInGame = child.GetComponent<TextMeshProUGUI>();
        GameManager.Inst.onPlayersInGameChange += (playerCount) => playerInGame.text = $"Players : {playerCount}";

        // 플레이어 이름 변경 적용용
        userNameText = GameManager.Inst.UserName;
        colorText = $"<#{ColorUtility.ToHtmlStringRGB(Color.black)}>";

        child = transform.GetChild(4);
        userName = child.GetComponent<TextMeshProUGUI>();
        GameManager.Inst.onUserNameChange += (name) =>
        {
            userNameText = name;
            userName.text = $"Name : {colorText}{userNameText}</color>";
        };
        GameManager.Inst.onUserColorChange += (color) =>
        {
            colorText = $"<#{ColorUtility.ToHtmlStringRGB(color)}>";
            userName.text = $"Name : {colorText}{userNameText}</color>";
        };
    }
}
