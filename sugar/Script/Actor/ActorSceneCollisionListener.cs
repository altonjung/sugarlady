using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorSceneCollisionListener : MonoBehaviour
{    
    ActorSceneController  _script;
    
    void Start() {
        Transform _parentTransform = transform.parent;
        _script = _parentTransform.gameObject.GetComponent<ActorSceneController>();     
    }

    void OnTriggerEnter(Collider _a_other)
    {
        if (_script != null) {
            if (_a_other.gameObject.name == "Collider_light")
                _script.ChangeActorColor(Color.white);
            else if (_a_other.gameObject.name == "Collider_light_red")
                _script.ChangeActorColor(new Color(1.0f, 0.5f, 0.75f));                
        }
    }    

    void OnTriggerStay(Collider _a_other)
    {
        if (_script != null) {
            if (_a_other.gameObject.name == "Collider_light")
                _script.ChangeActorColor(Color.white);
            else if (_a_other.gameObject.name == "Collider_light_red")
                _script.ChangeActorColor(new Color(1.0f, 0.5f, 0.75f));                
        }
    }       

    void OnTriggerExit(Collider _a_other)
    {
        if (_script != null) {
            if (_a_other.gameObject.name == "Collider_light")
                _script.ChangeActorColor(Color.grey);
            else if (_a_other.gameObject.name == "Collider_light_red")
                _script.ChangeActorColor(Color.grey);                
        }
    }    
}
