using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.IO;

public class SpaceGroupController : MonoBehaviour
{
    public GameObject nextPlace;

    void Start(){
        StartCoroutine(WaitCoroutine());
        moveNext();
    }

    public void moveNext(){
        if (nextPlace != null) {            
            nextPlace.SetActive(true);
        }
    }

    IEnumerator WaitCoroutine()
    {    
        yield return new WaitForSeconds(0.5f); // 0.5초 대기     
    }
}
