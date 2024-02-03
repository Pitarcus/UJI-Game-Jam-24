using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyBallMechanic : MonoBehaviour
{
    [SerializeField] public int sizeLevel = 1;

    [SerializeField] public int[] levelLimits;  // The limits to which the level increases
    [SerializeField] public float[] ballColliderSizes;  // The limits to which the level increases

    [SerializeField] public UnityEvent<int> onIncreaseSize;  // When catching an object that sticks to the ball. returns current size
    [SerializeField] public UnityEvent<int> onLevelUp;   // When leveling up, and increasing the radius of the camera etc. returns current level


    // Private memebers
    private PlayerMovement _ballMovement; // When going up levels, the movement should be slower and clunkier??
    private SphereCollider _sphereCollider;

    private int currentLevel = 0;   // Index to the levelLimits array

    public static StickyBallMechanic Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _sphereCollider = GetComponent<SphereCollider>();
        _ballMovement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StickableObject collisionObject = collision.gameObject.GetComponent<StickableObject>();

        if (collisionObject != null)
        {
            // TODO: check size, if size is inside the range, get points and keep growing
            // Also check if there is an evolution point, (maybe every 20/30 points?)

            if(collisionObject.size <= sizeLevel)
            {
                IncreaseSize(collisionObject.size);
                collisionObject.ManageObjectStuck();
            }

            Debug.Log(currentLevel);

            if (currentLevel <= levelLimits.Length - 1)
            {
                if (sizeLevel >= levelLimits[currentLevel])
                {
                    IncreaseLevel();
                }
            }
        }
    }

    public void IncreaseSize(int size)
    {
        sizeLevel += size;

        onIncreaseSize.Invoke(sizeLevel);
    }

    // This method increases the level, which means that all of the objects that are stuck to the main one
    // can now stick other onto them. Also the camera increases radius
    public void IncreaseLevel()
    {
        currentLevel += 1;

        _sphereCollider.radius = ballColliderSizes[currentLevel - 1];
        _ballMovement.moveSpeed += 2;

        onLevelUp.Invoke(currentLevel);
    }
}
