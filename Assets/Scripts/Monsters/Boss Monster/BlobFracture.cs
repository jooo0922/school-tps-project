using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobFracture : MonoBehaviour
{
    [Header("Origin Blob")]
    public GameObject originBlob;
    public GameObject fracturedBlob;

    [Header("Effects")]
    public ParticleSystem energyExplosionEffect; // 폭발 효과 파티클 시스템

    [Header("Explosion Settings")]
    public float explosionMinForce = 5;
    public float explosionMaxForce = 100;
    public float explosionForceRadius = 10;

    private Rigidbody[] fractureRigidbody; // 하위 Fracture 리지드바디 컴포넌트
    private BossMonster bossMonster; // BossMonster 컴포넌트

    // 필요한 컴포넌트 가져오기 및 초기 설정
    private void Awake()
    {
        originBlob.SetActive(true); // Blob 원본 게임 오브젝트를 스크립트에서 확실하게 비활성화 처리
        fracturedBlob.SetActive(false); // Blob Fracture 게임 오브젝트를 스크립트에서 확실하게 비활성화 처리

        fractureRigidbody = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in fractureRigidbody)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll; // 각 리지드바디의 모든 Constraints 를 잠가버림. (인스펙터 창에서 체크해놓으면, Explode 에서 제대로 해제가 제대로 안됨. 따라서 스크립트로 제어!)
        }

        bossMonster = GetComponent<BossMonster>();

        if (bossMonster != null)
        {
            bossMonster.onDeath += Explode; // 부모 게임 오브젝트의 OnDeath 델리게이트에 Explode 메서드 추가 > 디커플링 구현
        }
    }

    private void Explode()
    {
        originBlob.SetActive(false); // Blob 원본 게임 오브젝트를 비활성화 처리
        fracturedBlob.SetActive(true); // Blob Fracture 게임 오브젝트를 활성화 처리

        foreach (Rigidbody rigidbody in fractureRigidbody)
        {
            rigidbody.constraints = RigidbodyConstraints.None; // 각 리지드바디의 모든 Constraints 를 잠금 해제

            rigidbody.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), transform.position, explosionForceRadius); // 각 리지드바디에 폭발 힘 적용
        }

        energyExplosionEffect.Play(); // 폭발 이펙트 처리를 위한 파티클 시스템 재생
    }
}
