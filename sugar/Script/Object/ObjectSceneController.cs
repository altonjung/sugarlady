using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ObjectSceneController : _ObjectController
{
    public GameObject sceneActorObj;

    public GameObject nextActorPosMark;

    public float      waitForAction = 2.0f;

    public float      start_angle = 2.0f;

    public float      end_angle = 2.0f;

    public override void DoAction(GameObject _a_targetActor) {
        if (action == "open" || action == "close") {
            AudioClip clip = Resources.Load<AudioClip>("Sound/" + type + "/" + action_sound);
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }

            StartCoroutine(PlayWithDelay(_a_targetActor));
        }
    }

    IEnumerator PlayWithDelay(GameObject _a_targetActor) {
        yield return new WaitForSeconds(waitForAction); // 2초 대기


        // curSpaceObj.SetActive(false);
        // nextSpaceObj.SetActive(true);
        // _a_targetActor.transform.position = nextActorPosMark.transform.position;         
    }

    void FixedUpdate() {
        // left, right 키를 통해 camera rotate 수행..
    }
}
