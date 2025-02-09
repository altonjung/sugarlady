namespace ItemNamespace
{
    using System.IO;
    using UnityEngine;

    public class ItemMgmt : MonoBehaviour
    {
        // Singleton 인스턴스
        static ItemMgmt instance;

        static ItemMap itemMap;

        // Singleton 인스턴스에 접근할 수 있는 프로퍼티
        public static ItemMgmt Instance
        {
            get
            {
                if (instance == null)
                {
                    // 게임 내에서 DataManager 오브젝트를 찾아서 가져오거나 생성
                    instance = FindFirstObjectByType<ItemMgmt>();

                    if (instance == null)
                    {
                        // 게임 내에 DataManager 오브젝트가 없다면 새로 생성
                        GameObject obj = new GameObject("ItemMgmt");
                        instance = obj.AddComponent<ItemMgmt>();

                        itemMap = new ItemMap();

                        init();
                    }
                }

                return instance;
            }
        }

        static void init()
        {
            string _filePath = Path.Combine(Application.persistentDataPath, "items.json");

            if (_filePath != null)
            {
                string _jsonData = File.ReadAllText(_filePath);
                itemMap = JsonUtility.FromJson<ItemMap>(_jsonData);
            }
            else
            {
                Debug.LogWarning("Items.json 파일이 없습니다.");
            }
        }

        public void SetItem(ItemMap _a_items)
        {
            itemMap = _a_items;
        }

        public ItemMap GetItem()
        {
            return itemMap;
        }
    }
}
