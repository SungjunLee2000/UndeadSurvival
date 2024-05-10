using UnityEngine;

[RequireComponent (typeof(ConfirmUI))]
public class Confirm : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private ConfirmUI ui;

    public delegate void ConfirmCallback();
    private ConfirmCallback yesCallback;
    private ConfirmCallback noCallback;

    public void OnActive(string msg, ConfirmCallback yesCallback = null, ConfirmCallback noCallback = null)
    {
        this.yesCallback = () =>
        {
            yesCallback?.Invoke();

            ui.SetActiveConfirm(false);
        };

        this.noCallback = () =>
        {
            noCallback?.Invoke();

            ui.SetActiveConfirm(false);
        };

        // Confirm 띄우기
        ui.SetActiveConfirm(true);
        ui.SetMessage(msg);
    }

    public void OnClickYes()
    {
        yesCallback?.Invoke();
    }

    public void OnClickNo()
    {
        noCallback?.Invoke();
    }
}