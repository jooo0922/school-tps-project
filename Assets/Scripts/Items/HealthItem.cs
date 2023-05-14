using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour, IItem
{
    public float health = 10; // 회복 수치

    public void Use(GameObject target)
    {
        LivingEntity livingEntity = target.GetComponent<LivingEntity>(); // LivingEntity 컴포넌트를 대상 게임 오브젝트에서 가져오기

        if (livingEntity != null)
        {
            // LivingEntity 컴포넌트가 존재한다면 체력 회복 처리
            livingEntity.RestoreHealth(health);
        }

        Destroy(gameObject); // 아이템을 사용했으면, 자기자신의 인스턴스를 제거함.
    }
}
