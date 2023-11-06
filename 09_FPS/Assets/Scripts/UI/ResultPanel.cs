using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    TextMeshProUGUI title;
    TextMeshProUGUI kill;
    TextMeshProUGUI time;
    Button restart;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        title = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        kill = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        time = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(5);
        restart = child.GetComponent<Button>();
        restart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    public void Open(bool isClear, int killCount, float playTime)
    {
        if (isClear)
        {
            title.text = "Game Clear";
        }
        else
        {
            title.text = "Game Over";
        }
        kill.text = killCount.ToString();
        time.text = playTime.ToString("f1");
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
    }


}
