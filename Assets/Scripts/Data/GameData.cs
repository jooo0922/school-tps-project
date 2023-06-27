using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 씬 간 데이터 전송 목적으로 만든 스크립터블 오브젝트
[CreateAssetMenu(menuName = "Scriptable/GameData", fileName = "Game Data")]
public class GameData : ScriptableObject
{
    [SerializeField]
    private string mSelectedCharacter; // Lobby 씬에서 선택된 캐릭터 이름

    // selectedCharacter 데이터 읽기 / 쓰기를 제어하는 프로퍼티
    public string selectedCharacter
    {
        get { return mSelectedCharacter; }
        set { mSelectedCharacter = value; }
    }
}
