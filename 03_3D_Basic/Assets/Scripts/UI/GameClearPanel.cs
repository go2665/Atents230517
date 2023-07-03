using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Goal goal = FindObjectOfType<Goal>();
        if (goal != null)
        {
            goal.onGoalIn += () => canvasGroup.alpha = 1;
        }
    }
}
