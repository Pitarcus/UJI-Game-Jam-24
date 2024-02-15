using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSoundController : MonoBehaviour
{
    StudioEventEmitter emitter;


    private void Start()
    {
        UpdateLevel(0);
    }
    private void Awake()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }
    public void UpdateVelocity(float speedNormalized)
    {
        emitter.SetParameter("PlayerSpeed", speedNormalized);
    }

    public void UpdateLevel(int level)
    {
        if (level < 5)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Level", level);
        }
    }

    private void Update()
    {
        UpdateVelocity(PlayerMovement.Instance.GetRigidbody().velocity.magnitude / PlayerMovement.Instance.moveSpeed);
    }
}
