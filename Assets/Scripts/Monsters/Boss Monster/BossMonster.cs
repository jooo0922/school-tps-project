using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : LivingEntity
{

    // 공격받았을 때의 처리 override
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            // 공격받았을 때의 처리
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
