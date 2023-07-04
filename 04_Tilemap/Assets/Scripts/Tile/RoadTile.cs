using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoadTile : Tile
{
    [Flags]                         // 이 enum은 비트플래그로 사용한다는 어트리뷰트
    enum AdjTilePosition : byte     // 이 enum의 크기는 1 byte
    {
        None = 0,   // 0000 0000
        North = 1,  // 0000 0001
        East = 2,   // 0000 0010
        South = 4,  // 0000 0100
        West = 8,   // 0000 1000
        All = North | East | South | West   // 0000 1111
    }


    /// <summary>
    /// 길을 구성할 이미지 파일들
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// 타일이 그려질 때 자동으로 호출이 되는 함수
    /// </summary>
    /// <param name="position">타일의 위치(그리드의 좌표값)</param>
    /// <param name="tilemap">이 타일이 그려지는 타일맵</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for(int y = -1; y<2;y++)
        {
            for(int x = -1; x<2;x++)
            {
                Vector3Int location = new(position.x + x, position.y + y, position.z);
                if( HasThisTile(tilemap, location) )
                {
                    tilemap.RefreshTile(location);  // 같은 타일이면 갱신
                }
            }
        }
    }

    /// <summary>
    /// 타일 맵의 RefreshTile 함수가 호출되었을 때 어떤 스프라이트를 그릴지 결정하는 함수
    /// </summary>
    /// <param name="position">타일 데이터를 가져올 타일의 위치</param>
    /// <param name="tilemap">타일 데이터를 가져올 타일맵</param>
    /// <param name="tileData">가져온 타일 데이터의 참조(읽기, 쓰기 둘 다 가능)</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        AdjTilePosition mask = AdjTilePosition.None;
                
        // 주변 타일의 상황 기록
        //if( HasThisTile(tilemap, position + new Vector3Int(0,1,0)) )
        //{
        //    mask = mask | AdjTilePosition.North;
        //}
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.West : 0;

        int index = GetIndex(mask); // 어떤 스프라이트를 그릴 것인지 결정
        if( index > -1 )
        {
            tileData.sprite = sprites[index];
            tileData.color = Color.white;
            Matrix4x4 m = tileData.transform;
            m.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one); // 회전 적용
            tileData.transform = m;                     // 행렬 적용
            tileData.flags = TileFlags.LockTransform;   // 트랜스폼 잠금(다른 타일이 회전을 못시키게 만들기)
            tileData.colliderType = ColliderType.None;  // 컬라이더 없음
        }
        else
        {
            Debug.LogError($"잘못된 인덱스 : {index}, mask = {mask}");
        }

    }

    /// <summary>
    /// 마스크 상황에 따라 몇번째 스프라이트를 그려야 하는지 알려주는 함수
    /// </summary>
    /// <param name="mask">주변 타일 상황 표시한 마스크</param>
    /// <returns>그려야 하는 스프라이트의 인덱스</returns>
    int GetIndex(AdjTilePosition mask)
    {
        int index = -1;

        switch (mask)
        {
            case AdjTilePosition.None:
            case AdjTilePosition.North:
            case AdjTilePosition.East:
            case AdjTilePosition.South:
            case AdjTilePosition.West:
            case AdjTilePosition.North | AdjTilePosition.South:
            case AdjTilePosition.East | AdjTilePosition.West:
                index = 0;  // 1자 모양의 스프라이트
                break;
            case AdjTilePosition.South | AdjTilePosition.West:
            case AdjTilePosition.North | AdjTilePosition.West:
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.South | AdjTilePosition.East:
                index = 1;  // ㄱ자 모양의 스프라이트
                break;
            case AdjTilePosition.All & ~AdjTilePosition.North:  // 1111 & ~0001 = 1111 & 1110 = 1110
            case AdjTilePosition.All & ~AdjTilePosition.East:
            case AdjTilePosition.All & ~AdjTilePosition.South:
            case AdjTilePosition.All & ~AdjTilePosition.West:
                index = 2; // ㅗ자 모양의 스프라이트
                break;
            case AdjTilePosition.All:
                index = 3;  // +자 모양의 스프라이트
                break;
            default:
                break;
        }

        return index;
    }

    /// <summary>
    /// 마스크 상황에 따라 스프라이트를 얼마나 회전 시킬 것인지 결정하는 함수
    /// </summary>
    /// <param name="mask">주변 타일 상황 표시한 마스크</param>
    /// <returns>최종 회전</returns>
    Quaternion GetRotation(AdjTilePosition mask)
    {
        Quaternion rotate = Quaternion.identity;

        switch (mask)
        {
            case AdjTilePosition.East:                          // 1자
            case AdjTilePosition.West:
            case AdjTilePosition.East | AdjTilePosition.West:
            case AdjTilePosition.North | AdjTilePosition.West:  // ㄱ자
            case AdjTilePosition.All & ~AdjTilePosition.West:   // ㅗ자
                rotate = Quaternion.Euler(0, 0, -90);
                break;
            case AdjTilePosition.North | AdjTilePosition.East:  // ㄱ자
            case AdjTilePosition.All & ~AdjTilePosition.North:  // ㅗ자
                rotate = Quaternion.Euler(0, 0, -180);
                break;
            case AdjTilePosition.South | AdjTilePosition.East:  // ㄱ자
            case AdjTilePosition.All & ~AdjTilePosition.East:   // ㅗ자
                rotate = Quaternion.Euler(0, 0, -270);
                break;
        }
        return rotate;
    }

    /// <summary>
    /// 특정 타일맵의 특정 위치에 이 타일과 같은 종류의 타일이 있는지 확인하는 함수
    /// </summary>
    /// <param name="tilemap">확인할 타일맵</param>
    /// <param name="position">확인할 위치(그리드 좌표)</param>
    /// <returns>true면 같은 종류의 타일, false면 다른 종류의 타일</returns>
    bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;   // 타일맵이 가지고 있는 것은 원본 타일에 대한 참조
    }

#if UNITY_EDITOR
    // 메뉴에 항목 추가
    [MenuItem("Assets/Create/2D/Tiles/Custom/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject( // 파일 저장 창 열고 입력결과를 path 저장
            "Save Road Tile",   // 제목
            "New Road Tile",    // 파일의 기본 이름
            "Asset",            // 파일의 기본 확장자
            "Save Road Tile",   // 출력 메세지
            "Assets");          // 열리는 기본 폴더
        if(path != string.Empty)    // 뭔가 입력이 되었으면
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    // RoadTile를 파일로 저장
        }
    }
#endif
}
