using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SpaceController : MonoBehaviour
{    
    public GameObject prefabRoot;

    public GameObject playerPrefab;
    public GameObject playerPosMark;

    public GameObject[] npcPrefabs;
    public GameObject[] npcPosMarks;

    public float waitForStart = 0.0f;

    SpriteRenderer spriteRenderer;

    List<GameObject> playerActors = new List<GameObject>();
    List<GameObject> npcActors = new List<GameObject>();

    IEnumerator Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // 초기 알파값을 0으로 설정
            Color newColor = spriteRenderer.color;
            newColor.a = 0f;
            spriteRenderer.color = newColor;

            yield return StartCoroutine(FadeIn(waitForStart));
        }
    }

    void OnEnable() {
        // player 활성화 및 player position mark 에 따른 위치 구성
            if (playerPrefab != null) {
                // Player tag를 바로 사용못하는 이유는, 해당 오브젝트가 비활성화 될 수 있는 경우가 있기 때문
                GameObject _player = Instantiate(playerPrefab, prefabRoot.transform);

                playerActors.Add(_player);

                ActorMotionController actorScript = _player.GetComponent<ActorMotionController>();                

                if (playerPosMark != null) {      
                    // actor position mark 로 tranform 위치 이동                
                    ActorPositionMark markScript = playerPosMark.GetComponent<ActorPositionMark>();              
                    actorScript.SetActorPosition(playerPosMark.transform.position);
                    actorScript.SetActorDirection(markScript.getDirection());                                  
                } else {
                    Debug.Log("playerPosMark not found");
                }
            }

            // npc 활성화 및 npc position mark 에 따른 위치 구성
            for (int i = 0; i < npcPrefabs.Length; i++)
            {    
                GameObject _npc = Instantiate(npcPrefabs[i], prefabRoot.transform);
                npcActors.Add(_npc);
                GameObject _posMark =  npcPosMarks[i];

                ActorMotionController npcScript = _npc.GetComponent<ActorMotionController>();  

                if (npcScript != null) {
                    // actor position mark 로 tranform 위치 이동
                    ActorPositionMark markScript = _posMark.GetComponent<ActorPositionMark>();
                    npcScript.SetActorPosition(_posMark.transform.position);
                    npcScript.SetActorDirection(markScript.getDirection());   
                }  else {
                    Debug.Log("npcPosMark not found");
                }              
            }
    }

    void OnDisable(){
        for (int i = 0; i < npcActors.Count; i++) {
            Destroy(npcActors[i]);
        }
        
        for (int i = 0; i < playerActors.Count; i++) {
            Destroy(playerActors[i]);
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        float startAlpha = spriteRenderer.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1, time / duration);
            Color newColor = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                alpha
            );
            spriteRenderer.color = newColor;
            yield return null;
        }

        // 완전히 나타났을 때
        spriteRenderer.color = new Color(
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            1
        );
    }

    public void SetPlayerPosMark(GameObject posMark) {
        playerPosMark = posMark;        
    }

    // 현재 공간에서 타 공간으로 이동
    // public void moveToPlace(GameObject targetPlace, GameObject placePosMark)
    // {
    //     gameObject.SetActive(false);
        
    //     if (targetPlace != null) {                                             
    //         targetPlace.SetActive(true);

    //         if (placePosMark != null) {
    //             SpaceController script = targetPlace.GetComponent<SpaceController>();
    //             script.SetPlayerPosMark(placePosMark);
    //         } else {
    //             Debug.Log($"placePosMark not found");
    //         }
    //     }
    // }
}
