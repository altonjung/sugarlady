using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneMsgMgmt : MonoBehaviour
{   
    public RectTransform contentRect;

    public GameObject  sendImgPrefab; // 표시할 이미지들
    public GameObject  receiveImgPrefab; // 표시할 이미지들
        // Singleton 인스턴스
    private static PhoneMsgMgmt instance;


    private int  itemCnt;
    private float offsetY;

    // Singleton 인스턴스에 접근할 수 있는 프로퍼티
    public static PhoneMsgMgmt Instance
    {
        get
        {
            if (instance == null)
            {
                // 게임 내에서 DataManager 오브젝트를 찾아서 가져오거나 생성
                instance = FindFirstObjectByType<PhoneMsgMgmt>();

                if (instance == null)
                {
                    // 게임 내에 DataManager 오브젝트가 없다면 새로 생성
                    GameObject obj = new GameObject("PhoneMsgMgmt");
                    instance = obj.AddComponent<PhoneMsgMgmt>();        
                }
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start(){
        itemCnt = 0;    
        offsetY = 0.0f;
    }

    public void addSendMessage(string msg){

        GameObject listItem = Instantiate(sendImgPrefab, contentRect);
        addMessage(listItem, msg, 30.0f);
    }

    public void addReceiveMessage(string msg){

        GameObject listItem = Instantiate(receiveImgPrefab, contentRect);
        addMessage(listItem, msg, -30.0f);
    }

    void addMessage(GameObject listItem, string msg, float offsetX) {

        Image imageComponent = listItem.GetComponentInChildren<Image>(); // 이미지 컴포넌트 가져오기

        if (imageComponent != null)
        {
            TextMeshProUGUI textComponent = imageComponent.GetComponentInChildren<TextMeshProUGUI>(); // Text 컴포넌트 가져오기

            if (textComponent != null) {
                textComponent.text = msg;
            }

            RectTransform listItemRect = imageComponent.GetComponent<RectTransform>();
            listItemRect.sizeDelta = new Vector2(340, 60); // 아이템 크기 설정

            float itemPosY = -itemCnt * (70) - offsetY;
            
            listItemRect.anchoredPosition = new Vector2(offsetX, itemPosY);
              
            ScrollToBottomHorizontal();

            itemCnt  += 1;
        }
    }

   void ScrollToBottomHorizontal()
    {
        float contentHeight = contentRect.rect.height;
        float contentY = contentRect.anchoredPosition.y;
    
        if ((itemCnt + 1) * 70 > (contentY + contentHeight)) {
            Vector2 targetPosition = new Vector2(contentRect.anchoredPosition.x, itemCnt * 70 - offsetY);
            contentRect.anchoredPosition  = targetPosition;
        }     
    }
}
