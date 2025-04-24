using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 클래스 포함
using System.IO;

public class DialogueBox : MonoBehaviour
{
    public Text dialogueText; // UI Text 컴포넌트를 참조
    public Image actorImage;
    public Sprite dialogueBGBox;    

    void Start()
    {
    }

    // 지정한 시간이 지나면 텍스트를 지움
    public void Show(List<DialogueMap> dialogues)  {
        StartCoroutine(ShowText(dialogues));
    }

    IEnumerator ShowText(List<DialogueMap> dialogues)
    {        
        
        foreach (DialogueMap _dialogue in dialogues)
        {
            string spriteFilePath = "Dialogue/Thumbnail/" + _dialogue.name + "/" + _dialogue.mood;
            actorImage.sprite = Resources.Load<Sprite>(spriteFilePath);

            dialogueText.text = _dialogue.prompt;

            yield return new WaitForSeconds(_dialogue.duration);
            dialogueText.text = "";
            actorImage.sprite = dialogueBGBox;

            Debug.Log($"dialogue: {_dialogue.name}, {_dialogue.mood}, {_dialogue.prompt}");
        }        

        Cleanup();
    }

    public void Cleanup()
    {    
        // Confirm Box 비활성화 또는 삭제
        Destroy(gameObject);
    }
}
