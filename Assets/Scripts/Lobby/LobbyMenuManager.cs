using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject characterPreview; // 캐릭터 프리뷰 패널
    public GameObject selectCharacterPanel; // 캐릭터 선책 창 패널
    public GameObject settingsPanel; // 세팅 창 패널
    public GameObject lobbyBlur; // blur 필터

    [Header("Modal")]
    public GameObject quitGameUI; // 게임 종료 모달 UI

    // Play Game 버튼 클릭 콜백
    public void OnPlayGameBtnClick()
    {
        // TODO: Play Game 버튼 클릭 시, 포톤 API 로 서버 접속 및 씬 전환 구현
        // TODO: 로딩화면 구현 (ChatGPT 검색 결과, 로딩화면용 씬을 별도로 만드는 것을 권장)
    }

    // Settings 버튼 클릭 콜백
    public void OnSettingsBtnClick()
    {
        HideAllPanels(); // 기존 패널 모두 닫기

        lobbyBlur.SetActive(true);
        settingsPanel.SetActive(true);
    }

    // Select Characters 버튼 클릭 콜백
    public void OnSelectCharacterBtnClick()
    {
        HideAllPanels(); // 기존 패널 모두 닫기

        lobbyBlur.SetActive(true);
        characterPreview.SetActive(true);
        selectCharacterPanel.SetActive(true);
    }

    // Quit Game 버튼 클릭 콜백
    public void OnQuitGameBtnClick()
    {
        HideAllPanels(); // 기존 패널 모두 닫기

        lobbyBlur.SetActive(true);
        quitGameUI.SetActive(true);
    }

    // 모달 Quit 버튼 클릭 콜백
    public void OnModalQuitBtnClick()
    {
        /*
            #if
            #else
            #endif

            이는 전처리기 지시문으로,
            c# 의 일반적인 조건문이 런타임에 실행되는 것과 달리,
            전처리기 지시문은 컴파일 타임에 실행됨.

            굳이 컴파일 타임에 실행하는 이유는,
            실행환경 차이로 인한 기능 지원여부가 다르기 때문에,
            각 실행환경에 맞는 코드를 런타임에 실행하도록
            컴파일 타임에 어떤 코드를 사용할 지 미리 검사한 것.

            이를 통해 불필요한 코드 실행을 최소화하여 메모리 누수 등을 방지할 수 있음.
         */
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터일 경우, 플레이 모드 종료
#else
        Application.Quit(); // 일반적인 빌드된 애플리케이션 실행 종료
#endif
    }

    // 모달 Cancel 버튼 클릭 콜백
    public void OnModalCancelBtnClick()
    {
        HideAllPanels(); // 기존 패널 모두 닫기

        quitGameUI.SetActive(false);
    }

    // 기존 패널들을 모두 비활성화
    private void HideAllPanels()
    {
        characterPreview.SetActive(false);
        selectCharacterPanel.SetActive(false);
        settingsPanel.SetActive(false);
        lobbyBlur.SetActive(false); // 기존에 열려있던 Blur 필터도 비활성화
    }
}

