using UnityEngine;

[RequireComponent(typeof(PasswordUI))]
public class PasswordManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private PasswordUI ui;

    public delegate void PasswordCallback();
    private event PasswordCallback checkedCallback;

    public void InputPassword(string roomPassword, PasswordCallback listener)
    {
        // 비밀번호 확인 버튼을 눌렀을 때 실행
        checkedCallback = () => 
        {
            string inputPw = ui.GetInputPassword();

            if (roomPassword == inputPw)
            {
                // 비밀번호 일치 시 콜백 실행
                listener?.Invoke();
            }
            else
            {
                // 불일치 시 경고문 띄우기
                ui.SetWrongPwPanel(true);
            }
        };

        // 비밀번호 패널 띄우기
        ui.SetPasswordPanel(true);
    }

    public void OnCheckHandler()
    {
        checkedCallback?.Invoke();

        ui.SetPasswordPanel(false);
        ui.ClearInputField();
    }
}