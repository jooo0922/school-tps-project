using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Effect")]
    public AudioClip openClip; // 문 열기 오디오
    public AudioClip closeClip; // 문 닫기 오디오

    private AudioSource doorAudioPlayer; // 오디오 소스 컴포넌트
    private Animator doorAnimator; // 애니메이터 컴포넌트

    private bool isOpen = false; // 문 개방 상태
    private string interactButtonName = "Interact"; // 상호작용을 위한 입력 버튼 이름

    // 필요한 컴포넌트 가져오기
    private void Start()
    {
        doorAudioPlayer = GetComponent<AudioSource>();
        doorAnimator = GetComponent<Animator>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetButtonDown(interactButtonName) && collision.gameObject.tag == "Player")
        {
            isOpen = !isOpen; // 문 개방 상태 변경

            doorAnimator.SetBool("IsOpen", isOpen); // 애니메이터 상태 변경

            if (isOpen)
            {
                doorAudioPlayer.PlayOneShot(openClip);
            }
            else
            {
                doorAudioPlayer.PlayOneShot(closeClip);
            }
        }
    }
}
