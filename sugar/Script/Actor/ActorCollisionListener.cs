using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCollisionListener : MonoBehaviour
{
    ActorMotionController _script;

    void Start()
    {
        _script = gameObject.GetComponent<ActorMotionController>();
    }

    void OnTriggerEnter(Collider _a_other)
    {
        // Debug.Log($" {_a_other.name}");
        if (_script != null)
        {
            if (_a_other.name == "Collider_light")
                _script.changeActorColor(Color.white);
            else if (_a_other.name == "Collider_light_red")
                _script.changeActorColor(new Color(1.0f, 0.5f, 0.75f));
        }
    }

    void OnTriggerStay(Collider _a_other)
    {
        if (_script != null)
        {
            if (_a_other.name == "Collider_light")
                _script.changeActorColor(Color.white);
            else if (_a_other.name == "Collider_light_red")
                _script.changeActorColor(new Color(1.0f, 0.5f, 0.75f));
        }
    }

    void OnTriggerExit(Collider _a_other)
    {
        if (_script != null)
        {
            if (_a_other.name == "Collider_light")
                _script.changeActorColor(Color.grey);
            else if (_a_other.name == "Collider_light_red")
                _script.changeActorColor(Color.grey);
        }
    }

    // void OnCollisionEnter(Collision _a_collision) {

    //     if (_a_collision.gameObject.name == "Collider_wall") {

    //         if (_script) {
    //             _script.RestrictCurMove();
    //         }
    //     }
    // }

    // void OnCollisionStay(Collision _a_collision) {
    // }

    // void OnCollisionExit(Collision _a_collision) {
    //     if (_a_collision.gameObject.name == "Collider_wall") {
    //         _script.ReleaseAllMove();
    //     }
    // }    
}
