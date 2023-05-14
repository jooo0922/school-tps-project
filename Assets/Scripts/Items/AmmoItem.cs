using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour, IItem
{
    public int ammo = 30; // 추가할 총알 개수
    public string gunName = "rifle"; // 총알에 대응되는 총기 식별자

    public void Use(GameObject target)
    {
        PlayerGunManager playerGunManager = target.GetComponent<PlayerGunManager>(); // PlayerShooter 컴포넌트 가져오기 시도

        if (playerGunManager != null)
        {
            playerGunManager.addGunAmmo(gunName, ammo); // 해당하는 총 인스턴스를 찾아 탄알 수를 추가하는 메서드 실행
        }

        Destroy(gameObject); // 사용된 탄알 아이템 오브젝트 제거
    }
}
