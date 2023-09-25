using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDeploymentButton : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);        
    }

    void OnClick()
    {
        UserPlayer player = GameManager.Inst.UserPlayer;

        if(player.IsAllDeployed )           // 배가 전부 배치된 상황이면
        {   
            player.UndoAllShipDeployment(); // 배치 리셋
        }
        player.AutoShipDeployment(true);    // 남은 배 자동 배치
    }
}
