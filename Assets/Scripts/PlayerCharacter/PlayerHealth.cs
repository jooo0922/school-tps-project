using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 체력 및 사망상태 관리 모듈 > LivingEntity 상속
public class PlayerHealth : LivingEntity
{
    [Header("Effects")]
    public AudioClip deathClip; // 사망 오디오
    public AudioClip hitClip; // 피격 오디오

    private AudioSource playerAudioPlayer; // 오디오 소스 컴포넌트
    private Animator playerAnimator; // 애니메이터 컴포넌트
    private PlayerMovement playerMovement; // PlayerMovement 컴포넌트

    // 필요한 컴포넌트들 가져오기
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
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
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리 override
    public override void Die()
    {
        base.Die();
    }

    // 아이템과 충돌 처리 이벤트 메서드
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
