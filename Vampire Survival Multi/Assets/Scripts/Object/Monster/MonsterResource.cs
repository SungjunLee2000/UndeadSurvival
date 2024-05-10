using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Objects/Monster";
    private const string FILE_PATH = "Assets/Resources/Objects/Monster/MonsterResource.asset";

    private static MonsterResource _instance;
    public static MonsterResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<MonsterResource>("Objects/Monster/MonsterResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<MonsterResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<MonsterResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("몬스터 프리팹 종류")]
    [SerializeField] 
    private List<GameObject> _monsterList = new List<GameObject>();
    public List<GameObject> MonsterList
    {
        get { return _monsterList; }
    }
}