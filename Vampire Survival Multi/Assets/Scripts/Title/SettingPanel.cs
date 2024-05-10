using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    public GameObject settingPanel;

    public void closeSettingPanel()
    {
        settingPanel.SetActive(false);
    }
}
