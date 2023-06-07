using System;
using System.Collections.Generic;
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
    public TextMeshProUGUI magAmmoText; // 현재 탄창 내 탄알 수 텍스트
    public TextMeshProUGUI totalAmmoText; // 전체 탄알 수 텍스트
    public TextMeshProUGUI playerHPText; // 플레이어 체력 텍스트
    public TextMeshProUGUI killsScoreText; // 킬 수 표시 텍스트
    public TextMeshProUGUI timeText; // 시간 표시 텍스트

    [Header("Slider Bar UI")]
    public Slider playerHealthSlider; // 플레이어 체력 슬라이더
    public Slider bossHealthSlider; // 보스몬스터 체력 슬라이더
    public Slider reloadSlider; // 재장전 대기 슬라이더

    [Header("UI Object Reference")]
    public GameObject bossHealthUI; // 보스몬스터 공격 시 활성화할 체력 UI

    // GunAmmoText 게임 오브젝트 레퍼런스를 담기 위해, 해당 데이터를 구조체 형태로 선언
    // 구조체 타입을 사용한 멤버변수를 인스펙터 창에 표시하려면, 해당 구조체를 '직렬화' 할 수 있어야 함. -> [Serializable] 선언
    // 직렬화 : 객체나 데이터를 바이트로 변환하여 저장 및 전송 가능한 형식으로 변환하는 과정.
    [Serializable]
    public struct GunAmmoUI
    {
        public string gunName;
        public TextMeshProUGUI gunAmmoText;
    }

    [Header("Gun Ammo UI")]
    public List<GunAmmoUI> gunAmmoUIList; // 총기 종류별 탄알 수 텍스트 리스트

    private Dictionary<string, TextMeshProUGUI> gunAmmoUIDictionary = new Dictionary<string, TextMeshProUGUI>();

    [Serializable]
    public struct GunButtonBox
    {
        public GameObject enableBox;
        public GameObject disableBox;
    }

    [Serializable]
    public struct GunButtonUI
    {
        public string gunName;
        public GunButtonBox gunButtonBox;
    }

    [Header("Gun Buttons")]
    public List<GunButtonUI> gunButtonUIList; // 총기 종류별 버튼 리스트

    private Dictionary<string, GunButtonBox> gunButtonUIDictionary = new Dictionary<string, GunButtonBox>();

    private void Start()
    {
        bossHealthUI.SetActive(false); // 인스펙터 창에서 비활성화 체크 후, 코드에서도 확실하게 비활성화.

        // 인스펙터 창에서 할당받은 탄알 수 텍스트 리스트를 변환해서 딕셔너리 타입 멤버변수 초기화
        foreach (GunAmmoUI gunAmmoUI in gunAmmoUIList)
        {
            if (!gunAmmoUIDictionary.ContainsKey(gunAmmoUI.gunName))
            {
                gunAmmoUIDictionary.Add(gunAmmoUI.gunName, gunAmmoUI.gunAmmoText); // 해당 gunName 이 딕셔너리에 존재하지 않으면 새롭게 추가
            }
        }

        // 인스펙터 창에서 할당받은 총기 종류별 버튼 리스트를 변환해서 딕셔너리 타입 멤버변수 초기화
        foreach (GunButtonUI gunButtonUI in gunButtonUIList)
        {
            if (!gunButtonUIDictionary.ContainsKey(gunButtonUI.gunName))
            {
                gunButtonUIDictionary.Add(gunButtonUI.gunName, gunButtonUI.gunButtonBox); // 해당 gunName 이 딕셔너리에 존재하지 않으면 새롭게 추가
            }
        }
    }

    // 킬 수 UI 업데이트
    public void UpdateKillsScoreText(int kills)
    {
        killsScoreText.text = kills.ToString();
    }

    // 플레이어 체력 UI 업데이트
    public void UpdatePlayerHealth(float currentHealth, float startingHealth)
    {
        playerHPText.text = currentHealth + " / " + startingHealth;
        playerHealthSlider.value = currentHealth / startingHealth;
    }

    // 보스몬스터 체력 UI 업데이트
    public void UpdateBossHealth(float currentHealth, float startingHealth)
    {
        bossHealthSlider.value = currentHealth / startingHealth;
    }

    // 보스몬스터 첫 공격 시, 체력 UI 보여주기
    public void ShowBossHealth()
    {
        bossHealthUI.SetActive(true);
    }

    // 보스몬스터 사망 시, 체력 UI 숨기기
    public void HideBossHealth()
    {
        bossHealthUI.SetActive(false);
    }

    // 시간 UI 업데이트
    public void UpdateTimeText(int min, int sec)
    {
        timeText.text = min + ":" + sec.ToString("00"); // 초 단위 시간값은 두 자릿수로 표시
    }

    // 현재 탄창 내 탄알 UI 업데이트
    public void UpdateMagAmmoText(int magAmmo, int magCapacity)
    {
        magAmmoText.text = magAmmo + " / " + magCapacity;
    }

    // 총기 종류별 탄알 UI 업데이트
    public void UpdateGunAmmoText(string gunName, int ammoRemain)
    {
        if (gunAmmoUIDictionary.TryGetValue(gunName, out TextMeshProUGUI gunAmmoText))
        {
            gunAmmoText.text = ammoRemain.ToString(); // 총기 이름에 해당하는 gunAmmoText 가 존재할 경우, 해당 총의 탄알 수 업데이트
        }
    }

    // 전체 총기 탄알 UI 업데이트
    // 메서드 단일 책임 원칙 및 의존성 주입 원칙에 따라,
    // 전체 탄알 업데이트 메서드는 UpdateGunAmmoText 내부 자동호출 보다는, 호출부에서 각각 명시적으로 호출하도록 함.
    public void UpdateTotalAmmoText()
    {
        int totalAmmo = 0;

        foreach (TextMeshProUGUI gunAmmoText in gunAmmoUIDictionary.Values)
        {
            int ammoCount;
            if (int.TryParse(gunAmmoText.text, out ammoCount))
            {
                // 문자열 > 정수 변환 시도 메서드 int.TryParse() 로 정수형 변환 가능 여부 판별.
                // 변환 가능 시, 변환된 정수값을 전체 탄알 수에 더함.
                totalAmmo += ammoCount; 
            }
        }

        totalAmmoText.text = totalAmmo.ToString();
    }

    // 총기 버튼 활성화 UI 업데이트
    public void UpdateGunButtonBox(string gunName)
    {
        foreach (KeyValuePair<string, GunButtonBox> gunButtonUI in gunButtonUIDictionary)
        {
            string currentGun = gunButtonUI.Key;
            GunButtonBox gunButtonBox = gunButtonUI.Value;

            bool isActive = currentGun == gunName;
            gunButtonBox.enableBox.SetActive(isActive);
            gunButtonBox.disableBox.SetActive(!isActive);
        }
    }

    // 재장전 대기 시간 UI 업데이트
    public void UpdateReloadSlider(float progress)
    {
        reloadSlider.value = progress; // Slider의 value를 재장전 진행도에 맞게 업데이트 (0 ~ 1 사이)
    }
}
