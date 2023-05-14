using UnityEngine;
using UnityEngine.AI; // 내비메시 코드 사용 목적

// 플레이어 근처에 주기적으로 랜덤 아이템 생성
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // 생성할 아이템 프리팹 배열
    public Transform playerTransform; // 플레이어의 트랜스폼 컴포넌트

    public float maxDistance = 5f; // 플레이어와 아이템 간 최대 배치 반경
    public float timeBetSpawnMax = 7f; // 최대 생성 시간 간격
    public float timeBetSpawnMin = 2f; // 최소 생성 시간 간격

    private float timeBetSpawn; // 생성 시간 간격
    private float lastSpawnTime; // 마지막 생성 시점

    private void Start()
    {
        // 생성 시간 간격과 마지막 생성 시점 초기화
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    private void Update()
    {
        // 현재 시점이 마지막 생성 시점으로부터 랜덤 생성 간격보다 지났고, 플레이어의 트랜스폼 컴포넌트가 존재하면 아이템 생성 처리
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            lastSpawnTime = Time.time; // 마지막 생성 시점 갱신
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax); // 랜덤 생성 간격 변경
            Spawn(); // 실제 아이템 생성 처리    
        }
    }

    private void Spawn()
    {
        // 플레이어 근처에 내비메시 위의 랜덤 좌표 구하기
        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
        spawnPosition += Vector3.up * 0.5f; // 바닥에서 0.5만큼 위로 올리기

        // 아이템 중 하나를 무작위로 골라 랜덤 위치에 생성
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity);

        Destroy(item, 5f); // 생성된 아이템 인스턴스가 사용되지 않는다면 5초 후 파괴시킴
    }

    // 내비메시 위의 랜덤한 위치를 반환함
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center를 중심으로 반지름이 maxDistance인 구 안에서의 랜덤한 위치 하나를 저장
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit; // 내비메시 샘플링 결과 정보 저장 변수

        // distance 반경 내에서 randomPos 에 가장 가까운 내비메시 위의 한 점을 찾음
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        return hit.position; // 찾은 점 반환
    }
}
