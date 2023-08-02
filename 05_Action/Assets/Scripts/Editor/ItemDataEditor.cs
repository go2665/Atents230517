using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//ItemData용 커스텀 에티터. 두번째 파라메터가 true면 ItemData를 상속받은 자식클래스도 같이 적용 받는다.
[CustomEditor(typeof(ItemData), true)]
public class ItemDataEditor : Editor
{
    ItemData itemData;

    private void OnEnable()
    {
        itemData = target as ItemData;  // target은 에디터에서 선택한 에셋
    }

    // 인스팩터 창의 내부를 그리는 함수
    public override void OnInspectorGUI()
    {
        if(itemData != null)                // 아이템 데이터가 있어야 하고
        {
            if( itemData.itemIcon != null ) // 아이템 데이터에 아이콘 이미지가 있어야 한다.
            {
                Texture2D texture;
                EditorGUILayout.LabelField("Item Icon Preview");            // 제목 적기
                texture = AssetPreview.GetAssetPreview(itemData.itemIcon);  // 텍스쳐 가져오기(아이템 데이터에 있는 아이콘 이미지 기반)
                if( texture != null )                                               // 텍스쳐가 있으면
                {
                    GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // 텍스쳐가 그려질 영역 설정
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // 앞에서 설정한 영역에 텍스쳐 그리기
                }
            }
        }

        base.OnInspectorGUI();  // 기본적으로 그리던 것은 그대로 그리기
    }
}
#endif