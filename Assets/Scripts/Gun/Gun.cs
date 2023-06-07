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

    public ParticleSystem muzzleFlashEffect; // 총구 화염 이펙트 파티클 시스템 컴포넌트
    public ParticleSystem shellEjectEffect; // 탄피 배출 이펙트 파티클 시스템 컴포넌트
    public ParticleSystem bulletHitEffect; // 탄알 충돌 이펙트 파티클 시스템 컴포넌트

    public int ammoRemain { get; private set; } // 남아있는 모든 탄알 수 프로퍼티
    public int magAmmo { get; private set; } // 현재 탄창 안에 남아있는 탄알 수 프로퍼티
    public int magCapacity { get; private set; } // 탄창 용량 프로퍼티

    public string gunName { get; private set; } // 총기 식별자 프로퍼티

    [SerializeField]
    private GunData gunData; // 총의 현재 데이터

    private AudioSource gunAudioPlayer; // 총 효과음 재생
    private Animator gunAnimator; // 총기 반동 애니메이션 재생
    private LineRenderer bulletLineRenderer; // 탄알 궤적을 그리는 라인 렌더러 컴포넌트

    private float fireDistance = 50f; // 총의 사정거리 > 너무 멀리있는 오브젝트의 레이캐스팅 처리 제한 목적
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    [SerializeField]
    private Transform m_leftHandMount; // 총의 왼쪽 손잡이
    [SerializeField]
    private Transform m_rightHandMount; // 총의 오른쪽 손잡이
    [SerializeField]
    private Transform m_fireTransform; // 총구 위치

    public Transform leftHandMount
    {
        get { return m_leftHandMount; }
    }

    public Transform rightHandMount
    {
        get { return m_rightHandMount; }
    }

    public Transform fireTransform
    {
        get { return m_fireTransform; }
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
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2; // 라인 렌더링에 사용할 점 2개로 초기화
        bulletLineRenderer.enabled = false; // 컴포넌트 비활성화 처리
    }

    // 총 상태 초기화
    private void InitGunState()
    {
        ammoRemain = gunData.startAmmoRemain; // 전체 탄알 수 초기화
        magAmmo = gunData.magCapacity; // 현재 탄창 탄알 수 초기화
        magCapacity = gunData.magCapacity; // 탄창 용량 수 초기화
        gunName = gunData.gunName; // 총기 식별자 초기화
        state = State.Ready; // 현재 총 상태 초기화
        lastFireTime = 0; // 총을 마지막으로 발사한 시점 초기화
    }

    public void AddAmmoRemain(int ammo)
    {
        ammoRemain += ammo;

        UIManager.instance.UpdateGunAmmoText(gunName, ammoRemain); // 현재 총 탄알 UI 업데이트
        UIManager.instance.UpdateTotalAmmoText(); // 전체 탄알 UI 업데이트
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
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
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
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hit, hitPosition)); // 발사 이펙트 처리 코루틴 메서드 실행

        magAmmo--; // 현재 탄창 탄알 수 감소시킴

        UIManager.instance.UpdateAmmoText(magAmmo, magCapacity); // 탄창 UI 업데이트

        if (magAmmo <= 0)
        {
            state = State.Empty; // 탄창에 남은 탄알 수가 0보다 작다? > 탄창이 빈 상태로 상태 변경
        }
    }

    // 발사 이펙트 처리하는 코루틴 메서드
    private IEnumerator ShotEffect(RaycastHit hit, Vector3 hitPosition)
    {
        // 탄알 발사 이펙트 처리를 위한 파티클 시스템 재생
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        if (hit.collider)
        {
            // 충돌한 collider 가 존재할 경우에만 탄알 충돌 이펙트 처리
            bulletHitEffect.transform.position = hit.point; // 탄알 충돌 지점으로 파티클 시스템 위치 변경
            bulletHitEffect.transform.rotation = Quaternion.LookRotation(hit.normal); // 충돌표면 방향벡터(=피격방향)를 바라보도록 쿼터니온 회전값을 반환! -> 파티클 효과 게임오브젝트가 피격방향을 바라보도록 함
            bulletHitEffect.Play(); // 탄알 충돌 파티클 시스템 재생
        }

        gunAnimator.SetTrigger("Fire");

        gunAudioPlayer.PlayOneShot(gunData.fireClip); // 총 발사 오디오 재생 (총 연속 발사 시, 기존 효과음과 자연스럽게 중첩 처리 -> .PlayOnmeShot())

        bulletLineRenderer.SetPosition(0, fireTransform.position); // 라인 렌더러의 시작점 총구 위치로 설정
        bulletLineRenderer.SetPosition(1, hitPosition); // 라인 렌더러의 끝점은 충돌 위치로 설정
        bulletLineRenderer.enabled = true; // 라인 렌더러 활성화 > 탄알 궤적 표시

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false; // 대기처리 후 라인 렌더러 비활성화 > 탄알 궤적 제거 
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

        UIManager.instance.UpdateAmmoText(magAmmo, magCapacity); // 탄창 UI 업데이트
        UIManager.instance.UpdateGunAmmoText(gunName, ammoRemain); // 현재 총 탄알 UI 업데이트
        UIManager.instance.UpdateTotalAmmoText(); // 전체 탄알 UI 업데이트

        state = State.Ready; // 재장전 처리가 끝났으면, 현재 총의 상태를 발사 준비 상태로 전환
    }
}
