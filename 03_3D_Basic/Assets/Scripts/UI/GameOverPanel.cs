using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.onDie += (_) => canvasGroup.alpha = 1;
        }
    }
}
