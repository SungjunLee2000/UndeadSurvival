using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TipResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option/Tip";
    private const string FILE_PATH = "Assets/Resources/Option/Tip/TipResource.asset";

    private static TipResource _instance;
    public static TipResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<TipResource>("Option/Tip/TipResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<TipResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<TipResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("CSV 파일")]
    [SerializeField] private TextAsset csvFile;

    private List<string> _tips;
    private List<string> Tips
    {
        get
        {
            if (_tips == null || _tips.Count <= 0)
            {
                ReloadResource();
            }

            return _tips;
        }
    }

    [ContextMenu ("Reload Tips")]
    public void ReloadResource()
    {
        StringReader sr = new StringReader(csvFile.text);
        List<string> csvLines = new List<string>();

        string str;
        while ((str = sr.ReadLine()) != null)
        {
            if (str[0] != '#')
            {
                string[] strs = str.Split(',');

                csvLines.Add(strs[0]);
            }
        }

        _tips = csvLines;
    }

    public string GetRandomTip()
    {
        int randomIndex = Random.Range(0, Tips.Count);

        return Tips[randomIndex];
    }
}