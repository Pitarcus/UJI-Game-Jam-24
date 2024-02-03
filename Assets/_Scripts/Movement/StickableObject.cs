using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Scripting;

public class StickableObject : MonoBehaviour
{
    [SerializeField] public int size = 1;

    private Collider stickCollider;


    private void Awake()
    {
        stickCollider = GetComponent<Collider>();
    }
    public void ManageObjectStuck(Transform ballTransform)
    {
        stickCollider.isTrigger = true;

        transform.parent = ballTransform;
    }
}
