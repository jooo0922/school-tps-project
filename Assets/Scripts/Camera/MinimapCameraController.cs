using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [Header("Follow Target")]
    [Tooltip("Minimap Camera 가 체크된 컴포넌트의 Target 좌표를 따라갑니다.")]
    [SerializeField] private bool x = false;
    [SerializeField] private bool y = false;
    [SerializeField] private bool z = false;

    [Header("Target Transform")]
    [Tooltip("Minimap Camera 가 따라갈 대상을 지정합니다.")]
    [SerializeField] private Transform target; 

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!target)
        {
            return; // 쫒아갈 대상이 없으면 카메라 이동 종료
        }

        // 활성화된 컴포넌트에 한해서 Target 의 좌표를 따라감
        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
}
