using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomData
{
    public string title;
    public int maxPlayer;
    public int playerCount;
    public string type;
}

[RequireComponent(typeof(RoomUI))]
public class RoomManager : MonoBehaviour
{
    // 해당 방 ID
    private string id;
    private string password;

    [Header("참조 스크립트")]
    [SerializeField] private RoomUI ui;

    public delegate void SelectedCallback(string id, string roomPassword = "");
    private event SelectedCallback onSelectedCallback;

    public void InitRoomInfo(RoomInfo info)
    {
        RoomData room = new RoomData();

        room.title = (string)info.CustomProperties["RoomName"];
        room.maxPlayer = info.MaxPlayers;
        room.playerCount = info.PlayerCount;
        room.type = TypeToString((RoomType)info.CustomProperties["RoomType"]);

        // 방 정보 저장
        id = info.Name;
        password = (string)info.CustomProperties["Password"];

        // UI에 데이터 전달
        ui.InitRoom(room);
    }

    private string TypeToString(RoomType type)
    {
        switch (type)
        {
            case RoomType.Public: return "공개";
            case RoomType.Private: return "비공개";
            default: return "";
        }
    }

    public void SetClickHandler(SelectedCallback listener)
    {
        onSelectedCallback = listener;
    }

    public void OnEnterHandler()
    {
        onSelectedCallback?.Invoke(id, password);
    }
}