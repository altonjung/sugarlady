using System.Collections;
using System.Collections.Generic;
// using DialogueNamespace; // 네임스페이스 추가
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using InventoryNamespace;

// actor 내 발생되는 event 처리
public class ActorActionListener : MonoBehaviour
{
    AudioSource _audioSource;

    AudioClip _walkAudioClip;

    // AudioClip attackAudioClip;
    // AudioClip defenceAudioClip;
    // AudioClip hitAudioClip;

    ActorStatus _inventory;

    void Start()
    {
        _inventory = new ActorStatus();
        _audioSource = GetComponent<AudioSource>();
        _walkAudioClip = Resources.Load<AudioClip>("Sound/action/walk/high_heel");
        // attackAudioClip = Resources.Load<AudioClip>("Sound/action/walk/high_heel");
        // defenceAudioClip = Resources.Load<AudioClip>("Sound/action/walk/high_heel");
        // hitAudioClip = Resources.Load<AudioClip>("Sound/action/walk/high_heel");
    }

    public void OnSound(string _a_material = "wood", string a_type = "")
    {
        if (a_type == "walk")
        {
            if (_walkAudioClip != null)
            {
                _audioSource.clip = _walkAudioClip;
                _audioSource.Play(); // 오디오 재생
            }
        }
    }
}
