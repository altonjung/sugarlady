using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadBtnHandler : MonoBehaviour
{
    public GameObject targetObj;        

    public string btnType;

    private Button button;


    void Start()
    {
        button = GetComponent<Button>();

        // 버튼에 클릭 이벤트를 추가합니다.
        button.onClick.AddListener(OnButtonClick);
    }

    // Start is called before the first frame update
    void OnButtonClick()
    {    
        if (targetObj != null)
                moveSpace();     
    }  

    private void moveSpace()
    {
            if (btnType == "left" || btnType == "right" || btnType == "exit")
            {
                SpaceController  spaceScript = targetObj.GetComponent<SpaceController>();
                
                if (spaceScript != null)
                {
                    // if (btnType == "left")
                    //     spaceScript.moveLeft();
                    // else if (btnType == "right")
                    //     spaceScript.moveRight();
                    // else 
                    //     spaceScript.moveNext();
                }
            }
    }
}
