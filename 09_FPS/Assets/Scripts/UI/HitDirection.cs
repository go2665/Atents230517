using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDirection : MonoBehaviour
{
    public float duration = 0.5f;
    float timeElapsed = 0;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onAttacked += PlayerAttacked;
        image.color = Color.clear;
        timeElapsed = duration;
    }

    private void PlayerAttacked(float angle)
    {
        image.color = Color.white;
        timeElapsed = 0;
        this.gameObject.SetActive(true);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        float alpha = timeElapsed / duration;

        image.color = Color.Lerp(Color.white, Color.clear, alpha);

    }
}
