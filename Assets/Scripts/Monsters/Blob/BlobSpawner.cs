using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobSpawner : MonoBehaviour
{
    public GameObject blobPrefab; // GPU 인스턴싱에 사용할 blob 프리팹
    public Transform Sky; // 인스턴스를 추가할 부모 게임 오브젝트

    [Range(5, 50)]
    public int instanceCount = 30; // GPU 인스턴스 개수

    [Header("Instance Position")]
    public float minX = -50f;
    public float maxX = 50f;
    public float minY = 30f;
    public float maxY = 70f;
    public float minZ = -40f;
    public float maxZ = 40f;

    [Header("Instance Scale")]
    public float minScale = 0.1f;
    public float maxScale = 2.5f;

    // GPU 인스턴싱을 위한 멤버변수 초기화 및 각 인스턴스 변환 계산
    private void Setup()
    {
        for (int i = 0; i < instanceCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            Quaternion rotation = Quaternion.identity;
            Vector3 scale = Vector3.one * Random.Range(minScale, maxScale);

            GameObject blob = Instantiate(blobPrefab, position, rotation);
            blob.transform.localScale = scale;

            blob.transform.SetParent(Sky); // Sky 게임 오브젝트에 추가
        }
    }

    private void Start()
    {
        Setup();
    }
}
