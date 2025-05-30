using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using InventoryNamespace;
using ItemNamespace;

public class ObjectItemController : _ObjectController
{

    public override void DoAction(GameObject _a_targetActor) {
        if (action == "collect") {
            AudioClip clip = Resources.Load<AudioClip>("Sound/" + type + "/" + action_sound);
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }

            if(InventoryMgmt.Instance.saveInventory(title, amount)) {
                ItemMgmt.Instance.SetItem(place, title, amount);
            }
        }
    }
}
