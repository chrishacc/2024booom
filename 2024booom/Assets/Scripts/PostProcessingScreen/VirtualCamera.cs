using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noiseProfile;

    public static VirtualCamera Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noiseProfile = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    /// <summary>
    /// ÕðÆÁ
    /// </summary>
    /// <param name="duration">Ê±³¤</param>
    /// <param name="amplitude">·ù¶È</param>
    /// <param name="frequency">ÆµÂÊ</param>
    public void CameraShake(float duration = 2f, float amplitude = 2f, float frequency = 2f)
    {
        if (noiseProfile != null)
        {
            noiseProfile.m_AmplitudeGain = amplitude;
            noiseProfile.m_FrequencyGain = frequency;
            Invoke(nameof(StopShaking), duration);
        }
    }

    /// <summary>
    /// Í£Ö¹ÕðÆÁ
    /// </summary>
    private void StopShaking()
    {
        if (noiseProfile != null)
        {
            noiseProfile.m_AmplitudeGain = 0f;
            noiseProfile.m_FrequencyGain = 0f;
        }
    }
}
