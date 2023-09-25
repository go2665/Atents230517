using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishDeploymentButton : MonoBehaviour
{
    Button button;
    UserPlayer player;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        player = GameManager.Inst.UserPlayer;
        foreach(var ship in player.Ships)
        {
            ship.onDeploy += OnShipDeployed;
        }
    }

    private void OnShipDeployed(bool isDeployed)
    {
        if(isDeployed && player.IsAllDeployed)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    void OnClick()
    {
        UserPlayer player = GameManager.Inst.UserPlayer;

        Debug.Log("Finish 버튼 클릭");
    }
}
