using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Flags]
public enum Direction : byte
{
    North = 1, 
    East = 2, 
    South = 4,
    West = 8,
}

public class Cell
{
    /// <summary>
    /// 이 셀에 연결된 길을 기록하는 변수(북동남서 순서대로 비트에 1이 세팅되어있으면 길이 있다. 0이 세팅되어 있으면 길이 없다(벽))
    /// </summary>
    byte path;
    public byte Path => path;

    /// <summary>
    /// 그리드 상에서 X 좌표.(왼쪽->오른쪽)
    /// </summary>
    protected int x;
    public int X => x;

    /// <summary>
    /// 그리드 상에서 Y좌표.(앞->뒤)
    /// </summary>
    protected int y;
    public int Y => y;

    public Cell(int x, int y)
    {
        this.path = 0;
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// 이 셀에 길을 추가하는 함수
    /// </summary>
    /// <param name="direction">길이 생길 방향</param>
    public void MakePath(Direction direction)
    {
        path |= (byte)direction;    // or 연산자를 이용해서 비트 세팅
    }

    /// <summary>
    /// direction 방향에 길이 있는지 체크하는 함수
    /// </summary>
    /// <param name="direction">체크할 방향</param>
    /// <returns>길이 있으면 true, 벽이면 false</returns>
    public bool IsPath(Direction direction)
    {
        return (path & (byte)direction) != 0;
    }

    /// <summary>
    /// direction 방향에 벽이 있는지 체크하는 함수
    /// </summary>
    /// <param name="direction">체크할 방향</param>
    /// <returns>벽이 있으면 true, 길이면 false</returns>
    public bool IsWall(Direction direction)
    {
        return !((path & (byte)direction) != 0);
    }
}
