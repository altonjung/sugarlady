using UnityEngine;

public class ObjectPositionMark : MonoBehaviour
{
    public void DoAction()
    {   
        _ObjectController _script = transform.parent.GetComponent<_ObjectController>();
        _script.DoAction(GameObject.FindWithTag("Player"));
    }
}
