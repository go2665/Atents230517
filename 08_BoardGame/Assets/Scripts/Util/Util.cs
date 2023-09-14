using UnityEngine;

public class Util 
{
    /// <summary>
    /// 피셔 예이츠 알고리즘
    /// </summary>
    /// <param name="source">순서를 섞을 배열</param>
    public static void Shuffle<T>(T[] source)
    {
        for(int i=source.Length-1; i > -1; i--)
        {
            int index = Random.Range(0, i);
            (source[index], source[i]) = (source[i], source[index]);
        }
    }
}
