using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridPainter : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject letterPrefab;

    const int gridLineCount = 11;

    private void Awake()
    {
        DrawGridLines();
        DrawGridLetters();
    }

    void DrawGridLines()
    {
        // 카메라는 위에서 아래로 바라보는 상황
        // 세로 선 그리기
        for(int i=0;i<gridLineCount; i++)
        {
            GameObject line = Instantiate(linePrefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(i, 0, 1));
            lineRenderer.SetPosition(1, new Vector3(i, 0, 1 - gridLineCount));
        }

        // 가로 선 그리기
        for (int i = 0; i < gridLineCount; i++)
        {
            GameObject line = Instantiate(linePrefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3(-1, 0, -i));
            lineRenderer.SetPosition(1, new Vector3(gridLineCount-1, 0, -i));
        }
    }

    void DrawGridLetters()
    {
        // 알파벳 가로로 찍기
        for (int i = 1; i < gridLineCount; i++)
        {
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.transform.position = new Vector3( i - 0.5f, 0, 0.5f );
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            char alphabet = (char)(64 + i);     // 아스키코드로 65가 'A'
            text.text = alphabet.ToString();
        }

        // 숫자 세로로 찍기
        for (int i = 1; i < gridLineCount; i++)
        {
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.transform.position = new Vector3(-0.5f, 0, 0.45f - i);
            TextMeshPro text = letter.GetComponent<TextMeshPro>();
            text.text = i.ToString();
            if( i>9 )
            {
                text.fontSize = 8;  // 두자리 숫자는 폰트 크기를 줄이기
            }
        }
    }
}
