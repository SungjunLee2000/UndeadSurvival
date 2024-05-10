using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExpResource : ScriptableObject
{
    [System.Serializable]
    private class ExpData
    {
        public int getExp;
        public int probability;
    }

    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option/Level";
    private const string FILE_PATH = "Assets/Resources/Option/Level/ExpResource.asset";

    private static ExpResource _instance;
    public static ExpResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ExpResource>("Option/Level/ExpResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<ExpResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ExpResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("획득 경험치 확률표")]
    [SerializeField] private List<ExpData> expDataList;

    // 경험치 획득 정보
    private Dictionary<int, int> expTable;

    // 확률 난수 최대값
    private int maxNum;

    private void OnValidate()
    {
        UpdateExpInfo();
    }

    private void UpdateExpInfo()
    {
        // 본래 값 리셋
        maxNum = 0;
        expTable = new Dictionary<int, int>();

        foreach (ExpData data in expDataList)
        {
            maxNum += data.probability;
            expTable[maxNum] = data.getExp;
        }
    }

    public int GetExp()
    {
        int randomNum = Random.Range(0, maxNum);
        
        foreach (int probability in expTable.Keys)
        {
            if (randomNum < probability)
            {
                return expTable[probability];
            }
        }

        return 0;
    }
}