using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자 입력에 따라 플레이어 캐릭터를 움직이고 애니메이션 클립을 재생하는 모듈
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody; // 플레이어 캐릭터의 자식 오브젝트로 저장된 실제 캐릭터 모델 게임 오브젝트
    [SerializeField]
    private Transform cameraArm; // TPS 카메라의 방향벡터를 참조할 카메라 암 오브젝트

    public float moveSpeed = 3f; // 플레이어 이동 속력

    private PlayerInput playerInput; // 플레이어 입력 상태 관리 모듈
    private Rigidbody playerRigidBody; // 플레이어 캐릭터 리지드바디 컴포넌트 -> 플레이어 캐릭터의 물리 처리를 고려한 이동 구현
    private Animator playerAnimator; // 플레이어 캐릭터 > VRoid 게임 오브젝트의 애니메이터 컴포넌트

    // 플레이어 캐릭터 게임 오브젝트 활성화 시점에 필요한 컴포넌트들을 모두 가져옴.
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<Animator>(); // 애니메이터 컴포넌트는 플레이어 캐릭터의 자식 게임오브젝트에 추가되어 있으니 거기서 가져온 것!
    }

    // 리지드바디 컴포넌트로 플레이어 캐릭터 이동 구현
    // -> 따라서, 유니티 물리엔진 갱신 주기에 맞춰 실행되는 유니티 이벤트 메서드 FixedUpdate() 사용 
    private void FixedUpdate()
    {
        // 플레이어 캐릭터 이동 및 애니메이션 실행
        Move();
    }

    // 사용자 입력 상태에 따라 플레이어 캐릭터 이동 및 애니메이션 실행 구현
    private void Move()
    {
        // 좌우이동 입력 상태값을 x값, 앞뒤이동 입력 상태값을 y값에 넣어 Vector2 타입으로 이동 입력 상태값 저장
        Vector2 moveInput = new Vector2(playerInput.leftRightMove, playerInput.frontBackMove);
        playerAnimator.SetFloat("Move X", moveInput.x); // 2D BlendTree 의 x축 전이 파라미터 값을 좌우이동 입력값으로 업데이트
        playerAnimator.SetFloat("Move Z", moveInput.y); // 2D BlendTree 의 y축 전이 파라미터 값을 앞뒤이동 입력값으로 업데이트

        // cameraArm 의 오브젝트공간 좌표 기준 방향벡터는 TPSCamera 의 회전에 따라 달라질 것임.
        // 수평화(y값 0으로 할당)로 인해 달라진 방향벡터의 길이를 다시 1로 맞추기 위해 normalized 한 것.
        Vector3 cameraForwardDirection = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized; // 카메라 암의 오브젝트공간 좌표 기준 y값이 수평화된 앞쪽 방향벡터
        Vector3 cameraRightDirection = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized; // 카메라 암의 오브젝트공간 좌표 기준 y값이 수평화된 오른쪽 방향벡터

        // 사용자의 이동입력 상태값이 저장된 Vector2 값의 벡터 크기(길이)가 0이 아니다?
        // > x값과 y값 중 최소 하나는 0이 아니다!
        // > 사용자가 Horizontal 이든 Vertical 이든 AxisInput 을 입력했다!
        // > 따라서, 사용자 이동입력에 대한 플레이어 캐릭터 이동 처리를 해줘야 한다!
        // 참고로, Vector2.magnitude -> 벡터의 크기(길이)값을 반환함.
        if (moveInput.magnitude != 0)
        {
            // 최종 이동값(방향 + 거리가 포함된 최종 Vector3 값) 계산
            Vector3 moveDistance = cameraForwardDirection * moveInput.y + cameraRightDirection * moveInput.x; // 우선 좌우축, 앞뒤축 각각의 이동방향 및 이동여부가 포함된 Vector3 계산
            moveDistance *= moveSpeed; // 그 다음, 이동속도 곱함.
            moveDistance *= Time.fixedDeltaTime; // 마지막으로, 유니티 물리엔진의 갱신주기 만큼의 시간간격을 곱함 -> 1회의 FixedUpdate() 호출 시점 기준 이동거리로 변환한 것!

            // 리지드바디 컴포넌트 > MovePosition() 메서드로 플레이어 캐릭터 게임 오브젝트 위치 이동
            // 트랜스폼 컴포넌트로도 위치를 변경할 수는 있으나, 충돌 등의 물리 처리를 무시하고 벽을 뚫고 지나가는 현상을 방지하기 위해 리지드바디 컴포넌트로 이동시켜준 것임!
            playerRigidBody.MovePosition(playerRigidBody.position + moveDistance);
        }

        // 실제 캐릭터 게임오브젝트가 바라보는 방향을 카메라 암의 수평화된 앞쪽 방향벡터와 일치시킴 -> TPS 카메라가 움직이면 그에 따라 플레이어 캐릭터도 방향전환 (-> 직관적 컨트롤)
        characterBody.forward = cameraForwardDirection;
    }
}
