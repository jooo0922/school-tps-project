using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicSpawner : MonoBehaviour
{
    [Tooltip("Original Mimic Prefab")]
    public MimicObject mimicPrefab; // 생성할 Mimic 원본 프리팹

    [Tooltip("MimicData Asset")]
    public MimicData[] mimicDatas; // 사용할 Mimic 데이터 에셋

    [Tooltip("Transform of Mimic Spawn Points")]
    public Transform[] spawnPoints; // Mimic 생성 위치

    private List<MimicObject> mimics = new List<MimicObject>(); // 생성된 Mimic 인스턴스를 담아둘 리스트
    private int wave = 5; // 현재 mimic 생성 웨이브

    // 업데이트 루프
    private void Update()
    {
        if (mimics.Count <= 0)
        {
            // 현재 Mimic 인스턴스가 존재하지 않을 경우, 다음 스폰 실행
            SpawnWave();
        }

        UpdateUI(); // UI 정보 업데이트
    }

    // Mimic 관련 정보를 UI로 표시
    private void UpdateUI()
    {

    }

    // 현재 웨이브에 맞춰서 Mimic 생성 트리거
    private void SpawnWave()
    {
        wave++; // Mimic 생성 웨이브 증가
        int spawnCount = Mathf.RoundToInt(wave * 1.5f); // 현재 웨이브 * 1.5 를 곱한 값을 반올림하여 Mimic 생성 수 결정

        // spawnCount 수만큼 Mimic 생성
        for (int i = 0; i < spawnCount; i++)
        {
            CreateMimic();
        }
    }

    // Mimic 인스턴스 생성 및 저장
    private void CreateMimic()
    {
        MimicData mimicData = mimicDatas[Random.Range(0, mimicDatas.Length)]; // 사용할 MimicData 랜덤 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Mimic 생성 위치 랜덤 결정

        MimicObject mimic = Instantiate(mimicPrefab, spawnPoint.position, spawnPoint.rotation); // Mimic 프리팹으로부터 인스턴스 생성
        MimicAI mimicAI = mimic.gameObject.GetComponentInChildren<MimicAI>(); // MimicAI 컴포넌트 가져오기
        mimicAI.Setup(mimicData);

        mimics.Add(mimic); // 생성된 Mimic 인스턴스 리스트에 저장

        // Mimic 사망 시 콜백함수들을 onDeath 델리게이트에 등록
        mimicAI.onDeath += () => mimics.Remove(mimic); // 사망한 Mimic 을 리스트에서 제거
        mimicAI.onDeath += () => Destroy(mimic.gameObject, 3f); // 사망한 Mimic 인스턴스 3초 뒤 파괴
    }
}
