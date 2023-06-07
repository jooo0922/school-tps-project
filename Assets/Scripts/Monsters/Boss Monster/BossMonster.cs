using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : LivingEntity
{
    [Header("Effects")]
    public AudioClip hitSound; // 피격 시 재생할 오디오

    private AudioSource bossMonsterAudio; // 오디오 소스 컴포넌트

    private bool isFirstAttack = true; // 첫 공격 여부 

    // 공격받았을 때의 처리 override
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            if (isFirstAttack)
            {
                UIManager.instance.ShowBossHealth(); // 첫 공격 시 보스 체력 바를 보여줌
                isFirstAttack = false;
            }

            // 공격받았을 때의 처리
            bossMonsterAudio.PlayOneShot(hitSound); // 피격 오디오 재생
        }

        base.OnDamage(damage, hitPoint, hitNormal);
        UIManager.instance.UpdateBossHealth(health, startingHealth); // 보스몬스터 체력 UI 업데이트
    }

    public override void Die()
    {
        base.Die();
        UIManager.instance.HideBossHealth(); // 보스몬스터 사망 시, 체력 UI 숨기기 
    }

    // Start is called before the first frame update
    private void Awake()
    {
        bossMonsterAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
