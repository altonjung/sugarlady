namespace InventoryNamespace
{
    using System.IO;
    using UnityEngine;

    // GameMgmt 를 통해서 inventory.json 초기 상태 생성
    // inventory 이력 관리
    // json 파일로 그 이력 관리되어야 함
    public class InventoryMgmt : MonoBehaviour
    {
        // Singleton 인스턴스
        private static InventoryMgmt instance;

        private InventoryMap inventoryMap;

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

                        instance.inventoryMap = new InventoryMap();
                    }
                }

                return instance;
            }
        }

        public float checkInventory(string _a_title)
        {
            float _value = 0.0f;
            if (_a_title == "coin")
                _value = inventoryMap.coin;

            return _value;
        }

        public bool saveInventory(string _a_title, float _a_amount)
        {   
            bool _return = false;

            if (_a_title == "coin")
                inventoryMap.coin += _a_amount;

            SaveJson();
            
            return _return;
        }

        public void consumeInventory(string _a_title, float _a_amount)
        {
            if (_a_title == "coin")
            {
                inventoryMap.coin -= _a_amount;

                if (inventoryMap.coin < 0)
                {
                    inventoryMap.coin = 0;
                }
            }
        }

        public void SetInventoryMap(InventoryMap _a_inventoryMap)
        {
            inventoryMap = _a_inventoryMap;
        }

        void SaveJson()
        {
            // 파일 저장 경로 설정
            string _filePath = Path.Combine(Application.persistentDataPath, "inventory.json");

            // JSON으로 변환
            string _json = JsonUtility.ToJson(inventoryMap, true);

            // 파일에 저장
            File.WriteAllText(_filePath, _json);
        }
    }
}
