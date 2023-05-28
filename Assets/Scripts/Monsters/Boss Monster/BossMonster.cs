using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : LivingEntity
{
    [Header("Effects")]
    public AudioClip hitSound; // 피격 시 재생할 오디오

    private AudioSource bossMonsterAudio; // 오디오 소스 컴포넌트

    // 공격받았을 때의 처리 override
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            // 공격받았을 때의 처리
            bossMonsterAudio.PlayOneShot(hitSound); // 피격 오디오 재생
        }

        base.OnDamage(damage, hitPoint, hitNormal);
        // TODO: UI 업데이트 처리
    }

    public override void Die()
    {
        base.Die();
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
