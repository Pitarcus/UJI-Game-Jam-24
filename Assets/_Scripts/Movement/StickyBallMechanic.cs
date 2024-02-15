using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    public SphereCollider movementCollider;
    public SphereCollider stickerCollider;

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

    }

    private void OnCollisionEnter(Collision collision)
    {
        StickableObject stickableObject = collision.gameObject.GetComponent<StickableObject>();

        if (stickableObject != null)    // When the collision is a stickable object
        {
            IncreaseSize(stickableObject.size, stickableObject, collision);
        }
    }


    public void IncreaseSize(int size, StickableObject collisionObject, Collision collision)
    {
        if (size <= sizeLevel / 4 || sizeLevel < 20 && size < 5)
        {
            sizeLevel += size;
            onIncreaseSize.Invoke(sizeLevel);
            collisionObject.ManageObjectStuck();

            // The contact normal does not give you this object's surface normal, just the collision's
            Vector3 sphereNormal = new Vector3();
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 direction = collision.transform.position - transform.position;

            collisionPoint += direction * 0.1f;
            RaycastHit hitInfo;

            if (Physics.Raycast(new Ray(collisionPoint, -direction), out hitInfo, 2, ~LayerMask.NameToLayer("Sticker")))
            {
                // this is the collider surface normal
                sphereNormal = hitInfo.normal;
            }


            collisionObject.transform.position = collisionObject.transform.position + 
                -sphereNormal * 0.3f * collisionObject.size * 0.2f / ((currentLevel + 1) * 0.15f);
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

        movementCollider.radius = ballColliderSizes[currentLevel - 1].y;
        stickerCollider.radius = ballColliderSizes[currentLevel - 1].y * 1.1f;

        _ballMovement.moveSpeed += 2.5f + 0.1f * currentLevel;

        onLevelUp.Invoke(currentLevel);
    }
}
