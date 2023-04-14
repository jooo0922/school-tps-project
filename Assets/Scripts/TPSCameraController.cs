using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    public Transform cameraArm; // TPS 카메라의 실제 회전을 적용할 카메라 암 오브젝트

    private PlayerInput playerInput; // 플레이어 입력 상태 관리 모듈

    void Start()
    {
        // 사용할 컴포넌트들 가져오기
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
    }

    // 마우스 이동에 따른 TPS 카메라 회전 처리 메서드
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(playerInput.horizontalCameraRotate, playerInput.verticalCameraRotate); // 마우스를 좌우로 움직인 수치와 위아래로 움직인 수치를 Vector2 타입 구조체로 묶어서 저장.
        Vector3 cameraAngle = cameraArm.rotation.eulerAngles; // 현재 카메라의 오일러 각을 저장.

        float rotateX = cameraAngle.x - mouseDelta.y; // 마우스 위아래 움직인 수치만큼 현재 카메라 암의 x축 회전값 계산 -> 이는 카메라 암의 수직방향 회전과 동일.

        // 카메라 암의 수직방향 회전 범위 제한
        if (rotateX < 180f)
        {
            // 위쪽 방향 회전 시(0 ~ 180도), 카메라 회전각을 0 ~ 70도 사이로 제한함.
            // clamp 최소값을 0도가 아닌 -1도로 해준 이유는, 0도로 제한 시, 0도 아래 영역, 즉 360 ~ 180도 회전영역 아래로 내려가는 회전이 막혀버려서 -1도까지 허용한 것임. 
            rotateX = Mathf.Clamp(rotateX, -1f, 70f);
        }
        else
        {
            // 아래쪽 방향 회전 시(360 ~ 180도), 카메라 회전각을 360 ~ 335도 시이로 제한함. (25도까지만 회전하도록 한 것.)
            // clamp 최대값을 360도가 아닌 361도로 해준 이유는, 위와 마찬가지로 360도 위 영역, 즉, 0 ~ 180도 회전영역 위로 올라가는 회전이 막히기 때문에 361도까지 허용한 것.
            rotateX = Mathf.Clamp(rotateX, 335f, 361f);
        }

        // 마우스 좌우로 움직인 수치만큼 현재 카메라 암의 y축 회전값 계산 -> 이는 카메라 암의 수평방향 회전과 동일.
        cameraArm.rotation = Quaternion.Euler(rotateX, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    }
}
