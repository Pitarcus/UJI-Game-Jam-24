using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraRadiusControl : MonoBehaviour
{
    [SerializeField] Vector2[] radiusAndHeightIncreasePerLevel;
    [SerializeField] float transitionTime = 0.3f;

    private CinemachineFreeLook cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineFreeLook>();
    }

    public void IncreaseCameraRadius(int currentLevel)
    {
        float radius = radiusAndHeightIncreasePerLevel[currentLevel-1].x;
        DOVirtual.Float(cinemachineCamera.m_Orbits[0].m_Radius, cinemachineCamera.m_Orbits[0].m_Radius + radius, transitionTime, IncreaseCameraRadius0);
        DOVirtual.Float(cinemachineCamera.m_Orbits[1].m_Radius, cinemachineCamera.m_Orbits[1].m_Radius + radius, transitionTime, IncreaseCameraRadius1);
        DOVirtual.Float(cinemachineCamera.m_Orbits[2].m_Radius, cinemachineCamera.m_Orbits[2].m_Radius + radius * 0.6f, transitionTime, IncreaseCameraRadius2);

        float height = radiusAndHeightIncreasePerLevel[currentLevel - 1].y;
        DOVirtual.Float(cinemachineCamera.m_Orbits[0].m_Height, cinemachineCamera.m_Orbits[0].m_Height + height, transitionTime, IncreaseCameraHeight0);
        DOVirtual.Float(cinemachineCamera.m_Orbits[1].m_Height, cinemachineCamera.m_Orbits[1].m_Height + height * 0.8f, transitionTime, IncreaseCameraHeight1);
        //DOVirtual.Float(cinemachineCamera.m_Orbits[2].m_Height, cinemachineCamera.m_Orbits[2].m_Height - height, transitionTime, IncreaseCameraHeight2);
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

    private void IncreaseCameraHeight0(float radius)
    {
        cinemachineCamera.m_Orbits[0].m_Height = radius; // TopRig
    }
    private void IncreaseCameraHeight1(float radius)
    {
        cinemachineCamera.m_Orbits[1].m_Height = radius; // MiddleRig
    }

    private void IncreaseCameraHeight2(float radius)
    {
        cinemachineCamera.m_Orbits[2].m_Height = radius; // BottomRig
    }

}
