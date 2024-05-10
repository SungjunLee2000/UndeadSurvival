using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option/Level";
    private const string FILE_PATH = "Assets/Resources/Option/Level/LevelResource.asset";

    private static LevelResource _instance;
    public static LevelResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<LevelResource>("Option/Level/LevelResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<LevelResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<LevelResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("레벨별 필요 경험치표")]
    [SerializeField] 
    private TextAsset levelCSV;
    private Dictionary<int, int> levelTable;

    public int GetRequireExp(int level)
    {
        if (levelTable == null) InitTable();

        if (levelTable.ContainsKey(level))
        {
            return levelTable[level];
        }

        return int.MaxValue;
    }

    private void InitTable()
    {
        levelTable = new Dictionary<int, int>();

        if (levelCSV != null)
        {
            StringReader sr = new StringReader(levelCSV.text);

            string str;
            while ((str = sr.ReadLine()) != null)
            {
                if (str[0] != '#')
                {
                    string[] strs = str.Split(",");

                    int level = int.Parse(strs[0]);
                    int exp = int.Parse(strs[1]);

                    levelTable[level] = exp;
                }
            }
        }
    }
}