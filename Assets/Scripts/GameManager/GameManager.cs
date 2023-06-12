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
    public int mimicKillCount { get; private set; } // Mimic 킬 수 자동구현 프로퍼티

    private float elapsedTime = 0f; // 경과시간 (초 단위)
    private int prevSec = -1; // 이전 초 단위 시간값

    private float playTimeWeight = 0.1f; // 플레이 시간 가중치
    private float killsWeight = 0.5f; // 킬 수 가중치
    private float successBonus = 100.0f; // 승리 보너스 점수

    // 몬스터 킬 점수 추가
    public void IncreaseMimicKillCount()
    {
        mimicKillCount++;
        UIManager.instance.UpdateKillsScoreText(mimicKillCount); // 킬 수 UI 업데이트
    }

    // 게임오버 처리
    public void EndGame(bool won)
    {
        isGameover = true;
        isGameWon = won; // 게임 승패 여부 상태값 업데이트

        int totalScore = CalculateTotalScore(isGameWon, mimicKillCount, elapsedTime); // 최종 점수 계산
        UIManager.instance.UpdateGameResultUI(isGameWon, totalScore); // Game Result UI 업데이트
    }

    // 최종점수 계산
    private int CalculateTotalScore(bool isWon, int kills, float playTime)
    {
        float score = kills * killsWeight - playTime * playTimeWeight; // 플레이 시간과 킬 수에 가중치를 곱해서 합산

        if (isWon)
        {
            score += successBonus; // 게임 승리 시, 승리 보너스 추가
        }

        int totalScore = Mathf.RoundToInt(Mathf.Max(0f, score)); // 최종 점수가 0보다 작아지지 않도록 최솟값을 설정한 후, 정수형으로 변환

        return totalScore;
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

        playerHealth.onDeath += () => EndGame(false); // PlayerHealth 사망 시 게임오버 처리를 이벤트 콜백으로 등록
        bossMonster.onDeath += () => EndGame(true); // BossMonster 사망 시 게임오버 처리를 이벤트 콜백으로 등록
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        elapsedTime += Time.deltaTime;

        int min = Mathf.FloorToInt(elapsedTime / 60f); // 분 단위 시간값 계산 (소수점 버림)
        int sec = Mathf.FloorToInt(elapsedTime % 60f); // 초 단위 시간값 계산 (소수점 버림)

        if (sec != prevSec)
        {
            UIManager.instance.UpdateTimeText(min, sec); // 시간 UI 업데이트
            prevSec = sec; // 이전 초 단위 시간값 갱신
        }
    }
}
