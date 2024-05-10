using Photon.Pun;
using UnityEngine;

public class Monster : MonoBehaviourPun
{
    [Header("몬스터 데이터")]
    [SerializeField]
    private MonsterData data;
    public MonsterData MonsterData { get { return data; } }

    // 몬스터 스테이터스
    private float currentHP;

    // 몬스터 유한 상태 기계
    private FSM fsm;

    private void Awake()
    {
        fsm = new FSM();
    }

    public void OnEnable()
    {
        // init stat
        currentHP = data.HP;

        // init fsm
        fsm.SetState(new ChaseState(this));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.AttackDistance);
    }

    /***************************************************************
    * [ 상태 제어 ]
    * 
    * 몬스터의 상태 제어
    ***************************************************************/

    public virtual void OnAttack(GameObject target)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            float damage = data.STR; // 데미지 공식

            Player player = target.GetComponent<Player>();
            player.OnTakeDamage(damage);
        }
    }

    public void OnTakeDamage(Player attacker, float damage)
    {
        // 공격 받았을 때
        float dmg = Mathf.Abs(damage);
        float def = data.DEF;
        float lastDamage = dmg / (dmg + def) * dmg;

        photonView.RPC(nameof(TakeDamage), RpcTarget.MasterClient, attacker.photonView.ViewID, lastDamage);
    }

    [PunRPC]
    protected void TakeDamage(int attackerViewID, float lastDamage)
    {
        GameObject attackerChr = PhotonView.Find(attackerViewID).gameObject;
        Player attacker = attackerChr.GetComponent<Player>();

        currentHP -= lastDamage;
        photonView.RPC(nameof(AsyncHP), RpcTarget.Others, currentHP);

        if (currentHP <= 0)
        {
            OnDead(attacker);
        }
    }

    [PunRPC]
    protected void AsyncHP(float currentHP)
    {
        this.currentHP = currentHP;
    }

    protected void OnDead(Player killPlayer)
    {
        // 몬스터 제거
        photonView.RPC(nameof(DestroyMob), RpcTarget.All);

        // 경험치 획득
        int exp = GetMonsterExp();
        photonView.RPC(nameof(GetExp), RpcTarget.All, exp);

        // 플레이어에게 킬 알림
        killPlayer.OnKilled();
    }

    [PunRPC]
    protected void DestroyMob()
    {
        Destroy(gameObject);

        // 몬스터 카운트 감소
        WaveData.Instance.OnKilledMob();
    }

    protected virtual int GetMonsterExp()
    {
        int exp = ExpResource.Instance.GetExp();

        return exp;
    }

    [PunRPC]
    protected void GetExp(int exp)
    {
        GameData.Instance.AddExp(exp);
    }

    /***************************************************************
    * [ 움직임 제어 ]
    * 
    * 몬스터의 움직임 제어
    ***************************************************************/

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            fsm.OnAction();
        }
    }
}