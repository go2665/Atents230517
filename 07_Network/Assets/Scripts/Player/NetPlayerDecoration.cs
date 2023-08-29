using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;

public class NetPlayerDecoration : NetworkBehaviour
{
    NetworkVariable<Color> bodyColor = new NetworkVariable<Color>();

    Renderer playerRenderer;
    Material bodyMaterial;

    NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();
    NamePlate namePlate;

    private void Awake()
    {
        bodyColor.OnValueChanged += OnBodyColorChange;

        playerRenderer = GetComponentInChildren<Renderer>();
        bodyMaterial = playerRenderer.material;

        namePlate = GetComponentInChildren<NamePlate>();
    }

    private void OnBodyColorChange(Color previousValue, Color newValue)
    {
        bodyMaterial.SetColor("_BaseColor", newValue);
    }

    public override void OnNetworkSpawn()
    {        
        if( IsServer )  // 이 클라이언트가 Server다
        {
            // 서버쪽에서만 색상을 랜덤으로 지정
            //if ( GameManager.Inst.UserColor == Color.clear)     // UserColor가 투명이면 랜덤으로 새로 정하기
            //{
            bodyColor.Value = UnityEngine.Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);  
            //}
            //else
            //{
                //bodyColor.Value = GameManager.Inst.UserColor;
            //}
        }
        bodyMaterial.SetColor("_BaseColor", bodyColor.Value);   // 지정된 색상을 적용
    }

    public void SetColor(Color color)
    {
        if( IsOwner )
        {
            if(IsServer )
            {
                bodyColor.Value = color;
            }
            else
            {
                RequestBodyColorChangeServerRpc(color);
            }
        }
    }

    [ServerRpc]
    void RequestBodyColorChangeServerRpc(Color color)
    {
        bodyColor.Value = color;
    }
}
