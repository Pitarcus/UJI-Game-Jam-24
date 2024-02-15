using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraControl : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] cameras;


    public void SetCameraPriorityIndex(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if(i == index)
            {
                cameras[i].Priority = 10;
            }
            else
            {
                cameras[i].Priority = 0;
            }
        }
    }
}
