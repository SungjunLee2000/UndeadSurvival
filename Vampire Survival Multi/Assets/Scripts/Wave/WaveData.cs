using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class WaveData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option/Wave";
    private const string FILE_PATH = "Assets/Resources/Option/Wave/WaveData.asset";

    private static WaveData _instance;
    public static WaveData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<WaveData>("Option/Wave/WaveData");

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
                _instance = AssetDatabase.LoadAssetAtPath<WaveData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<WaveData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("현재 웨이브 정보")]
    [SerializeField]
    private bool _isRunning;
    public bool IsRunning
    {
        private set { _isRunning = value; }
        get { return _isRunning; }
    }

    [ReadOnly]
    [SerializeField]
    private int _waveLevel;
    public int WaveLevel
    {
        private set { _waveLevel = value; }
        get { return _waveLevel; }
    }

    [ReadOnly]
    [SerializeField]
    private float _remainTime;
    public float RemainTime
    {
        private set { _remainTime = value; }
        get { return _remainTime; }
    }

    [ReadOnly]
    [SerializeField]
    private int _mobCount;
    public int MobCount
    {
        private set { _mobCount = value; }
        get { return _mobCount; }
    }

    public bool IsLastWave
    {
        get
        {
            WaveResource resource = WaveResource.Instance;

            return WaveLevel >= resource.MaxLevel;
        }
    }

    public bool IsBossWave
    {
        get
        {
            WaveResource resource = WaveResource.Instance;

            return resource.IsBossWave(WaveLevel);
        }
    }

    // 소환할 몬스터 목록
    private Queue<GameObject> waveMobs;

    public void InitData()
    {
        WaveResource resource = WaveResource.Instance;

        int startLevel = 1;

        WaveLevel = startLevel;
        RemainTime = resource.GetWaveTime(startLevel);
        MobCount = resource.GetWaveMobs(startLevel).Count;

        // 소환할 몬스터 목록 추가
        List<GameObject> mobList = resource.GetWaveMobs(startLevel);

        waveMobs = new Queue<GameObject>(mobList);
    }

    public GameObject GetMob()
    {
        if (waveMobs.Count > 0)
        {
            return waveMobs.Dequeue();
        }
        else
            return null;
    }

    public void NextWave()
    {
        WaveResource resource = WaveResource.Instance;

        int maxLevel = resource.MaxLevel;

        if (maxLevel > WaveLevel)
        {
            // 웨이브 레벨 증가
            int currentLevel = ++WaveLevel;

            // 웨이브 정보 갱신
            RemainTime = resource.GetWaveTime(currentLevel);
            MobCount += resource.GetWaveMobs(currentLevel).Count;

            // 소환할 몬스터 목록 추가
            List<GameObject> mobList = resource.GetWaveMobs(currentLevel);

            AddMobs(mobList);
        }
        else
        {
            // 최종 웨이브 이후엔 웨이브 종료
            WaveStop();
        }
    }

    private void AddMobs(List<GameObject> addMobList)
    {
        if (waveMobs != null && waveMobs.Count > 0)
        {
            foreach (GameObject mob in addMobList)
            {
                waveMobs.Enqueue(mob);
            }
        }
        else
        {
            waveMobs = new Queue<GameObject>(addMobList);
        }
    }

    public void WaveStart()
    {
        IsRunning = true;
    }

    public void WaveStop()
    {
        IsRunning = false;
    }

    public void OnKilledMob()
    {
        MobCount--;
    }

    public void PassedWaveTime(float time)
    {
        RemainTime -= time;
    }
}