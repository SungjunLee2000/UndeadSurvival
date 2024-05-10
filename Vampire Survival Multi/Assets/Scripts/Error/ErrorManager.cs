using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ErrorUI))]
public class ErrorManager : MonoBehaviour
{
    // 방 입장 에러 코드
    private const short GAME_FULL = 32765;
    private const short GAME_CLOSED = 32764;
    private const short GAME_DOES_NOT_EXIST = 32758;

    [Header("참조 스크립트")]
    [SerializeField] private ErrorUI ui;

    public void AlertError(short returnCode, string message)
    {
        switch (returnCode)
        {
            case GAME_FULL:
                AlertGameFull();
                break;

            default:
                AlertOtherError(returnCode, message);
                break;
        }
    }

    private void AlertGameFull()
    {
        ui.AlertError("방 접속에 실패하였습니다!", "이미 방 안에 인원수가 가득 찼습니다!");
    }

    private void AlertOtherError(short returnCode, string message)
    {
        string content = $"Error Code ${returnCode}: ${message}";
        ui.AlertError("예기치 못한 오류가 발생했습니다!", content);
    }
}