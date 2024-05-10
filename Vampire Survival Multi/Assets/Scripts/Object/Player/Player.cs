using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("이벤트")]
    [SerializeField] private GameEvent deadEvent;

    // 플레이어 공통 옵션
    private PlayerResource playerOption;
    private float curDuration;

    private void Start()
    {
        playerOption = PlayerResource.Instance;
    }

    private void OnEnable()
    {
        // Reset HP
        playerData.HP = playerData.MaxHP;
    }

    private void Update()
    {
        if (curDuration > 0)
        {
            float time = Time.deltaTime;

            photonView.RPC(nameof(PassedTime), RpcTarget.All, time);
        }
    }

    [PunRPC]
    private void PassedTime(float time)
    {
        curDuration -= Time.deltaTime;
    }

    /***************************************************************
    * [ 상태 처리 ]
    * 
    * 다른 오브젝트와의 상호작용에 의한 상태 처리
    ***************************************************************/

    public void OnTakeDamage(float damage)
    {
        // 피격받은 지 일정시간이 경과하면 데미지
        if (curDuration <= 0)
        {
            // 공격 받았을 때
            float dmg = Mathf.Abs(damage);
            float def = playerData.DEF;
            float lastDamage = dmg / (dmg + def) * dmg;

            TakeDamage(lastDamage);
        }
    }

    public void OnNormalAttack(Monster monster, float damage)
    {
        monster.OnTakeDamage(this, damage);

        // 최종 데미지에 따른 체력 회복
        float recoverHP = damage * playerData.LifeSteal;

        HealHP(recoverHP);
    }

    public void OnSkillAttack(Monster monster, float damage)
    {
        monster.OnTakeDamage(this, damage);
    }

    [PunRPC]
    private void OnDead()
    {
        // 게임 데이터에 플레이어 값 저장
        GameData.Instance.AddDeadList(gameObject);

        // 플레이어 오브젝트 비활성화
        gameObject.SetActive(false);

        if (playerData.Player.IsLocal)
        {
            LocalPlayerData.Instance.IsDead = true;

            // Notify Dead Event
            deadEvent.NotifyUpdate();
        }
    }

    public void OnKilled()
    {

    }


    /***************************************************************
    * [ 체력 처리 ]
    * 
    * 회복 및 피해에 의한 체력 처리
    ***************************************************************/

    private void TakeDamage(float damage)
    {
        playerData.HP -= damage;

        photonView.RPC(nameof(UpdateHP), RpcTarget.All, playerData.HP);
        photonView.RPC(nameof(AsyncNoDamageTime), RpcTarget.All);

        if (playerData.HP <= 0)
        {
            // hp 값이 0이하면 죽음 처리
            photonView.RPC(nameof(OnDead), RpcTarget.All);
        }
    }

    [PunRPC]
    private void AsyncNoDamageTime()
    {
        curDuration = playerOption.NoDamageDuration;
    }

    private void HealHP(float recoverHP)
    {
        playerData.HP += recoverHP;

        photonView.RPC(nameof(UpdateHP), RpcTarget.Others, playerData.HP);
    }

    [PunRPC]
    private void UpdateHP(float currentHP)
    {
        playerData.HP = currentHP;
    }
}