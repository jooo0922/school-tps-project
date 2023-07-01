using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LobbySettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer soundEffectVolume; // 효과음 오디오 믹서 에셋
    public AudioMixer backgroundVolume; // 배경음 오디오 믹서 에셋

    // 효과음 볼륨 조절 메서드
    public void SetSoundEffectVolume(float volume)
    {
        soundEffectVolume.SetFloat("vol", volume);
    }

    // 배경음 볼륨 조절 메서드
    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume.SetFloat("vol", volume);
    }

    // TODO: 그래픽 설정 제어 (해상도, 퀄리티)
}
