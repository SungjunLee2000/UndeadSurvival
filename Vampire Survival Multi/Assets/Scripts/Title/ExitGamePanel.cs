using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExitGamePanel : MonoBehaviour
{
    public GameObject exitGamePanel;

    public void openExitGamePanel()
    {
        exitGamePanel.SetActive(true);
    }

    public void closeExitGamePanel()
    {
        exitGamePanel.SetActive(false);
    }
    public void gameExit()
    {
        Application.Quit();
    }
}
