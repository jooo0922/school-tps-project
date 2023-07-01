using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro; // TextMeshPro (TMP) 관련 코드

public class LobbySettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer soundEffectVolume; // 효과음 오디오 믹서 에셋
    public AudioMixer backgroundVolume; // 배경음 오디오 믹서 에셋

    [Header("Graphics")]
    public TMP_Dropdown resolutionOptionDropdown; // 해상도 옵션 드롭다운

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

    // 그래픽 퀄리티 설정 메서드
    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // 해상도 설정 메서드
    public void SetResolution(int resolutionIndex)
    {
        string selectedResolution = resolutionOptionDropdown.options[resolutionIndex].text;

        // 선택된 해상도 문자열 파싱
        string[] resolutionParts = selectedResolution.Split("*");
        int width = int.Parse(resolutionParts[0].Trim()); // 참고로 Trim() 은 문자열 공백 제거 
        int height = int.Parse(resolutionParts[1].Trim());

        Screen.SetResolution(width, height, true); // 해상도 설정
    }
}

