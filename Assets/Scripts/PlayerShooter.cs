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
