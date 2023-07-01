using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro (TMP) 관련 코드

public class LobbySelectCharacterManager : MonoBehaviour
{
    [Header("Characters")]
    public List<GameObject> characterModels = new List<GameObject>(); // 캐릭터 모델 게임 오브젝트
    public List<PlayerCharacterData> characterDatas = new List<PlayerCharacterData>(); // 캐릭터 데이터 스크립터블 오브젝트
    public TextMeshProUGUI characterHealthText; // 캐릭터 체력 텍스트
    public TextMeshProUGUI characterSpeedText; // 캐릭터 스피드 텍스트
    public TextMeshProUGUI characterJumpForceText; // 캐릭터 점프력 텍스트

    [Header("Data")]
    public GameData gameData; // 씬 간 데이터 전송을 위한 스크립터블 오브젝트 데이터

    // 캐릭터 썸네일 버튼 클릭 콜백
    public void OnCharacterThumbnailBtnClick(string characterName)
    {
        foreach (GameObject characterModel in characterModels)
        {
            if (characterModel.name == characterName)
            {
                characterModel.SetActive(true); // 콜백 메서드로 전달받은 이름과 동일한 캐릭터 모델만 활성화
            }
            else
            {
                characterModel.SetActive(false); // 나머지 캐릭터 모델들은 전부 비활성화
            }
        }

        foreach (PlayerCharacterData characterData in characterDatas)
        {
            // 입력받은 캐릭터 이름과 일치하는 캐릭터 데이터로 캐릭터 스탯 UI 변경
            if (characterData.characterName == characterName)
            {
                characterHealthText.text = characterData.health.ToString();
                characterSpeedText.text = characterData.speed.ToString();
                characterJumpForceText.text = characterData.jumpForce.ToString();
            }
        }

        gameData.selectedCharacter = characterName; // Main 씬으로 선택된 캐릭터 정보를 전달하기 위해 스크립터블 오브젝트 데이터에 저장
    }

}
