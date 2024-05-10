using UnityEngine;

public class PhotonUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private GameObject disconnectedAlert;
    [SerializeField] private GameObject connectingPanel;

    public void SetConnectiongPanel(bool isActive)
    {
        connectingPanel.SetActive(isActive);
    }

    public void SetDisconnectedAlert(bool isActive)
    {
        disconnectedAlert.SetActive(isActive);
    }
}