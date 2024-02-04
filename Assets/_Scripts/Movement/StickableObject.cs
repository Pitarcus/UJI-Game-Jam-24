using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Scripting;

public class StickableObject : MonoBehaviour
{
    [SerializeField] public int size = 1;

    private Collider stickCollider;
    private Rigidbody rb;

    private bool canStick = false;

    private StickyBallMechanic stickyBall;


    private void Awake()
    {
        stickCollider = GetComponent<Collider>();
        stickyBall = GameObject.FindGameObjectWithTag("StickyBall").GetComponent<StickyBallMechanic>();
        rb = GetComponent<Rigidbody>();
    }

    //public void ManageObjectStuck(Transform ballTransform)
    //{
    //    stickCollider.isTrigger = true;

    //    transform.parent = ballTransform;
    //}

    public void ManageObjectStuck()
    {
        stickCollider.isTrigger = true;
        if(rb != null)
            rb.isKinematic = true;

        transform.parent = stickyBall.transform;

        //  stickyBall.onLevelUp.AddListener(SetToCanStick);
    }


    // When entering another object it is available for sticking
    private void OnTriggerEnter(Collider other)
    {
        if(!canStick)
            return;

        StickableObject stickableObject = other.gameObject.GetComponent<StickableObject>();
        if (stickableObject != null)
        {
            stickyBall.IncreaseSize(stickableObject.size, stickableObject);
        }
    }

    public void SetToCanStick(int currentLevel)
    {
        if (size < stickyBall.levelLimits[currentLevel -1] * 0.25f)
        {
            canStick = true;
            stickyBall.onLevelUp.RemoveListener(SetToCanStick);
        }
    }
}
