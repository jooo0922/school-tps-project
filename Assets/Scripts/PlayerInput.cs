using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 캐릭터 및 TPS 카메라 조작을 위한 입력 상태 관리 모듈
// 감지된 입력값을 다른 컴포넌트가 접근 및 읽기 가능
public class PlayerInput : MonoBehaviour
{
    public string frontBackMoveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string leftRightMoveAxisName = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string horizontalCameraRotateAxisName = "Mouse X"; // 수평방향 카메라 회전을 위한 입력축 이름
    public string verticalCameraRotateAxisName = "Mouse Y"; // 수직방향 카메라 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1"; // 밣사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름
    public string jumpButtonName = "Jump"; // 점프를 위한 입력 버튼 이름

    // 입력 상태를 자동구현 프로퍼티 메서드로 관리 -> 외부에서는 '읽기'만 가능
    public float frontBackMove { get; private set; } // 감지된 앞뒤 움직임 입력값
    public float leftRightMove { get; private set; } // 감지된 좌우 움직임 입력값 
    public float horizontalCameraRotate { get; private set; } // 감지된 수평방향 카메라 회전 입력값 
    public float verticalCameraRotate { get; private set; } // 감지된 수직방향 카메라 회전 입력값 
    public bool fire { get; private set; } // 감지된 발사 입력값 
    public bool reload { get; private set; } // 감지된 재장전 입력값
    public bool jump { get; private set; } // 감지된 점프 입력값


    // 매 프레임마다 입력 상태 업데이트
    void Update()
    {
        // 앞뒤 움직임 입력 상태 업데이트
        frontBackMove = Input.GetAxis(frontBackMoveAxisName);
        // 좌우 움직임 입력 상태 업데이트
        leftRightMove = Input.GetAxis(leftRightMoveAxisName);
        // 수평방향 카메라 회전 입력 상태 업데이트
        horizontalCameraRotate = Input.GetAxis(horizontalCameraRotateAxisName);
        // 수직방향 카메라 회전 입력 상태 업데이트
        verticalCameraRotate = Input.GetAxis(verticalCameraRotateAxisName);
        // 발사 입력 상태 업데이트
        fire = Input.GetButton(fireButtonName);
        // 재장전 입력 상태 업데이트
        reload = Input.GetButton(reloadButtonName);
        // 점프 입력 상태 업데이트
        jump = Input.GetButton(jumpButtonName);
    }
}
