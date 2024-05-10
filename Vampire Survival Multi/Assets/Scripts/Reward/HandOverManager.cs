using Photon.Pun;
using UnityEngine;

public class HandOverManager : MonoBehaviourPun
{
    [Header("참조 스크립트")]
    [SerializeField] private RewardManager manager;
    [SerializeField] private RewardUI ui;

    private ItemData handOverItem;

    private void Start()
    {
        InitPlayerPanel();
    }

    private void InitPlayerPanel()
    {
        PlayerResource resource = PlayerResource.Instance;

        for (int i = 0; i < resource.PlayerDatas.Count; i++)
        {
            PlayerData playerData = resource.PlayerDatas[i];

            if (playerData.IsPlaying)
            {
                // 본인을 제외한 플레이어 목록 띄우기
                bool isActive = !playerData.Player.IsLocal;

                ui.SetActivePlayer(i, isActive);
            }
            else
            {
                // 플레이 중이 아닌 캐릭터 제외
                ui.SetActivePlayer(i, false);
            }

        }
    }

    public void SetHandOverItem(ItemData handOverItem)
    {
        this.handOverItem = handOverItem;
    }

    public void OnClickReturn()
    {
        manager.CloseHandOverWindow();
    }

    public void OnClickPlayer(PlayerData playerData)
    {
        photonView.RPC(nameof(HandOverItem), RpcTarget.MasterClient, playerData.Player, handOverItem.ID);

        manager.CloseHandOverWindow();
        manager.CloseWindow();
    }

    [PunRPC]
    private void HandOverItem(Photon.Realtime.Player getPlayer, int itemID)
    {
        photonView.RPC(nameof(GetReward), getPlayer, itemID);
    }

    [PunRPC]
    private void GetReward(int itemID)
    {
        Debug.Log(itemID);
        ItemData item = ItemResource.Instance.FindItem(itemID);
        PlayerEquip.Instance.EquipItem(item);
    }
}