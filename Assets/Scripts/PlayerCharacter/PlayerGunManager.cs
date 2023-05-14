using System;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀을 적용한 총 게임 오브젝트 관리 모듈
public class PlayerGunManager : MonoBehaviour
{
    public event Action<bool> onMountChange; // 클래스 간 결합도를 낮추기 위한 델리게이트

    public Transform gunPivot; // 총 프리팹 인스턴스를 추가할 부모 오브젝트
    public AudioClip gunSwapClip; // 무기 교체 오디오 클립

    private PlayerInput playerInput; // 플레이어 입력 상태 관리 모듈
    private PlayerShooter playerShooter; // 플레이어 슈터 관리 모듈
    private Animator playerAnimator; // 플레이어 캐릭터 > VRoid 게임 오브젝트의 애니메이터 컴포넌트
    private AudioSource playerAudioSource; // 플레이어 캐릭터 오디오 소스

    [SerializeField]
    private List<GameObject> gunPrefabs; // 총 프리팹 리스트
    private List<Gun> gunPool = new List<Gun>(); // 인스턴스화하여 저장한 총 오브젝트 풀

    private Gun currentGun; // 현재 사용중인 총
    private int currentGunIndex; // 현재 사용중인 총 인스턴스의 인덱스

    private bool isMounted = false; // 무기 착용 상태 여부

    public void addGunAmmo(string gunName, int ammo)
    {
        Gun foundGun = gunPool.Find(gun => gun.gunName == gunName);
        foundGun.ammoRemain += ammo;
    }

    private void Awake()
    {
        InitComponents();
        InitGunPool();
        SetCurrentGun(playerInput.gunIndex);
    }

    private void Update()
    {
        Mount();
        ChangeGun();
    }

    // 필요한 컴포넌트들 가져오기
    private void InitComponents()
    {
        playerShooter = GetComponent<PlayerShooter>();
        playerInput = GetComponent<PlayerInput>();
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>(); // 애니메이터 컴포넌트는 플레이어 캐릭터의 자식 게임 오브젝트에 추가되어 있으니 거기서 가져온 것!        
    }

    // 프리팹 목록을 인스턴스화하여 오브젝트 풀 초기화
    private void InitGunPool()
    {
        for (int i = 0; i < gunPrefabs.Count; i++)
        {
            Gun gun = Instantiate(gunPrefabs[i], Vector3.zero, Quaternion.identity).GetComponent<Gun>();
            if (gun)
            {
                gunPool.Add(gun);
                gun.gameObject.SetActive(false);
                gun.transform.SetParent(gunPivot, false);
            }
        }

    }

    // 현재 총 설정하기
    private void SetCurrentGun(int gunIndex)
    {
        if (IsValidGunIndex(gunIndex))
        {
            if (currentGun != null)
            {
                currentGun.gameObject.SetActive(false); // 현재 무기 비활성화
            }
            Gun gun = gunPool[gunIndex];
            playerShooter.gun = gun;
            currentGun = gun;
            currentGunIndex = gunIndex;
        }
    }

    // 무기 착용 상태 업데이트
    // 무기 착용은 rigidBody 등 물리 애니메이션과 무관하므로, Update() 이벤트 메서드에서 호출
    // FixedUpdate() 내부에서 호출되면 키 입력이 물리 엔진 갱신 주기에만 감지되서 키 입력이 잘 안먹음.
    private void Mount()
    {
        if (playerInput.mount)
        {
            isMounted = !isMounted;
            playerAnimator.SetBool("IsMounted", isMounted);
            playerShooter.enabled = isMounted;
            playerAudioSource.PlayOneShot(gunSwapClip);
            onMountChange(isMounted);
        }
    }

    // 총기 변경하기
    private void ChangeGun()
    {
        if (IsGunChangePossible())
        {
            SetCurrentGun(playerInput.gunIndex);
            currentGun.gameObject.SetActive(true); // 변경된 무기 활성화
            playerAudioSource.PlayOneShot(gunSwapClip); // 무기 교체 오디오 재생
        }
    }

    // 총기 변경 가능 상태 검사
    private bool IsGunChangePossible()
    {
        bool result = IsValidGunIndex(playerInput.gunIndex) && playerInput.gunIndex != currentGunIndex && isMounted && currentGun.state != Gun.State.Reloading;
        return result;
    }

    // gunIndex 유효성 검사
    private bool IsValidGunIndex(int gunIndex)
    {
        bool result = gunIndex >= 0 && gunIndex < gunPool.Count;
        return result;
    }
}
