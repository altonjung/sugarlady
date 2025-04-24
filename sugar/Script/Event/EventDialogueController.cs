using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI; // UI 관련 클래스 포함

// Event 를 통해 dialogue 수행 처리
public class EventDialogueController : MonoBehaviour
{
    public string eventName;

    void Start()
    {
        GameObject uiObject = GameObject.FindWithTag("UI_Canvas");

        if (uiObject != null)
        {
            Canvas uiCanvas = uiObject.GetComponent<Canvas>();
            UIController uiScript = uiCanvas.GetComponent<UIController>();
            TextAsset jsonFile = Resources.Load<TextAsset>("Dialogue/" + eventName);

            if (jsonFile != null)
            {
                string json = jsonFile.text;

                List<DialogueMap> dialogueList = new List<DialogueMap>();

                // 이름
                JObject jsonObject = JObject.Parse(json);
                if (jsonObject != null && jsonObject["player"] != null)
                {
                    string name_str = jsonObject["player"].ToString(); //  jsonObject

                    // 대화
                    jsonObject = JObject.Parse(name_str);
                    string dialogue_str = jsonObject["dialogues"].ToString(); //  jsonObject

                    JArray jsonArray = JArray.Parse(dialogue_str); //  jsonArray

                    foreach (JObject item in jsonArray)
                    {
                        DialogueMap dialogue = new DialogueMap();

                        dialogue.name = item["name"].ToString();
                        dialogue.mood = item["mood"].ToString();
                        dialogue.prompt = item["prompt"].ToString();
                        dialogue.duration = float.Parse(item["duration"].ToString());
                        dialogueList.Add(dialogue);
                    }

                    // 초기 대화 처리
                    uiScript.ShowDialogueBox(dialogueList);
                }
                else
                {
                    Debug.Log("no actor to say..");
                }
            }
            else
            {
                Debug.Log("dialogue file open error!!!");
            }
        }
    }
}
