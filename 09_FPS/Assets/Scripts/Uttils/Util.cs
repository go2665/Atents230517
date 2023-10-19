using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Util
{
    public static void Shuffle<T>(T[] source)
    {
        for(int i=source.Length-1; i>-1; i--)
        {
            int index = Random.Range(0, i);
            (source[index], source[i]) = (source[i], source[index]);
        }
    }
}
