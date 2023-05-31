using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Pitch")]
    public float mountPitch = 0.9f;
    public float unmountPitch = 1f;

    private AudioSource playerAudioPlayer;
    private PlayerInput playerInput;
    private Animator playerAnimator;

    private bool isMounted = false; // 무기 착용 상태 여부

    // 필요한 컴포넌트 가져오기
    private void Awake()
    {
        playerAudioPlayer = GetComponent<AudioSource>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerAnimator = GetComponentInParent<Animator>();

        playerAudioPlayer.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        ChangePitch();
        ToggleAudioPlayer();
    }

    private void ChangePitch()
    {
        if (playerInput.mount)
        {
            isMounted = !isMounted;

            if (isMounted)
            {
                playerAudioPlayer.pitch = mountPitch;
            }
            else
            {
                playerAudioPlayer.pitch = unmountPitch;
            }
        }
    }

    private void ToggleAudioPlayer()
    {
        Vector2 moveInput = new Vector2(playerInput.leftRightMove, playerInput.frontBackMove);

        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if (moveInput.magnitude != 0 && (stateInfo.IsName("Movement") || stateInfo.IsName("Movement Aim")))
        {
            playerAudioPlayer.enabled = true;
        }
        else
        {
            playerAudioPlayer.enabled = false;
        }
    }
}
