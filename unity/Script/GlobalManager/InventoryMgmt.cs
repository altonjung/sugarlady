namespace InventoryNamespace
{
    using UnityEngine;

    public class InventoryMgmt : MonoBehaviour
    {
        // Singleton 인스턴스
        static InventoryMgmt instance;

        static InventoryMap inventoryMap;

        // Singleton 인스턴스에 접근할 수 있는 프로퍼티
        public static InventoryMgmt Instance
        {
            get
            {
                if (instance == null)
                {
                    // 게임 내에서 DataManager 오브젝트를 찾아서 가져오거나 생성
                    instance = FindFirstObjectByType<InventoryMgmt>();

                    if (instance == null)
                    {
                        // 게임 내에 DataManager 오브젝트가 없다면 새로 생성
                        GameObject obj = new GameObject("InventoryMgmt");
                        instance = obj.AddComponent<InventoryMgmt>();

                        inventoryMap = new InventoryMap();
                    }
                }

                return instance;
            }
        }

        public float checkFromInventory(string _a_type)
        {
            float _value = 0.0f;
            if (_a_type == "coin")
                _value = inventoryMap.coin;

            return _value;
        }

        public void putIntoInventory(string _a_type, float _a_amount)
        {
            if (_a_type == "coin")
                inventoryMap.coin += _a_amount;
        }

        public void getFromInventory(string _a_type, float _a_amount)
        {
            if (_a_type == "coin")
            {
                inventoryMap.coin -= _a_amount;

                if (inventoryMap.coin < 0)
                {
                    inventoryMap.coin = 0;
                }
            }
        }

        // public override void LoadJson()
        // {
        //     if (item_path != null)
        //     {
        //         string filePath = Path.Combine(Application.persistentDataPath, item_path); //
        //         Debug.Log($"path {filePath}");

        //         if (File.Exists(filePath))
        //         {
        //             string json = File.ReadAllText(filePath);
        //             data = JsonUtility.FromJson<ItemData>(json);
        //         }
        //         else
        //         {
        //             Debug.LogWarning("JSON 파일이 없습니다.");
        //         }
        //     }
        // }

        // public override void SaveJson()
        // {
        //     if (item_path != null)
        //     {
        //         // JSON으로 변환
        //         string json = JsonUtility.ToJson(data, true);

        //         // 파일 저장 경로 설정
        //         string filePath = Path.Combine(Application.persistentDataPath, item_path);

        //         // 파일에 저장
        //         File.WriteAllText(filePath, json);

        //         Debug.Log($"JSON 저장 경로: {filePath}");
        //     }
        // }
    }
}
