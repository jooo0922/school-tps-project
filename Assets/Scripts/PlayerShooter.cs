using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 주어진 Gun 게임 오브젝트 발사 및 재장전 관련 애니메이션 및 로직 수행
public class PlayerShooter : MonoBehaviour
{
    public Gun gun; // 사용할 총
    public Transform gunPivot; // 총 위치 기준점

    private PlayerInput playerInput; // 플레이어 입력 모듈
    private Animator playerAnimator; // 애니메이터 컴포넌트

    // 컴포넌트 가져오기
    public void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 슈터가 활성화될 때 총 게임 오브젝트도 같이 활성화
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        // 슈터가 비활성화될 때 총 게임 오브젝트도 같이 비활성화
        gun.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerInput.fire)
        {
            gun.Fire(); // 발사 입력 감지 시, 총 발사 트리거 메서드 실행
        }
        else if (playerInput.reload)
        {
            // 기본 Movement 상태에서는 Reload 트리거 파라미터를 실행해도 Reload 애니메이션이 적용되지 않음.
            // 왜냐하면, IsMounted 파라미터 및 상태값이 true가 되는 시점에 PlayerShooter 컴포넌트 자체가 활성화되도록 설정해놨기 때문! (PlayerMovement 참고)
            playerAnimator.SetTrigger("Reload");
        }

        UpdateUI();           
    }

    // 총 상태 관련 UI 갱신
    private void UpdateUI()
    {

    }

    // 애니메이터 IK 갱신 이벤트 메서드
    private void OnAnimatorIK(int layerIndex)
    {
        // 총 위치 기준점을 캐릭터 모델의 오른쪽 팔꿈치로 이동
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK 왼손의 위치와 회전 변경 시의 가중치 설정 -> 1.0 은 목표값으로 100% 변경시킴
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        // IK 왼손의 위치와 회전 목표값 설정
        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, gun.leftHandMount.rotation);

        // IK 오른손의 위치와 회전 변경 시의 가중치 설정 -> 1.0 은 목표값으로 100% 변경시킴
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        // IK 오른손의 위치와 회전 목표값 설정
        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, gun.rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, gun.rightHandMount.rotation);

    }
}
