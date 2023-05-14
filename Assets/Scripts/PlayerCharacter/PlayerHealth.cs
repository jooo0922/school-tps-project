using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 체력 및 사망상태 관리 모듈 > LivingEntity 상속
public class PlayerHealth : LivingEntity
{
    [Header("Effects")]
    public AudioClip deathClip; // 사망 오디오
    public AudioClip hitClip; // 피격 오디오
    public AudioClip pickupClip; // 아이템 줍기 오디오

    private AudioSource playerAudioPlayer; // 오디오 소스 컴포넌트
    private Animator playerAnimator; // 애니메이터 컴포넌트
    private PlayerMovement playerMovement; // PlayerMovement 컴포넌트
    private PlayerShooter playerShooter; // PlayerShooter 컴포넌트
    private PlayerGunManager playerGunManager; // PlayerGunManager 컴포넌트
    private PlayerInput playerInput; // PlayerInput 컴포넌트
    private TPSCameraController TPSCameraController; // TPSCameraController 컴포넌트

    // 필요한 컴포넌트들 가져오기
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerGunManager = GetComponent<PlayerGunManager>();
        playerInput = GetComponent<PlayerInput>();
        TPSCameraController = GetComponent<TPSCameraController>();
    }

    // LivingEntity 상태 초기화 처리 override
    protected override void OnEnable()
    {
        base.OnEnable();

        playerMovement.enabled = true; // PlayerMovement 컴포넌트 활성화
    }

    // 체력 회복 처리 override
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);
    }

    // 공격받았을 때 처리 override
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip); // 플레이어가 사망하지 않은 상태에서만 피격 오디오 재생
            playerAnimator.SetTrigger("Hit");
        }

        base.OnDamage(damage, hitPoint, hitNormal); // 대미지 적용 처리
    }

    // 사망 처리 override
    public override void Die()
    {
        base.Die(); // 사망 처리 적용

        playerAudioPlayer.PlayOneShot(deathClip); // 플레이어 사망 오디오 재생
        playerAnimator.SetTrigger("Die"); // 사망 애니메이션 재생

        // 플레이어 조작 관련 컴포넌트들 전부 비활성화
        playerMovement.enabled = false;
        playerShooter.enabled = false;
        playerGunManager.enabled = false;
        TPSCameraController.enabled = false;
    }

    // 아이템과 충돌 처리 이벤트 메서드
    private void OnTriggerStay(Collider other)
    {
        // 플레이어가 살아있고, pickup 버튼을 누른 상태에서만 아이템 사용 처리
        if (!dead && playerInput.pickup)
        {
            IItem item = other.GetComponent<IItem>(); // 충돌한 게임 오브젝트에서 IItem 컴포넌트를 찾아보기

            if (item != null)
            {
                item.Use(gameObject); // 플레이어 게임 오브젝트 자신을 인자로 넘겨주면서 아이템 사용 메서드 실행
                playerAudioPlayer.PlayOneShot(pickupClip); // 아이템 줍기 효과음 적용
            }
        }
    }
}
