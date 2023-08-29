using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class NetPlayerDecoration : NetworkBehaviour
{
    NetworkVariable<Color> color = new NetworkVariable<Color>();

    Renderer playerRenderer;
    Material bodyMaterial;

    private void Awake()
    {
        playerRenderer = GetComponentInChildren<Renderer>();
        bodyMaterial = playerRenderer.material;
    }

    public override void OnNetworkSpawn()
    {        
        if( IsServer )  // 이 클라이언트가 Server다
        {
            color.Value = UnityEngine.Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);  // 서버쪽에서만 색상을 랜덤으로 지정
        }
        bodyMaterial.SetColor("_BaseColor", color.Value);   // 지정된 색상을 적용
    }
}
