using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeCam : MonoBehaviour
{
    public static List<ShakeController> shakeCamparametersBlockNormalStatic = new List<ShakeController>();
    public List<ShakeController> shakeCamparametersBlockNormal = new List<ShakeController>();

    public static List<ShakeController> shakeCamparametersBlockPerfectStatic = new List<ShakeController>();
    public List<ShakeController> shakeCamparametersBlockPerfect = new List<ShakeController>();

    public static List<ShakeController> shakeCamparametersAttackNormalStatic = new List<ShakeController>();
    public List<ShakeController> shakeCamparametersAttackNormal = new List<ShakeController>();

    public static List<ShakeController> shakeCamparametersAttackPerfectStatic = new List<ShakeController>();
    public List<ShakeController> shakeCamparametersAttackPerfect = new List<ShakeController>();

    static CinemachineVirtualCamera cinemachineVirtualCamera;

    static CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannel = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shakeCamparametersBlockNormalStatic = shakeCamparametersBlockNormal;

        shakeCamparametersBlockPerfectStatic = shakeCamparametersBlockPerfect;

        shakeCamparametersAttackNormalStatic = shakeCamparametersAttackNormal;

        shakeCamparametersAttackPerfectStatic = shakeCamparametersAttackPerfect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ShakeCamBlockNormal(Vector3 axeShake, float amplitude, float frequence)
    {
        cinemachineBasicMultiChannel.m_PivotOffset = axeShake;
        cinemachineBasicMultiChannel.m_AmplitudeGain = amplitude;
        cinemachineBasicMultiChannel.m_FrequencyGain = frequence;
    }
}

[System.Serializable]
public class ShakeController
{
    public Vector3 axeShake;

    public float amplitude;
    public float frequence;
}