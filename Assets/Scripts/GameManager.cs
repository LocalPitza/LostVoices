using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1500, 500);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
