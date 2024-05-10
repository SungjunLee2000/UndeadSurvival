using Photon.Pun;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class StatManager : MonoBehaviourPun
{
    // 임시 스텟 UI
    [SerializeField] private TextMeshProUGUI testStatInfo;

    // 플레이어 스텟
    private PlayerData _playerStat;
    private PlayerData PlayerStat
    {
        set { _playerStat = value; }
        get
        {
            if (_playerStat == null)
                _playerStat = LocalPlayerData.Instance.PlayerData;

            return _playerStat;
        }
    }

    private void Start()
    {
        // 임시 UI 업데이트
        UpdateTmpUI();
    }

    /***************************************************************
     * [ 스테이터스 설정 ]
     * 
     * 상황에 따른 플레이어의 스테이터스(체력, 근력, 민첩, 방어력) 설정
     ***************************************************************/

    public void EquipItem()
    {
        // 적용 스텟 계산
        ItemData item = PlayerEquip.Instance.LastItem;

        UpdateStat(item);
    }

    private void UpdateStat(ItemData item)
    {
        photonView.RPC(nameof(SetStat), RpcTarget.MasterClient, PlayerStat.ID, item.ID);
    }

    [PunRPC]
    private void SetStat(int playerID, int itemID)
    {
        ItemData item = ItemResource.Instance.FindItem(itemID);
        PlayerData playerData = PlayerResource.Instance.PlayerDatas[playerID];

        // Update Max HP
        playerData.MaxHP = GetEquipHP(playerData, item);
        photonView.RPC(nameof(UpdateMaxHP), RpcTarget.Others, playerID, playerData.MaxHP);

        // Update STR
        playerData.STR = GetEquipSTR(playerData, item);
        photonView.RPC(nameof(UpdateSTR), RpcTarget.Others, playerID, playerData.STR);

        // Update DEF
        playerData.DEF = GetEquipDEF(playerData, item);
        photonView.RPC(nameof(UpdateDEF), RpcTarget.Others, playerID, playerData.DEF);

        // Update Attack Speed
        playerData.AttackSpeed = GetEquipAttackSpeed(playerData, item);
        photonView.RPC(nameof(UpdateAttackSpeed), RpcTarget.Others, playerID, playerData.AttackSpeed);

        // Update Move Speed
        playerData.MoveSpeed = GetEquipMoveSpeed(playerData, item);
        photonView.RPC(nameof(UpdateMoveSpeed), RpcTarget.Others, playerID, playerData.MoveSpeed);

        // Update Life Steal
        playerData.LifeSteal = GetEquipLifeSteal(playerData, item);
        photonView.RPC(nameof(UpdateLifeSteal), RpcTarget.Others, playerID, playerData.LifeSteal);
    }

    [PunRPC]
    private void UpdateMaxHP(int playerID, float maxHP)
    {
        PlayerData playerData = PlayerResource.Instance.PlayerDatas[playerID];

        playerData.MaxHP = maxHP;

        if (playerData.Player.IsLocal)
        {
            UpdateTmpUI();
        }
    }

    [PunRPC]
    private void UpdateSTR(int playerID, float str)
    {
        PlayerData playerData = PlayerResource.Instance.PlayerDatas[playerID];

        playerData.STR = str;

        if (playerData.Player.IsLocal)
        {
            UpdateTmpUI();
        }
    }

    [PunRPC]
    private void UpdateDEF(int playerID, float def)
    {
        PlayerData playerData = PlayerResource.Instance.PlayerDatas[playerID];

        playerData.DEF = def;

        if (playerData.Player.IsLocal)
        {
            UpdateTmpUI();
        }
    }

    [PunRPC]
    private void UpdateAttackSpeed(int playerID, float attackSpeed)
    {
        PlayerData playerData = PlayerResource.Instance.PlayerDatas[playerID];

        playerData.AttackSpeed = attackSpeed;

        if (playerData.Player.IsLocal)
        {
            UpdateTmpUI();
        }
    }

    [PunRPC]
    private void UpdateMoveSpeed(int playerID, float moveSpeed)
    {
        PlayerData playerData = PlayerResource.Instance.PlayerDatas[playerID];

        playerData.MoveSpeed = moveSpeed;

        if (playerData.Player.IsLocal)
        {
            UpdateTmpUI();
        }
    }

    [PunRPC]
    private void UpdateLifeSteal(int playerID, float lifeSteal)
    {
        PlayerData playerData = PlayerResource.Instance.PlayerDatas[playerID];

        playerData.LifeSteal = lifeSteal;

        if (playerData.Player.IsLocal)
        {
            UpdateTmpUI();
        }
    }

    private float GetEquipHP(PlayerData playerStat, ItemData item)
    {
        float hp = playerStat.MaxHP;

        hp = hp + item.HP;
        hp = hp + (hp * item.PercentHP);

        return hp;
    }

    private float GetEquipSTR(PlayerData playerStat, ItemData item)
    {
        float str = playerStat.STR;

        str = str + item.STR;
        str = str + (str * item.PercentSTR);

        return str;
    }

    private float GetEquipDEF(PlayerData playerStat, ItemData item)
    {
        float def = playerStat.DEF;

        def = def + item.DEF;
        def = def + (def * item.PercentDEF);

        return def;
    }

    private float GetEquipAttackSpeed(PlayerData playerStat, ItemData item)
    {
        float speed = 100.0f / (100 + item.AttackSpeed) * playerStat.AttackSpeed;

        return speed;
    }

    private float GetEquipMoveSpeed(PlayerData playerStat, ItemData item)
    {
        ClassData classData = LocalPlayerData.Instance.Class;

        float originSpeed = classData.MoveSpeed;
        float speed = playerStat.MoveSpeed + (originSpeed * item.MoveSpeed);

        return speed;
    }

    private float GetEquipLifeSteal(PlayerData playerStat, ItemData item)
    {
        float lifeSteal = playerStat.LifeSteal + item.ListSteal;

        return lifeSteal;
    }

    private void UpdateTmpUI()
    {
        testStatInfo.text =
            "<Status>" +
            "\r\nHP : " + PlayerStat.MaxHP +
            "\r\nSTR : " + PlayerStat.STR +
            "\r\nDEF : " + PlayerStat.DEF +
            "\r\nSpeed : " + PlayerStat.MoveSpeed +
            "\r\nAttack Speed : " + PlayerStat.AttackSpeed +
            "\r\nLife Steal : " + PlayerStat.LifeSteal;
    }
}