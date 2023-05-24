using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 점수, 게임 승패, 게임오버 여부 관리 클래스
public class GameManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance; // 싱글톤 인스턴스를 반환
        }
    }

    private static GameManager m_instance; // 싱글톤 인스턴스가 할당된 정적 멤버변수

    public bool isGameover { get; private set; } // 게임오버 상태 자동구현 프로퍼티
    public bool isGameWon { get; private set; } // 게임 승패 자동구현 프로퍼티

    // 게임 승리 처리
    public void SetGameWon()
    {
        isGameWon = true;
    }

    // 게임 패배 처리
    public void SetGameLost()
    {
        isGameWon = false;
    }


    // 게임오버 처리
    public void EndGame()
    {
        isGameover = true;
        // TODO : UI 업데이트 처리
    }

    private void Awake()
    {
        if (instance != this)
        {
            // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면 자신을 파괴함.
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        BossMonster bossMonster = FindObjectOfType<BossMonster>();

        playerHealth.onDeath += SetGameLost; // PlayerHealth 사망 시 게임패배 처리를 이벤트 콜백으로 등록
        playerHealth.onDeath += EndGame; // PlayerHealth 사망 시 게임오버 처리를 이벤트 콜백으로 등록

        bossMonster.onDeath += SetGameWon; // BossMonster 사망 시 게임승리 처리를 이벤트 콜백으로 등록
        bossMonster.onDeath += EndGame; // BossMonster 사망 시 게임오버 처리를 이벤트 콜백으로 등록
    }
}
