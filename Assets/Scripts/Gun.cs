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

    public int ammoRemain; // 남아있는 모든 탄알 수
    public int magAmmo; // 현재 탄창 안에 남아있는 탄알 수

    private AudioSource gunAudioPlayer; // 총 효과음 재생

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
    public void Awake()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    
}
