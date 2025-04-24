using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitBtnHandler : MonoBehaviour
{
    private Button button; // 비공개 버튼 변수
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        SceneManager.LoadScene("LobbyScene");   
    }
}
