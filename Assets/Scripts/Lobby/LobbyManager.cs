using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // 유니티용 포톤 컴포넌트
using Photon.Realtime; // 포톤 서비스 관련 라이브라리

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    public GameObject characterPreview; // 캐릭터 프리뷰 패널
    public GameObject selectCharacterPanel; // 캐릭터 선책 창 패널
    public GameObject settingsPanel; // 세팅 창 패널
    public GameObject lobbyBlur; // blur 필터

    [Header("Modal")]
    public GameObject quitGameUI; // 게임 종료 모달 UI

    private string gameVersion; // 게임 버전

    private void Start()
    {
        
    }

    // 마스터 서버 접속 성공 시 자동 실행
    public override void OnConnectedToMaster()
    {
        
    }

    // 마스터 서버 접속 실패 시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        
    }

    // 랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {

    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {

    }

    // Play Game 버튼 클릭 콜백
    public void OnPlayGameBtnClick()
    {
        
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

    public void OnModalQuitBtnClick()
    {
        
    }

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

    // 룸 접속 시도
    private void Connect()
    {

    }
}
