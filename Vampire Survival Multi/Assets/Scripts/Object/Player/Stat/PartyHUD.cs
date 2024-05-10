using System.Collections.Generic;
using UnityEngine;

public class PartyHUD : MonoBehaviour
{
    [Header("체력바 목록")]
    [SerializeField] private List<GameObject> barList;

    private void Start()
    {
        PlayerResource resource = PlayerResource.Instance;

        CreateHUD(resource.PlayerDatas);
    }

    public void CreateHUD(List<PlayerData> playerDatas)
    {
        PlayerData localPlayerData = LocalPlayerData.Instance.PlayerData;

        for (int i = 0; i < barList.Count; i++)
        {
            if (playerDatas[i] != localPlayerData)
            {
                bool isPlaying = playerDatas[i].IsPlaying;

                barList[i].SetActive(isPlaying);
            }
            else
                barList[i].SetActive(false);
        }
    }
}