using TMPro;
using UnityEngine;

public class ErrorUI : MonoBehaviour
{
    [Header("참조 오브젝트")]
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI alertTitle;
    [SerializeField] private TextMeshProUGUI alertContent;

    public void CloseAlert()
    {
        errorPanel.SetActive(false);
    }

    public void AlertError(string title, string message)
    {
        alertTitle.text = title;
        alertContent.text = message;

        // 패널 띄우기
        errorPanel.SetActive(true);
    }
}