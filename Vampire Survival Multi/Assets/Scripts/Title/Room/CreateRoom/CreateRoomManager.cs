using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent(typeof(CreateRoomUI))]
public class CreateRoomManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private CreateRoomUI ui;

    public void CreateNewRoom()
    {
        // 방 만들기
        RoomOptions options = GetCreateOptions();
        PhotonNetwork.CreateRoom(null, options, null);
    }

    private RoomOptions GetCreateOptions()
    {
        Hashtable properties = new Hashtable();

        string[] keys = new string[3] { "RoomName", "RoomType", "Password" };

        properties.Add(keys[0], ui.GetRoomName());
        properties.Add(keys[1], ui.GetRoomType());
        properties.Add(keys[2], ui.GetPassword());

        RoomOptions options = new RoomOptions();

        options.MaxPlayers = ui.GetMaxPlayer();
        options.IsVisible = (properties[keys[1]].Equals(RoomType.Hidden) == false);
        options.CustomRoomProperties = properties;
        options.CustomRoomPropertiesForLobby = keys;

        return options;
    }

    public void EnablePassword()
    {
        RoomType type = ui.GetRoomType();

        if (type.Equals(RoomType.Private))
        {
            // 방 타입이 비공개이면 비밀번호 입력칸 활성화
            ui.SetActivePwField(true);
        }
        else
        {
            // 그 외엔 비활성화
            ui.SetActivePwField(false);
        }
    }

    public void EnableCreateButton()
    {
        RoomType type = ui.GetRoomType();

        if (type.Equals (RoomType.Private))
        {
            bool isActive = ui.IsInputPw && ui.IsInputTitle;

            ui.SetCreateButton(isActive);
        }
        else
        {
            bool isActive = ui.IsInputTitle;

            ui.SetCreateButton(isActive);
        }
    }
}