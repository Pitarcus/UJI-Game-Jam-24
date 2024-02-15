using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyBallMechanic : MonoBehaviour
{
    [SerializeField] public int sizeLevel = 1;

    [SerializeField] public int[] levelLimits;  // The limits to which the level increases
    [SerializeField] public Vector2[] ballColliderSizes;  // The limits to which the level increases

    [SerializeField] public UnityEvent<int> onIncreaseSize;  // When catching an object that sticks to the ball. returns current size
    [SerializeField] public UnityEvent<int> onLevelUp;   // When leveling up, and increasing the radius of the camera etc. returns current level


    // Private memebers
    public PlayerMovement _ballMovement; // When going up levels, the movement should be slower and clunkier??
    private SphereCollider _sphereCollider;
    private SphereCollider _movementSphereCollider;

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
        _movementSphereCollider = _ballMovement.gameObject.GetComponent<SphereCollider>();
        //_ballMovement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StickableObject collisionObject = collision.gameObject.GetComponent<StickableObject>();

        if (collisionObject != null)
        {
            // TODO: check size, if size is inside the range, get points and keep growing
            // Also check if there is an evolution point, (maybe every 20/30 points?)
            IncreaseSize(collisionObject.size, collisionObject);
            
        }
    }

    public void IncreaseSize(int size, StickableObject collisionObject)
    {
        if (size <= sizeLevel / 4 || sizeLevel < 20 && size < 5)
        {
            sizeLevel += size;
            onIncreaseSize.Invoke(sizeLevel);
            collisionObject.ManageObjectStuck();
        }

        if (currentLevel >= levelLimits.Length)
            return;

        if (currentLevel < levelLimits.Length)
        {
            if (sizeLevel >= levelLimits[currentLevel])
            {
                IncreaseLevel();
            }
        }

        
    }

    // This method increases the level, which means that all of the objects that are stuck to the main one
    // can now stick other onto them. Also the camera increases radius
    public void IncreaseLevel()
    {
        currentLevel += 1;
        Debug.Log(currentLevel);
        _sphereCollider.radius = ballColliderSizes[currentLevel - 1].y;
        _movementSphereCollider.radius = ballColliderSizes[currentLevel - 1].x;
        _ballMovement.moveSpeed += 2.5f + 0.1f * currentLevel;

        onLevelUp.Invoke(currentLevel);
    }
}
