using UnityEngine;

public class UIMouseHandler : MonoBehaviour
{
    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 카메라에서 마우스 위치로 Ray 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Ray가 충돌한 오브젝트 확인
            LayerMask layerMask = LayerMask.GetMask("UI");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                // 충돌한 오브젝트의 정보
                GameObject clickedObject = hit.collider.gameObject;
                // Debug.Log($"Clicked on {clickedObject.name}");
                if (clickedObject.tag == "Object_Mark")
                {
                    ObjectPositionMark script = clickedObject.GetComponent<ObjectPositionMark>();
                    script.DoAction();
                    clickedObject.SetActive(false);
                }
            }
        }
    }
}
