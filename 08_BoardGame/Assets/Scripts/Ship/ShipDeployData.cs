using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployData
{
    /// <summary>
    /// 배의 방향
    /// </summary>
    private ShipDirection direction;
    public ShipDirection Direction => direction;
    
    /// <summary>
    /// 배의 위치
    /// </summary>
    private Vector2Int position;
    public Vector2Int Position => position;

    /// <summary>
    /// 배치 정보의 생성자
    /// </summary>
    /// <param name="dir">배치된 방향</param>
    /// <param name="pos">배치된 위치</param>
    public ShipDeployData(ShipDirection dir, Vector2Int pos)
    {
        direction = dir;
        position = pos;
    }

}
