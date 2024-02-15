using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;

public class StickableObject : MonoBehaviour
{
    [SerializeField] public int size = 1;

    private Collider stickCollider;
    private Rigidbody rb;

    private bool canStick = false;

    private StickyBallMechanic stickyBall;

    public UnityEvent onStick;


    private void Awake()
    {
        stickCollider = GetComponent<Collider>();
        stickyBall = GameObject.FindGameObjectWithTag("StickyBall").GetComponent<StickyBallMechanic>();
        rb = GetComponent<Rigidbody>();
    }


    public void ManageObjectStuck()
    {
        stickCollider.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        transform.parent = stickyBall.transform;

        onStick.Invoke();
        //  stickyBall.onLevelUp.AddListener(SetToCanStick);
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
