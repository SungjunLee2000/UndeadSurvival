using TMPro;
using UnityEngine;

public class ConfirmUI : MonoBehaviour
{
    [Header("구성 오브젝트")]
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private TextMeshProUGUI confirmContent;

    public void SetActiveConfirm(bool isActive)
    {
        confirmPanel.SetActive(isActive);
    }

    public void SetMessage(string msg)
    {
        confirmContent.text = msg;
    }
}