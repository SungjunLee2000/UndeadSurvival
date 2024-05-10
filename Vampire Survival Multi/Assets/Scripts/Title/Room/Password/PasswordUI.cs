using TMPro;
using UnityEngine;

public class PasswordUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private GameObject wrongPwPanel;
    [SerializeField] private TMP_InputField passwordInput;

    public void SetPasswordPanel(bool isActive)
    {
        passwordPanel.SetActive(isActive);
    }

    public void SetWrongPwPanel(bool isActive)
    {
        wrongPwPanel.SetActive(isActive);
    }

    public string GetInputPassword()
    {
        return passwordInput.text;
    }

    public void ClearInputField()
    {
        passwordInput.text = "";
    }
}