using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총 기능 구현 모듈
public class Gun : MonoBehaviour
{
    // 총의 상태를 표현하는 enum
    public enum State
    {
        Ready, // 발사 준비 상태
        Empty, // 탄창이 빈 상태
        Reloading // 재장전 상태
    }

    public State state { get; private set; } // 총의 상태값을 자동구현 프로퍼티로 접근 및 쓰기

    public GunData gunData; // 총의 현재 데이터

    public ParticleSystem muzzleFlashEffect; // 총구 화염 이펙트 파티클 시스템 컴포넌트
    public ParticleSystem shellEjectEffect; // 탄피 배출 이펙트 파티클 시스템 컴포넌트

    public int ammoRemain; // 남아있는 모든 탄알 수
    public int magAmmo; // 현재 탄창 안에 남아있는 탄알 수

    private AudioSource gunAudioPlayer; // 총 효과음 재생
    private Animator gunAnimator; // 총기 반동 애니메이션 재생

    private float fireDistance = 50f; // 총의 사정거리 > 너무 멀리있는 오브젝트의 레이캐스팅 처리 제한 목적
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    [SerializeField]
    private Transform m_leftHandMount; // 총의 왼쪽 손잡이
    [SerializeField]
    private Transform m_rightHandMount; // 총의 오른쪽 손잡이
    [SerializeField]
    private Transform m_firePosition; // 총구 위치

    public Transform leftHandMount
    {
        get { return m_leftHandMount; }
    }

    public Transform rightHandMount
    {
        get { return m_rightHandMount; }
    }

    public Transform firePosition
    {
        get { return m_firePosition; }
    }

    // 컴포넌트 가져오기
    private void Awake()
    {
        InitComponents();
        InitGunState();
    }

    // 필요한 컴포넌트들 가져오기
    private void InitComponents()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        gunAnimator = GetComponent<Animator>();
    }

    // 총 상태 초기화
    private void InitGunState()
    {
        ammoRemain = gunData.startAmmoRemain; // 전체 탄알 수 초기화
        magAmmo = gunData.magCapacity; // 현재 탄창 탄알 수 초기화
        state = State.Ready; // 현재 총 상태 초기화
        lastFireTime = 0; // 총을 마지막으로 발사한 시점 초기화
    }

    // 총 발사 트리거
    public void Fire()
    {
        // 현재 상태가 발사 준비 상태인지,
        // 마지막 총 발사 후, gunData.timeBetweenFire 만큼 시간이 지났는지 검사
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetweenFire)
        {
            lastFireTime = Time.time; // 마지막 발사 시점을 현재 시점으로 초기화
            Shot(); // 실제 발사 처리
        }
    }

    // 실제 발사 처리
    private void Shot()
    {
        RaycastHit hit; // 레이캐스트 충돌정보 저장하는 컨테이너 변수
        Vector3 hitPosition = Vector3.zero; // 레이캐스팅 충돌 지점 좌표를 저장할 변수 초기화

        // 레이캐스트 검사 실행 (레이캐스틴 시작 지점, 레이캐스팅 방향, 레이캐스팅 결과 저장 변수, 레이캐스팅 유효거리)
        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, fireDistance))
        {
            // 레이가 어떤 물체와 교차했을 때의 처리
            IDamageable target = hit.collider.GetComponent<IDamageable>(); // 교차한 오브젝트들 중에서 IDamageable 을 상속받는 컴포넌트를 갖고 있는 오브젝트 가져오기
            if (target != null)
            {
                // 충돌한 오브젝트들 중에서 IDamageable 인터페이스를 상속받는 컴포넌트가 존재하는 오브젝트가 존재할 때의 처리
                // IDamageable 인터페이스의 OnDamage() 메서드 실행 (깎을 데미지, 충돌 지점 좌표, 충돌 표면 방향벡터 전달)
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            hitPosition = hit.point; // 레이캐스팅 충돌 지점 좌표 저장
        }
        else
        {
            // 레이가 교체한 물체가 없을 때의 처리
            // 충돌 지점 좌표를 현재 총구위치를 기준으로 총구 방향벡터를 따라 최대 사정거리까지 날아간 지점의 좌표로 계산
            hitPosition = firePosition.position + firePosition.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition)); // 발사 이펙트 처리 코루틴 메서드 실행

        magAmmo--; // 현재 탄창 탄알 수 감소시킴
        if (magAmmo <= 0)
        {
            state = State.Empty; // 탄창에 남은 탄알 수가 0보다 작다? > 탄창이 빈 상태로 상태 변경
        }
    }

    // 발사 이펙트 처리하는 코루틴 메서드
    private IEnumerator ShotEffect(Vector3 hitPoisition)
    {
        // 탄알 발사 이펙트 처리를 위한 파티클 시스템 재생
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        gunAnimator.SetTrigger("Fire");

        gunAudioPlayer.PlayOneShot(gunData.fireClip); // 총 발사 오디오 재생 (총 연속 발사 시, 기존 효과음과 자연스럽게 중첩 처리 -> .PlayOnmeShot())
        yield return new WaitForSeconds(0.03f);
    }

    // 재장전 트리거
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            // 현재 재장전 중이거나, 탄알이 없거나,
            // 현재 탄창이 최대일 때에는 재장전 입력을 무시함
            return false;
        }

        StartCoroutine(ReloadRoutine()); // 위의 3가지 케이스가 아니면 실제 재장전 처리 코루틴 메서드 실행
        return true;
    }

    // 실제 재장전 처리 코루틴 메서드
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading; // 현재 총 상태를 재장전 상태로 전환
        gunAudioPlayer.PlayOneShot(gunData.reloadClip); // 이전 오디오를 중첩시켜서 자연스럽게 재생시킴

        yield return new WaitForSeconds(gunData.reloadTime); // 재장전 소요 시간만큼 아래 로직처리를 지연

        int ammoToFill = gunData.magCapacity - magAmmo; // 채워야 할 탄알 수 계산

        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain; // 채워야 할 탄알이 남아있는 탄알보다 많다면, 채워야 할 탄알 수를 남아있는 탄알 수로 갱신
        }

        magAmmo += ammoToFill; // 탄창을 채움
        ammoRemain -= ammoToFill; // 채운 탄알 수만큼 남아있는 탄알 수를 감소시킴

        state = State.Ready; // 재장전 처리가 끝났으면, 현재 총의 상태를 발사 준비 상태로 전환
    }
}
