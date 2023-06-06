using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 코드
using UnityEngine.UI; // UI 관련 코드
using TMPro; // TextMeshPro (TMP) 관련 코드

// UI 요소 즉시 접근 및 변경 관리 모듈
public class UIManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
                // 씬에서 UIManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤 인스턴스가 할당된 정적 멤버변수

    [Header("Text UI")]
    public TextMeshProUGUI ammoText; // 현재 탄창 내 탄알 수 텍스트
    public TextMeshProUGUI totalAmmoText; // 전체 탄알 수 텍스트
    public TextMeshProUGUI rifleAmmoText; // 소총 탄알 수 텍스트
    public TextMeshProUGUI pistolAmmoText; // 권총 탄알 수 텍스트
    public TextMeshProUGUI playerHPText; // 플레이어 체력 텍스트
    public TextMeshProUGUI killsScoreText; // 킬 수 표시 텍스트
    public TextMeshProUGUI timeText; // 시간 표시 텍스트

    [Header("Slider Bar UI")]
    public Slider playerHealthSlider; // 플레이어 체력 슬라이더
    public Slider bossHealthSlider; // 보스몬스터 체력 슬라이더
    public Slider reloadSlider; // 재장전 대기 슬라이더

    [Header("UI Object Reference")]
    public GameObject bossHealthUI; // 보스몬스터 공격 시 활성화할 체력 UI

    // 킬 수 UI 업데이트
    public void UpdateKillsScoreText(int kills)
    {
        killsScoreText.text = kills.ToString();
    }
}
