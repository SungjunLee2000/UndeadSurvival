using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/GameData.asset";

    private static GameData _instance;
    public static GameData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<GameData>("Option/GameData");

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
                _instance = AssetDatabase.LoadAssetAtPath<GameData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<GameData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("참가 플레이어 정보")]
    [ReadOnly]
    [SerializeField]
    private List<GameObject> _playerList;
    public List<GameObject> PlayerList
    {
        private set { _playerList = value; }
        get
        {
            if (_playerList == null)
                _playerList = new List<GameObject>();

            return _playerList;
        }
    }
    [ReadOnly]
    [SerializeField]
    private List<GameObject> _cachedPlayerList;
    public List<GameObject> CachedPlayerList
    {
        private set {  _cachedPlayerList = value; }
        get
        {
             if (_cachedPlayerList == null)
                _cachedPlayerList = new List<GameObject>();

            return _cachedPlayerList;
        }
    }

    [ReadOnly]
    [SerializeField]
    private List<GameObject> _deadPlayerList;
    public List<GameObject> DeadPlayerList
    {
        private set { _deadPlayerList = value; }
        get
        {
            if (_deadPlayerList == null)
                _deadPlayerList = new List<GameObject>();

            return _deadPlayerList;
        }
    }

    public bool IsAllDead
    {
        get
        {
            int playerCount = _playerList.Count;
            int deadPlayerCount = _deadPlayerList.Count;

            return playerCount == deadPlayerCount;
        }
    }

    [Header("레벨 정보")]
    [SerializeField]
    private int _exp;
    public int Exp
    {
        private set { _exp = value; }
        get { return _exp; }
    }

    [SerializeField]
    private int _requireExp;
    public int RequireExp
    {
        private set { _requireExp = value; }
        get { return _requireExp; }
    }

    [SerializeField]
    private int _level;
    public int Level
    {
        private set { _level = value; }
        get { return _level; }
    }

    [Header("이벤트")]
    [SerializeField] private GameEvent expEvent;
    [SerializeField] private GameEvent levelUpEvent;
    
    public void InitData()
    {
        PlayerList = new List<GameObject>(CachedPlayerList);
        CachedPlayerList.Clear();
        DeadPlayerList.Clear();

        InitLevel();
    }

    public void AddPlayableChr(GameObject playableChr)
    {
        CachedPlayerList.Add(playableChr);
    }

    private void InitLevel()
    {
        // Init Level
        int initLevel = 1;

        Level = initLevel;
        Exp = 0;
        RequireExp = LevelResource.Instance.GetRequireExp(initLevel);

        // 경험치 변경 이벤트
        expEvent.NotifyUpdate();
    }

    public void AddExp(int exp)
    {
        if (_requireExp < int.MaxValue)
        {
            Exp += exp;

            // 레벨 및 레벨업에 필요한 경험치량 조정
            while (Exp >= _requireExp)
            {
                // 레벨업
                Level++;
                Exp -= RequireExp;

                // 필요 경험치량 조정
                RequireExp = LevelResource.Instance.GetRequireExp(Level);

                // 레벨업 이벤트
                levelUpEvent.NotifyUpdate();
            }

            // 경험치 변경 이벤트
            expEvent.NotifyUpdate();
        }
    }

    public void AddDeadList(GameObject player)
    {
        DeadPlayerList.Add(player);
    }

    public void ReviveAllPlayer()
    {
        DeadPlayerList.Clear();
    }
}