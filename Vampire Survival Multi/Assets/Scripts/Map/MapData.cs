using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/MapData.asset";

    private static MapData _instance;
    public static MapData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<MapData>("Option/MapData");

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
                _instance = AssetDatabase.LoadAssetAtPath<MapData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<MapData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("게임 맵")]
    [SerializeField] private Tilemap map;

    // 타일 맵 관련 변수
    private Vector2 _size;
    public Vector2 Size
    {
        get { return _size; }
    }

    private Vector2 _minPos;
    public Vector2 MinPos
    {
        get { return _minPos; }
    }

    private Vector2 _maxPos;
    public Vector2 MaxPos
    {
        get { return _maxPos; }
    }

    private Vector2 _pivot;
    public Vector2 Pivot
    {
        get { return _pivot; }
    }

    private void OnValidate()
    {
        Vector2 size = new Vector2(map.size.x, map.size.y);
        Vector2 origin = new Vector2(map.origin.x, map.origin.y);

        _size = size;

        _pivot = origin + size / 2;

        _minPos = _pivot - size / 2;
        _maxPos = _pivot + size / 2;
    }
}