using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Items";
    private const string FILE_PATH = "Assets/Resources/Items/ItemResource.asset";

    private static ItemResource _instance;
    public static ItemResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ItemResource>("Items/ItemResource");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }

                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<ItemResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ItemResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("사용 가능한 아이템 리스트")]
    [SerializeField]
    private List<ItemData> _itemDatas;
    public List<ItemData> ItemDatas
    {
        get { return _itemDatas; }
    }

    // 아이템 목록
    private Dictionary<int, ItemData> itemTable = new Dictionary<int, ItemData>();

    [ContextMenu("Reload Items")]
    private void OnValidate()
    {
        foreach (ItemData item in _itemDatas)
        {
            itemTable[item.ID] = item;
            Debug.Log($"Add Item {item.Name}(ID:{item.ID})");
        }
    }

    public ItemData FindItem(int id)
    {
        if (itemTable.ContainsKey(id))
        {
            return itemTable[id];
        }

        return null;
    }
}