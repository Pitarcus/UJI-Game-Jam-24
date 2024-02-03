using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraRadiusControl : MonoBehaviour
{
    [SerializeField] float[] radiusIncreasePerLevel;
    [SerializeField] float transitionTime = 0.3f;

    private CinemachineFreeLook cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineFreeLook>();
    }

    public void IncreaseCameraRadius(int currentLevel)
    {
        float radius = radiusIncreasePerLevel[currentLevel-1];
        DOVirtual.Float(cinemachineCamera.m_Orbits[0].m_Radius, cinemachineCamera.m_Orbits[0].m_Radius + radius, transitionTime, IncreaseCameraRadius0);
        DOVirtual.Float(cinemachineCamera.m_Orbits[1].m_Radius, cinemachineCamera.m_Orbits[1].m_Radius + radius, transitionTime, IncreaseCameraRadius1);
        DOVirtual.Float(cinemachineCamera.m_Orbits[2].m_Radius, cinemachineCamera.m_Orbits[2].m_Radius + radius, transitionTime, IncreaseCameraRadius2);
    }

    private void IncreaseCameraRadius0(float radius)
    {
        cinemachineCamera.m_Orbits[0].m_Radius = radius; // TopRig
    }
    private void IncreaseCameraRadius1(float radius)
    {
        cinemachineCamera.m_Orbits[1].m_Radius = radius; // MiddleRig
    }

    private void IncreaseCameraRadius2(float radius)
    {
        cinemachineCamera.m_Orbits[2].m_Radius = radius; // BottomRig
    }

}
