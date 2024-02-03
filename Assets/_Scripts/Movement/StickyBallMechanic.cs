using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBallMechanic : MonoBehaviour
{
    [SerializeField] public int sizeLevel = 1;

    private void OnCollisionEnter(Collision collision)
    {
        StickableObject collisionObject = collision.gameObject.GetComponent<StickableObject>();

        if (collisionObject != null)
        {
            // TODO: check size, if size is inside the range, get points and keep growing
            // Also check if there is an evolution point, (maybe every 20/30 points?)

            if(collisionObject.size <= sizeLevel)
            {
                sizeLevel += collisionObject.size;

                collisionObject.ManageObjectStuck(transform);
            }
        }
    }
}
