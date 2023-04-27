using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀 패턴 기반으로 무기 게임 오브젝트를 관리하는 모듈
public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> weaponPrefabs; // 무기 프리팹 리스트
    private List<GameObject> weaponPool = new List<GameObject>(); // 인스턴스화하여 저장한 무기 오브젝트 풀
    private int currentWeaponIndex; // 현재 사용중인 무기 인스턴스의 인덱스

    // 프리팹 목록을 인스턴스화하여 오브젝트 풀 초기화
    private void Awake()
    {
        foreach (GameObject prefab in weaponPrefabs)
        {
            GameObject weapon = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            weapon.SetActive(false);
            weaponPool.Add(weapon);
        }

        currentWeaponIndex = 0;
    }

    // 무기 인덱스를 전달받아 해당하는 무기 인스턴스를 활성화 후 반환
    public GameObject GetWeapon(int weaponIndex) 
    {
        if (weaponPool.Count == 0)
        {
            Debug.LogWarning("반환할 무기 인스턴스가 존재하지 않습니다.");
            return null;
        }

        int weaponPoolIndex = Mathf.Abs(weaponIndex) % weaponPool.Count;
        GameObject weapon = weaponPool[weaponPoolIndex];

        if (weapon.activeSelf)
        {
            Debug.LogWarning("해당 무기 인스턴스는 이미 사용중입니다.");
            return null;
        }

        releaseCurrentWeapon();

        currentWeaponIndex = weaponPoolIndex;
        weapon.SetActive(true);
        return weapon;
    }

    // 현재 사용중인 무기 비활성화
    private void releaseCurrentWeapon()
    {
        GameObject currentWeapon = weaponPool[currentWeaponIndex];
        currentWeapon.SetActive(false);
    }
}
