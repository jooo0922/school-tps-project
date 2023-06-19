using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 코드
using TMPro; // TextMeshPro (TMP) 관련 코드
using UI.Parameters; // UI 로직 파라미터 관련

public class ResultUI : MonoBehaviour
{
    [Header("Game Result UI")]
    public GameObject successTitleUI; // 미션성공 타이틀
    public GameObject failTitleUI; // 미션실패 타이틀
    public TextMeshProUGUI resultPlayTimeText; // 게임결과 플레이 시간 텍스트
    public TextMeshProUGUI resultKillsText; // 게임결과 킬 수 텍스트
    public TextMeshProUGUI resultTotalScoreText; // 게임결과 최종 점수 텍스트

    [Header("Game Result Canvas Groups")]
    public CanvasGroup resultTitleCanvasGroup; // 게임 타이틀 캔버스 그룹
    public CanvasGroup resultTableCanvasGroup; // 게임 결과 테이블 캔버스 그룹
    public CanvasGroup resultButtonsCanvasGroup; // 게임 결과 버튼 캔버스 그룹

    [Header("Game Result Background")]
    public Image resultBackgroundImage; // 게임 결과 배경 이미지 컴포넌트
    public float resultBackgroundStartRadius; // 게임 결과 배경 초기 blur 반경
    public float resultBackgroundTargetRadius; // 게임 결과 배경 목표 blur 반경
    public Color resultBackgroundStartColor; // 게임 결과 배경 초기 색상
    public Color resultBackgroundTargetColor; // 게임 결과 배경 목표 색상

    private bool shouldFadeInResultTitle = false; // 게임 결과 타이틀 fade in 여부
    private bool shouldFadeInResultTable = false; // 게임 결과 테이블 fade in 여부
    private bool shouldFadeInResultButtons = false; // 게임 결과 버튼 fade in 여부
    private bool shouldFadeInResultBackground = false; // 게임 결과 배경 fade in 여부

    private Material resultBackgroundMaterial; // 게임 결과 배경 머티리얼
    private float resultBackgroundLerpValue = 0f; // 게임 결과 배경 보간값

    // 필요한 컴포넌트들 가져오기
    private void Awake()
    {
        resultBackgroundMaterial = resultBackgroundImage.material;

        // 게임 결과 배경 프로퍼티 초기값 복구
        resultBackgroundMaterial.SetFloat("_Radius", 0f);
        resultBackgroundMaterial.SetColor("_Color", new Color(1f, 1f, 1f, 1f));

        // 델리게이트에 
        UIManager.instance.OnGameResultUpdate += SetGameResultUI;
    }

    // 페이드 인 처리
    private void Update()
    {
        FadeInResultBackground();
        FadeInResultTitle();
        FadeInResultTable();
        FadeInResultButtons();
    }

    // Game Result UI 업데이트
    private void SetGameResultUI(GameResult gameResult)
    {
        // 게임 승패에 따라 활성화할 타이틀 지정
        if (gameResult.isGameWon)
        {
            successTitleUI.SetActive(true);
            failTitleUI.SetActive(false);
        }
        else
        {
            successTitleUI.SetActive(false);
            failTitleUI.SetActive(true);
        }

        // 기존의 timeText와 killsScoreText의 텍스트 값을 가져와서 결과 UI에 표시
        resultPlayTimeText.text = gameResult.timeText;
        resultKillsText.text = gameResult.killsText;
        resultTotalScoreText.text = gameResult.totalScore.ToString(); // 최종 점수를 문자열로 변환하여 업데이트

        StartCoroutine(ShowGameResultUICoroutine()); // 게임결과 UI 표시 코루틴 메서드 실행
    }

    // 게임결과 UI 를 실제로 보여주는 코루틴 메서드
    private IEnumerator ShowGameResultUICoroutine()
    {
        yield return new WaitForSeconds(1f);

        // 대기처리 후 배경화면 및 게임결과 타이틀 페이드 인
        shouldFadeInResultBackground = true;
        shouldFadeInResultTitle = true;

        yield return new WaitForSeconds(1.8f);

        // 대기처리 후 게임결과 테이블과 버튼 페이드 인
        shouldFadeInResultTable = true;
        shouldFadeInResultButtons = true;
    }

    // 게임결과 배경 페이드 인
    private void FadeInResultBackground()
    {
        if (shouldFadeInResultBackground)
        {
            if (resultBackgroundLerpValue < 1)
            {
                resultBackgroundLerpValue += Time.deltaTime;

                float radius = Mathf.Lerp(resultBackgroundStartRadius, resultBackgroundTargetRadius, resultBackgroundLerpValue);
                resultBackgroundMaterial.SetFloat("_Radius", radius); // 게임결과 배경 blur 반경 보간

                Color color = Color.Lerp(resultBackgroundStartColor, resultBackgroundTargetColor, resultBackgroundLerpValue);
                resultBackgroundMaterial.SetColor("_Color", color); // 게임결과 배경 색상 보간
            }
            else
            {
                shouldFadeInResultBackground = false;
            }
        }
    }

    // 게임결과 타이틀 페이드 인
    private void FadeInResultTitle()
    {
        if (shouldFadeInResultTitle)
        {
            if (resultTitleCanvasGroup.alpha < 1)
            {
                resultTitleCanvasGroup.alpha += Time.deltaTime;
            }
            else
            {
                shouldFadeInResultTitle = false;
            }
        }
    }

    // 게임결과 테이블 페이드 인
    private void FadeInResultTable()
    {
        if (shouldFadeInResultTable)
        {
            if (resultTableCanvasGroup.alpha < 1)
            {
                resultTableCanvasGroup.alpha += Time.deltaTime;
            }
            else
            {
                shouldFadeInResultTable = false;
            }
        }
    }

    // 게임결과 버튼 페이드 인
    private void FadeInResultButtons()
    {
        if (shouldFadeInResultButtons)
        {
            if (resultButtonsCanvasGroup.alpha < 1)
            {
                resultButtonsCanvasGroup.alpha += Time.deltaTime;
            }
            else
            {
                shouldFadeInResultButtons = false;
            }
        }
    }
}
