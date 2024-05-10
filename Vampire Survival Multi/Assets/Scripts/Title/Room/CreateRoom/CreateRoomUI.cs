using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    [Header("참조 오브젝트")]
    [SerializeField] private Button createButton;

    [Header("방 생성 정보")]
    [SerializeField] private TMP_Dropdown maxPlayerDropdown;
    [SerializeField] private TMP_Dropdown roomTypeDropdown;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField titleInput;

    public bool IsInputPw
    {
        get { return passwordInput.text != ""; }
    }

    public bool IsInputTitle
    {
        get { return titleInput.text != ""; }
    }

    public string GetRoomName()
    {
        return titleInput.text;
    }

    public int GetMaxPlayer()
    {
        int index = maxPlayerDropdown.value;
        string maxPlayer = maxPlayerDropdown.options[index].text;

        return int.Parse(maxPlayer);
    }

    public RoomType GetRoomType()
    {
        int index = roomTypeDropdown.value;

        return (RoomType)index;
    }

    public string GetPassword()
    {
        return passwordInput.text;
    }

    public void SetActivePwField(bool isActive)
    {
        passwordInput.text = ""; // 상태 변경 시 안에 내용 날리기
        passwordInput.interactable = isActive;
    }

    public void SetCreateButton(bool isActive)
    {
        createButton.interactable = isActive;
    }
}