using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총 기능 구현 모듈
public class Gun : MonoBehaviour
{
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
