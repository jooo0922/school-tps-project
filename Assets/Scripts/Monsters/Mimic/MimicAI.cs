using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Mimic AI 시스템 구현
public class MimicAI : LivingEntity
{
    [Header("Target")]
    public LayerMask whatIsTarget; // 추적 대상 레이어

    [Header("Effects")]
    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 이펙트
    public AudioClip deathSound; // 사망 시 재생할 오디오
    public AudioClip hitSound; // 피격 시 재생할 오디오

    [Header("Mimic Stats")]
    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    public float speed = 4; // 이동속도

    [Header("Nav Agent")]
    public NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트 컴포넌트 (NavAgent 자식 게임 오브젝트에서 가져옴 -> NavMeshAgent 를 Mimic 게임 오브젝트에 추가하면 transform 계산 시 충돌 발생)

    private LivingEntity targetEntity; // 추적 대상 저장 변수
    private AudioSource mimicAudioPlayer; // 오디오 소스 컴포넌트

    // 추적 대상 존재 여부 검사용 프로퍼티
    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                // 추적 대상 존재하고, 추적 대상이 아직 살아있으면 true 반환
                return true;
            }

            return false; // 나머지 경우 모두 false 반환
        }
    }

    // 필요한 컴포넌트들 가져오기
    private void Awake()
    {
        mimicAudioPlayer = GetComponent<AudioSource>();
    }

    // AI 추적 루틴 트리거
    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    // 추적 대상을 찾아 주기적으로 경로 갱신 
    private IEnumerator UpdatePath()
    {
        // 무한 루프를 돌며 주기적으로 
        while(!dead)
        {
            if (hasTarget)
            {
                // 추적 대상이 존재할 경우 처리
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(
                    targetEntity.transform.position);
            }
            else
            {
                // 추적 대상이 존재하지 않을 경우 처리
                navMeshAgent.isStopped = true; // 추적대상이 없으므로 이동 중지

                // 20유닛 반경 내의 가상의 구와 충돌하는 모든 콜라이더 컴포넌트들 가져오기
                // 이때, target 레이어만 가져오도록 필터링함.
                Collider[] colliders = Physics.OverlapSphere(transform.position, 15f, whatIsTarget);

                // 모든 콜라이더를 순회해서 LivingEntity 상속받은 게임 오브젝트 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>(); // LivingEntity 컴포넌트 가져오기

                    // 살아있는 LivingEntity 를 타겟으로 변경하고, 반복문 중단
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.25f); // 0.25초 주기로 추적 경로 갱신
        }
    }

    // 공격받았을 때의 처리 override
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 시 처리 override
    public override void Die()
    {
        base.Die();
    }

    // 타겟과 충돌 시, 공격 처리
    private void OnTriggerStay(Collider other)
    {
        
    }
}
