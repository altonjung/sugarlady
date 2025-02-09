using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI; // UI 관련 클래스 포함
using System.IO;
using System.Collections;
using System.Collections.Generic;


public class UIController : MonoBehaviour
{
    public GameObject confirmBoxPrefab;
    public GameObject DialogueBoxPrefab;

    GameObject confirmBoxInstance;
    GameObject dialogueBoxInstance;

    public void Start(){

    }

    public async Task<bool> ShowConfirmBox(string text)
    {
        if (confirmBoxInstance) {
            CloseConfirmBox();
        }

        confirmBoxInstance = Instantiate(confirmBoxPrefab, transform);

        ConfirmBox confirmBox = confirmBoxInstance.GetComponent<ConfirmBox>();

        bool result = await confirmBox.Show(text); // 내부에서 인스턴스 해제 처리

        confirmBoxInstance = null;

        return result;
    }

    public void CloseConfirmBox() {
        if (confirmBoxInstance != null) {
            ConfirmBox confirmBox = confirmBoxInstance.GetComponent<ConfirmBox>();
            if(confirmBox != null)
                confirmBox.Cleanup();
        }
    }

    public async void ShowDialogueBox(List<DialogueMap> dialogueList)
    {
        if (dialogueBoxInstance) {
            CloseConfirmBox();
        }

        dialogueBoxInstance = Instantiate(DialogueBoxPrefab, transform);

        DialogueBox dialogueBox = dialogueBoxInstance.GetComponent<DialogueBox>();

        dialogueBox.Show(dialogueList); // 내부에서 인스턴스 해제 처리
    }

    public void CloseDialogueBox() {
        if (dialogueBoxInstance != null) {
            DialogueBox dialogueBox = dialogueBoxInstance.GetComponent<DialogueBox>();
            if(dialogueBox != null)
                dialogueBox.Cleanup();
        }

        dialogueBoxInstance = null;
    }

    void OnDestroy() {
        Destroy(confirmBoxInstance);
        Destroy(dialogueBoxInstance);
    }
}
