using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobSpawner : MonoBehaviour
{
    public GameObject blobPrefab; // GPU 인스턴싱에 사용할 blob 프리팹

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

    private Mesh blobMesh; // Blob 메시
    private Material blobMaterial; // Blob 머티리얼

    private Matrix4x4[] matrices; // 인스턴스 변환 행렬 배열

    // GPU 인스턴싱을 위한 멤버변수 초기화 및 각 인스턴스 변환 계산
    private void Setup()
    {
        blobMesh = blobPrefab.GetComponentInChildren<MeshFilter>().sharedMesh; // Blob 메쉬 가져오기
        blobMaterial = blobPrefab.GetComponentInChildren<Renderer>().sharedMaterial; // Blob 머티리얼 가져오기

        matrices = new Matrix4x4[instanceCount]; // 변환행렬 배열 사이즈를 인스턴스 개수만큼 초기화

        for (int i = 0; i < instanceCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            Quaternion rotation = Quaternion.identity;
            Vector3 scale = Vector3.one * Random.Range(minScale, maxScale);

            matrices[i] = Matrix4x4.TRS(position, rotation, scale);
        }
    }

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        Graphics.DrawMeshInstanced(blobMesh, 0, blobMaterial, matrices); // 매 프레임마다 GPU 인스턴스 렌더링
    }
}
