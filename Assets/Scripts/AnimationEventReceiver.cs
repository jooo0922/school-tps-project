using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// VRoid 게임 오브젝트 > 애니메이터 컴포넌트 > 애니메이션 클립들에 추가된 이벤트 메서드들 정의하는 모듈
// 애니메이션 클립의 이벤트 메서드를 정의하는 컴포넌트(유니티에서는 Receiver 라고 함.)는
// 애니메이터 컴포넌트가 추가되어 있는 동일한 게임 오브젝트에 추가해줘야 한다고 함.
// 따라서, 실제 처리는 부모 게임 오브젝트인 플레이어 캐릭터에 정의해놓고, 그것들을 이 Receiver 컴포넌트에서 위임 실행한 것.
public class AnimationEventReceiver : MonoBehaviour
{
    private void OnJumpUp()
    {
        GetComponentInParent<PlayerMovement>().OnJumpUp();
    }

    private void OnJumpLand()
    {
        GetComponentInParent<PlayerMovement>().OnJumpLand();
    }

    private void OnJumpEnd()
    {
        GetComponentInParent<PlayerMovement>().OnJumpEnd();
    }
}
