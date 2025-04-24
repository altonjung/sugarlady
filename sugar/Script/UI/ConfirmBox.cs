using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmBox : MonoBehaviour
{
    public Button okButton;
    public Button cancelButton;
    public Text messageText; // 또는 TMP_Text 사용 시 TextMeshProUGUI로 변경

    private TaskCompletionSource<bool> tcs;

    public Task<bool> Show(string message)
    {
        // 메시지를 설정
        messageText.text = message;

        // 새로운 TaskCompletionSource 생성
        tcs = new TaskCompletionSource<bool>();

        // 버튼 클릭 이벤트 등록
        okButton.onClick.AddListener(OnOkClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);

        // Task 반환
        return tcs.Task;
    }

    private void OnOkClicked()
    {
        Cleanup();
        tcs.TrySetResult(true); // OK 클릭 시 true 반환
    }

    private void OnCancelClicked()
    {
        Cleanup();
        tcs.TrySetResult(false); // Cancel 클릭 시 false 반환
    }

    public void Cleanup()
    {
        // 버튼 이벤트 해제
        okButton.onClick.RemoveListener(OnOkClicked);
        cancelButton.onClick.RemoveListener(OnCancelClicked);

        // Confirm Box 비활성화 또는 삭제
        Destroy(gameObject);
    }
}
