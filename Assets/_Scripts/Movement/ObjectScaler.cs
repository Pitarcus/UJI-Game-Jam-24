using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    [SerializeField] private float[] scaleByLevel;
    [SerializeField] private float transitionTime;


    public void Scale(int currentLevel)
    {
        Vector3 newsScale = new Vector3(scaleByLevel[currentLevel-1], scaleByLevel[currentLevel - 1], scaleByLevel[currentLevel - 1]);
        transform.DOScale(newsScale, transitionTime);
    }
}
