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
        moveNext();
    }

    public void moveNext(){
        if (nextPlace != null) {            
            nextPlace.SetActive(true);
        }
    }
}
