using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerEquip : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Objects/Player/Data";
    private const string FILE_PATH = "Assets/Resources/Objects/Player/Data/PlayerEquip.asset";

    private static PlayerEquip _instance;
    public static PlayerEquip Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<PlayerEquip>("Objects/Player/Data/PlayerEquip");

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
                _instance = AssetDatabase.LoadAssetAtPath<PlayerEquip>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<PlayerEquip>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("장비 중인 아이템 리스트")]
    [SerializeField]
    private List<ItemData> _equipItems;
    public List<ItemData> EquipItems
    {
        get { return _equipItems; }
    }
    public ItemData LastItem
    {
        get
        {
            int index = _equipItems.Count - 1;

            return _equipItems[index];
        }
    }

    [Header("이벤트")]
    [SerializeField] private GameEvent equipEvent;

    public void InitEquips()
    {
        // 장비 초기화
        _equipItems.Clear();
    }

    public void EquipItem(ItemData item)
    {
        // 아이템 장비
        _equipItems.Add(item);

        // 이벤트 알림
        equipEvent.NotifyUpdate();
    }
}