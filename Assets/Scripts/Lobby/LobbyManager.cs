using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 유니티용 포톤 컴포넌트
using Photon.Realtime; // 포톤 서비스 관련 라이브라리

public class LobbyManager : MonoBehaviourPunCallbacks
{
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

    // 룸 접속 시도
    private void Connect()
    {

    }
}
