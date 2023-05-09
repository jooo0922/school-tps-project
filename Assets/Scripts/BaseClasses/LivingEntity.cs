using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    [Header("Health Stats")]
    [SerializeField] private float startingHealth = 50f; // 시작 체력

    public float health { get; protected set; } // 현재 체력 자동구현 프로퍼티
    public bool dead { get; protected set; } // 사망 상태 자동구현 프로퍼티
    public event Action onDeath; // 사망 이벤트를 등록할 델리게이트 (event 타입을 붙여 외부에서 델리게이트를 실행하지 못하도록 막아둠 -> 디커플링)

    // 생명체 활성화 시 체력 및 사망상태 초기화 -> 가상 메서드로 선언 (자식 클래스에서 override 가능)
    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    // 대미지 입었을 때의 처리
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage; // 전달받은 damage 만큼 체력을 감소시킴

        if (health <= 0 && !dead)
        {
            Die(); // 체력이 0보다 떨어지면 사망 처리 메서드 호출
        }
    }

    // 체력 회복 처리
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return; // 이미 사망한 상태라면 체력 회복 생략
        }

        health += newHealth; // 체력 증가
    }

    // 사망 처리
    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath(); // 사망 이벤트 델리게이트에 등록된 메서드들 실행
        }

        dead = true; // 사망상태 변경
    }
}
