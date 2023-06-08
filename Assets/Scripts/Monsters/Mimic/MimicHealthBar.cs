using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicHealthBar : MonoBehaviour
{
    private Transform mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.transform; // MainCamera 태그를 갖고 있는 카메라 검색 (TPSCamera 에 할당해놓음)
    }

    private void LateUpdate()
    {
        Vector3 toMainCameraDir = -mainCamera.forward; // TPSCamera를 향하는 방향벡터 (카메라 앞쪽 방향벡터의 반대방향)
        transform.LookAt(transform.position + toMainCameraDir); // 현재 캔버스 위치에서 TPSCamera 방향으로 1만큼 더해준 지점을 바라보도록 함. -> 항상 TPSCamera 를 바라봄
    }
}
