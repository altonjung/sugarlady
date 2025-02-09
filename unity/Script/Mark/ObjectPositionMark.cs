using UnityEngine;

public class ObjectPositionMark : MonoBehaviour
{
    public void DoAction()
    {   
        Debug.Log("여기 호출");
        _ObjectController _script = transform.parent.GetComponent<_ObjectController>();
        _script.DoAction(GameObject.FindWithTag("Player"));
    }
}
