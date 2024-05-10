using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviourPun
{
    [Header("참조 스크립트")]
    [SerializeField] private RewardUI ui;
    [SerializeField] private HandOverManager handOverManager;

    [Header("아이템 선택 관련 변수")]
    [SerializeField] private int selectCount;

    // 생성된 아이템 선택창
    private List<GameObject> selectWindows = new List<GameObject>();

    // 획득할 보상 수
    private int rewardCount = 0;

    // 선택을 끝낸 플레이어 수
    [SerializeField]
    private int completedPlayer = 0;

    public void GetReward()
    {
        // 이미 보상창이 열려 있다면 카운트 추가
        if (ui.IsActiveContainer()) rewardCount++;
        else
        {
            if (LocalPlayerData.Instance.IsDead == false)
            {
                // 살아있는 플레이어는 보상 받기
                Time.timeScale = 0.0f;

                OpenWindow();
            }
            else
            {
                // 죽은 플레이어는 대기 모드
                Time.timeScale = 0.0f;

                ui.SetContainer(true);
                CompletedReward();
            }
        }
    }

    private void OpenWindow()
    {
        ui.SetContainer(true);

        // 선택할 수 있는 아이템 랜덤 뽑기
        ItemData[] selectableItems = GetRandomItems(selectCount);

        // 아이템 선택창 띄우기
        List<GameObject> items = new List<GameObject>();

        for (int i = 0; i <selectCount; i++)
        {
            // 아이템 선택창 생성
            GameObject itemObj = ui.SetItem(selectableItems[i]);

            items.Add(itemObj);
        }

        selectWindows = items;

        // 첫번째 요소 선택 상태로 전환
        SelectObject(selectWindows[0]);
    }

    public void OpenHandOverWindow()
    {
        ui.SetContainer(false);
        ui.SetHandOverContainer(true);
    }

    public void CloseHandOverWindow()
    {
        ui.SetContainer(true);
        ui.SetHandOverContainer(false);
    }

    public void HandOverItem(ItemData item)
    {
        OpenHandOverWindow();

        handOverManager.SetHandOverItem(item);
    }

    private void SelectObject(GameObject SelectedItem)
    {
        Button button = SelectedItem.GetComponent<Button>();

        button.Select();
    }

    public void CloseWindow()
    {
        // 아이템 선택창 지우기
        foreach(GameObject prefab in selectWindows)
        {
            Destroy(prefab);
        }

        if (rewardCount > 0)
        {
            // 받을 보상이 남아있다면 창 다시 열기
            OpenWindow();

            rewardCount--;
        }
        else
        {
            CompletedReward();
        }
    }

    private void CompletedReward()
    {
        ui.SetStatePanel(true);
        UpdateUI();

        photonView.RPC(nameof(AddCompletedPlayer), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void AddCompletedPlayer()
    {
        completedPlayer++;
        UpdateUI();

        if (completedPlayer >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            completedPlayer = 0;

            ReturnToPlay();
        }
    }

    private void UpdateUI()
    {
        int remainPlayer = PhotonNetwork.CurrentRoom.PlayerCount - completedPlayer;

        ui.SetPlayerCount(remainPlayer);
    }

    private void ReturnToPlay()
    {
        ui.SetStatePanel(false);
        ui.SetContainer(false);

        // 게임 일시정지 해제
        Time.timeScale = 1.0f;
    }

    private ItemData[] GetRandomItems(int count)
    {
        ItemData[] items = new ItemData[count];

        // count 만큼 아이템 뽑기
        for(int i = 0;  i < count; i++)
        {
            items[i] = GetRandomItem();
        }

        return items;
    }

    private ItemData GetRandomItem()
    {
        // ItemResource 안의 사용가능한 아이템 중 하나 랜덤으로 뽑기
        List<ItemData> items = ItemResource.Instance.ItemDatas;

        int randomNum = Random.Range(0, items.Count);
        ItemData randomItem = items[randomNum];

        return randomItem;
    }
}