using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 사용할 수 있는 아이템 타입들이 공통으로 가져야 하는 인터페이스 정의
public interface IItem
{
    // 아이템을 사용할 대상 게임 오브젝트를 전달받음
    public void Use(GameObject target);
}
