using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // UI 관련 코드
using MimicSpace;

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
    public float damage = 2f; // 공격력
    public float timeBetAttack = 1.25f; // 공격 간격
    public float speed = 3; // 이동속도
    public float height = 0.5f; // Mimic 높이

    [Header("Nav Agent")]
    public NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트 컴포넌트 (NavAgent 자식 게임 오브젝트에서 가져옴 -> NavMeshAgent 를 Mimic 게임 오브젝트에 추가하면 transform 계산 시 충돌 발생)

    [Header("UI")]
    public GameObject mimicHealthBarCanvas;
    public Slider mimicHealthSlider;

    private LivingEntity targetEntity; // 추적 대상 저장 변수
    private AudioSource mimicAudioPlayer; // 오디오 소스 컴포넌트dw
    private Rigidbody mimicRigidBody; // 리지드바디 컴포넌트
    private SphereCollider mimicCollider; // 구체 콜라이더 컴포넌트
    private MeshRenderer mimicRenderer; // sphere 게임 오브젝트 렌더러 
    private Movement movement; // Movement 컴포넌트
    private Mimic mimic;

    private float lastAttackTime; // 마지막 공격 시점

    // MimicData 를 전달받아서 데이터 초기화
    public void Setup(MimicData mimicData)
    {
        // 체력 초기화
        startingHealth = mimicData.health;
        health = mimicData.health;

        // navMeshAgent 설정값 초기화
        navMeshAgent.speed = mimicData.speed;
        navMeshAgent.baseOffset = mimicData.height;

        // legs 관련 데이터 초기화
        mimic.newLegRadius = mimicData.newLegRadius;
        mimic.minLegDistance = mimicData.minLegDistance;
        mimic.widthCurve = mimicData.widthCurve;

        // collider 설정 초기화
        mimicCollider.radius = mimicData.radius;

        // rigidBody 설정 초기화
        mimicRigidBody.mass = mimicData.mass;

        // sphere 설정 초기화
        mimicRenderer.gameObject.transform.localScale = mimicData.scale;

        // 머티리얼 참조 초기화
        if (mimicData.mimicMaterial != null)
        {
            mimicRenderer.material = mimicData.mimicMaterial;
            mimic.mimicMaterial = mimicData.mimicMaterial;
        }
    }

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

    private void Awake()
    {
        // 필요한 컴포넌트들 가져오기
        mimicAudioPlayer = GetComponent<AudioSource>();
        mimicRigidBody = GetComponent<Rigidbody>();
        mimicCollider = GetComponent<SphereCollider>();
        mimicRenderer = GetComponentInChildren<MeshRenderer>(); // sphere 게임 오브젝트 렌더러
        movement = GetComponent<Movement>();
        mimic = GetComponent<Mimic>();

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

                // 25유닛 반경 내의 가상의 구와 충돌하는 모든 콜라이더 컴포넌트들 가져오기
                // 이때, target 레이어만 가져오도록 필터링함.
                Collider[] colliders = Physics.OverlapSphere(transform.position, 25f, whatIsTarget);

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
        if (!dead)
        {
            // 사망상태가 아닌 경우에만 피격 실행 처리
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal); // 충돌표면 방향벡터(=피격방향)를 바라보도록 쿼터니온 회전값을 반환! -> 파티클 효과 게임오브젝트가 피격방향을 바라보도록 함
            hitEffect.Play(); // 파티클 시스템 재생

            // 충돌표면 방향벡터의 반대방향(총구에서 충돌표면까지의 방향) 으로 힘을 가하여 Mimic 이 뒤로 밀려나도록 함
            // 현재 사용중인 GunData 의 damage 값만큼 힘을 가하도록 함. -> damage 가 클수록 멀리 밀려날 것.
            mimicRigidBody.AddForce(-hitNormal * damage, ForceMode.Impulse);

            mimicAudioPlayer.PlayOneShot(hitSound); // 피격 오디오 실행
        }

        base.OnDamage(damage, hitPoint, hitNormal); // 대미지 처리 실행
        UpdateMimicHealth(health, startingHealth); // Mimic 체력 UI 업데이트
    }

    // 사망 시 처리 override
    public override void Die()
    {
        base.Die(); // 사망 처리 실행

        // 다른 mimicAgent 의 길찾기를 방해하지 않도록 사망한 Mimic 콜라이더 컴포넌트 비활성화
        Collider mimicCollider = GetComponent<Collider>();
        mimicCollider.enabled = false;

        // mimicAgent 의 AI 추적 비활성화 및 중단 
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        // Mimic 조작 관련 컴포넌트 전부 비활성화
        mimicRigidBody.isKinematic = true; // 현재 게임오브젝트에서 물리엔진 적용을 비활성화
        mimicRigidBody.detectCollisions = false; // 충돌감지 비활성화
        mimic.enabled = false;
        movement.enabled = false;

        mimicHealthBarCanvas.SetActive(false); // Health Bar UI 비활성화

        mimicAudioPlayer.PlayOneShot(deathSound); // 사망 오디오 재생
    }

    // 타겟과 충돌 시, 공격 처리
    private void OnCollisionStay(Collision collision)
    {
        // 몬스터가 사망상태가 아니고, 마지막 공격 시점에서 timeBetAttack 만큼 시간이 흘렀다면 공격 처리 진행
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = collision.collider.GetComponent<LivingEntity>();

            // 충돌한 타겟에 LivingEntity 컴포넌트가 존재하고, 추적대상과 일치하면 공격 실행
            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time; // 마지막 공격 시점을 현재 시간으로 업데이트

                Vector3 hitPoint = collision.collider.ClosestPoint(transform.position); // 충돌한 상대방 게임 오브젝트의 Collider 물리 표면 중에서, 현재 Mimic 과 가장 가까운 위치를 반환
                Vector3 hitNormal = transform.position - collision.collider.transform.position; // 좀비 위치 ~ 공격받은 상대방 위치로 향하는 방향벡터

                attackTarget.OnDamage(damage, hitPoint, hitNormal); // 공격 실행
            }
        }
    }

    private void UpdateMimicHealth(float currentHealth, float startingHealth)
    {
        mimicHealthSlider.value = currentHealth / startingHealth;
    }
}
