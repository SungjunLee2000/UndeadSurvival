using Photon.Realtime;
using TMPro;
using UnityEngine;

public enum RoomType
{
    Public,
    Private,
    Hidden
}
public class RoomUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI maxPlayer;
    [SerializeField] private TextMeshProUGUI playerCount;
    [SerializeField] private TextMeshProUGUI roomType;

    public void InitRoom(RoomData room)
    {
        title.text = room.title;
        maxPlayer.text = room.maxPlayer.ToString();
        playerCount.text = room.playerCount.ToString();
        roomType.text = room.type;
    }
}