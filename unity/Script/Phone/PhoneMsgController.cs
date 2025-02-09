using UnityEngine;
using TMPro;

public class PhoneMsgController : MonoBehaviour
{
    [SerializeField]
    private Transform phoneScrollContent;
    
    [SerializeField]
    private GameObject chatMsgLeftPrefab;

    [SerializeField]
    private GameObject chatMsgRightPrefab;

    [SerializeField]
    private TMP_InputField inputField; 

    private string ID = "user";
    
    void Start()
    {
    }

    private void Update() {

        if ( Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false) {
            inputField.ActivateInputField();
        }
    }

    public void OnEndEditEventMethod() {
        if ( Input.GetKeyDown(KeyCode.Return) ) {
            UpdateChat();
        }
    }

    public void UpdateChat() {
        if ( inputField.text.Equals("")) return;

        GameObject clone = Instantiate(chatMsgLeftPrefab, phoneScrollContent );

        clone.GetComponent<TextMeshProUGUI>().text = $"{ID} : {inputField.text}";
        inputField.text = "";
    }
}
