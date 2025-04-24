namespace ItemNamespace
{
    using System.IO;
    using UnityEngine;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    // GameMgmt 를 통해서 item.json 초기 상태 생성
    // item 이력 관리
    // json 파일로 그 이력 관리되어야 함

    public class ItemMgmt : MonoBehaviour
    {
        // Singleton 인스턴스
        private static ItemMgmt instance;

        private ItemMap itemMap;

        private JObject jsonItemObject;

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

                        instance.itemMap = new ItemMap();

                        instance.init();
                    }
                }

                return instance;
            }
        }
        void init(){
             TextAsset _jsonFile = Resources.Load<TextAsset>("Items/items");
             jsonItemObject = JObject.Parse(_jsonFile.text);
        }

        public void SetItem(string _a_place, string _a_title, float _a_amount)
        {
            // itemMap = _a_items;
            SaveJson();
        }

        public void SetItemMap(ItemMap _a_itemMap)
        {
            itemMap = _a_itemMap;
        }

        public string GetItemInfo(string _a_place, string _a_title) {
            string _return = "nothing";

            if (jsonItemObject != null && jsonItemObject[_a_place] != null) {
                if(jsonItemObject[_a_place][_a_title] != null) {
                    _return = jsonItemObject[_a_place][_a_title].ToString();;
                }
            }

            return _return;
        }

        public void SaveJson()
        {
            // 파일 저장 경로 설정
            string _filePath = Path.Combine(Application.persistentDataPath, "items.json");

            // JSON으로 변환
            string _json = JsonUtility.ToJson(itemMap, true);

            // 파일에 저장
            File.WriteAllText(_filePath, _json);
        }
    }
}
