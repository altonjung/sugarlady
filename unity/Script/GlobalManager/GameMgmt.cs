using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using InventoryNamespace;
using ItemNamespace;

public class GameMgmt : MonoBehaviour
{
    private static GameMgmt instance;
    private int curTime = 9;

    // Singleton 인스턴스에 접근할 수 있는 프로퍼티
    public static GameMgmt Instance
    {
        get
        {
            if (instance == null)
            {
                // 게임 내에서 DataManager 오브젝트를 찾아서 가져오거나 생성
                instance = FindFirstObjectByType<GameMgmt>();

                if (instance == null)
                {
                    // 게임 내에 DataManager 오브젝트가 없다면 새로 생성
                    GameObject obj = new GameObject("GameMgmt");
                    instance = obj.AddComponent<GameMgmt>();
                }
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        curTime = 9;
        makeInventory();
        makeItems();
    }

    void makeInventory()
    {
        string _filePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        if (!File.Exists(_filePath))
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Mgmt/initInventory");

            if (jsonFile != null)
            {
                File.WriteAllText(_filePath, jsonFile.text);
                Debug.Log($"init inventory");
            }

            // 파일 읽기
            _filePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        }

        string _json = File.ReadAllText(_filePath);
        InventoryMap _data = JsonUtility.FromJson<InventoryMap>(_json);

        InventoryMgmt.Instance.SetInventoryMap(_data);
    }

    void makeItems()
    {
        string _filePath = Path.Combine(Application.persistentDataPath, "items.json");
        if (!File.Exists(_filePath))
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Mgmt/initItems");

            if (jsonFile != null)
            {
                File.WriteAllText(_filePath, jsonFile.text);
                Debug.Log($"init items");
            }

            // 파일 읽기
            _filePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        }

        string _json = File.ReadAllText(_filePath);
        ItemMap _data = JsonUtility.FromJson<ItemMap>(_json);

        ItemMgmt.Instance.SetItemMap(_data);
    }

    public int getCurTime()
    {
        return curTime;
    }

    public void setCurTime(int _curTime)
    {
        curTime = _curTime;
    }
}
