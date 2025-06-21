using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using ItemNamespace;

public abstract class _ObjectController : MonoBehaviour
{
    public string place;
    public string title;
    public string type;
    public string action;
    public string action_sound;
    public float  amount = 1.0f;

    public GameObject ItemMark; // 활성화 시점에 사용자에 보여질 아이템 모습

    protected AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();   
    }

    public abstract void DoAction(GameObject _a_targetActor);

    public string GetItemInfo() {
        return ItemMgmt.Instance.GetItemInfo(place, title);
    }

    public void ShowMark()
    {
        ItemMark.SetActive(true);
    }

    public void HideMark()
    {
        ItemMark.SetActive(false);
    }        

    public void Cleanup()
    {
        // Confirm Box 비활성화 또는 삭제
        Destroy(gameObject);
    }
}
