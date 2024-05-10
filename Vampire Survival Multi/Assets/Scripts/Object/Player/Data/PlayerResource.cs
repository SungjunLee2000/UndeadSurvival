using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Objects/Player/Data";
    private const string FILE_PATH = "Assets/Resources/Objects/Player/Data/PlayerResource.asset";

    private static PlayerResource _instance;
    public static PlayerResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<PlayerResource>("Objects/Player/Data/PlayerResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<PlayerResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<PlayerResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("플레이어 리소스")]
    [SerializeField]
    private List<PlayerData> _playerDatas;
    public List<PlayerData> PlayerDatas
    {
        get { return _playerDatas; }
    }
    public int maxPlayerNum
    {
        get { return _playerDatas.Count; }
    }

    [Header("플레이어 공통 옵션")]
    [SerializeField]
    private float _noDamageDuration;
    public float NoDamageDuration
    {
        get { return _noDamageDuration; }
    }
    
    [SerializeField]
    private float _knockbackArea;
    public float KnockbackArea
    {
        get { return _knockbackArea; }
    }

    [SerializeField]
    private float _knockbackForce;
    public float KnockbackForce
    {
        get { return _knockbackForce; }
    }
}