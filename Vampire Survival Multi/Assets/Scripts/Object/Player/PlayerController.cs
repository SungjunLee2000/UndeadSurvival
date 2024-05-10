using Photon.Pun;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviourPun, IControlState
{
    [Header("�÷��̾� ������")]
    [SerializeField] private PlayerData playerData;

    [Header("���� ��ũ��Ʈ")]
    [SerializeField] private Player player;
    [SerializeField] private CameraManager cameraManager;

    // �̵� ���� ����
    private Rigidbody2D rigid;
    private Vector2 moveVec;
    private SpriteRenderer spriter;
    private Animator anim;
    

    // ��ų ����
    private Skill autoAttack;
    private float attackCooldown;
    private Skill skill;
    private float skillCooldown;

    private void Awake()
    {
        spriter=GetComponent<SpriteRenderer>();
        anim=GetComponent<Animator>();

        if (playerData.IsPlaying == false)
        {
            Destroy(gameObject);

            return;
        }

        // ���� ���� ����
        Photon.Realtime.Player owner = playerData.Player;

        if (owner != null && owner.IsLocal)
        {
            // �ش� ĳ���Ͱ� �ڽ��� ���̶�� ���� ���� ����
            photonView.TransferOwnership(owner);
        }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        // Init Position In PlayerData
        playerData.Position = transform.position;

        if (playerData.Player.IsLocal)
        {
            // ��� �ʱ� ����
            PlayerEquip.Instance.InitEquips();

            // Set Tracker
            InitCamera();

            // Init Skill & Normal Attack
            InitSkill();
        }
    }

    private void InitCamera()
    {
        cameraManager.InitPlayer(gameObject);
    }

    private void InitSkill()
    {
        ClassData classData = LocalPlayerData.Instance.Class;

        autoAttack = classData.PassiveSkill;
        attackCooldown = 0;

        skill = classData.ActiveSkill;
        skillCooldown = 0;
    }

    private void Update()
    {
        if (playerData.Player.IsLocal)
        {
            // �⺻ ����
            OnNormalAttack();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // �⺻ ���� �� ��ų ��ٿ�
            CooldownSkills();
        }
    }

    private void OnNormalAttack()
    {
        if (autoAttack != null && attackCooldown <= 0)
        {
            autoAttack.UseSkill(player);

            // ���� �� ��ٿ� ����
            attackCooldown = playerData.AttackSpeed;
        }
    }

    private void CooldownSkills()
    {
        float time = Time.deltaTime;

        photonView.RPC(nameof(PassedAttackCooldown), photonView.Owner, time);
        photonView.RPC(nameof(PassedSkillCooldown), photonView.Owner, time);
    }

    [PunRPC]
    private void PassedAttackCooldown(float time)
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= time;
        }
    }

    [PunRPC]
    private void PassedSkillCooldown(float time)
    {
        if (skillCooldown > 0)
        {
            skillCooldown -= time;
        }
    }

    private void OnEnable()
    {
        if (playerData.Player.IsLocal)
        {
            // Set Control State
            ControlContext.Instance.SetState(this);
        }
    }

    /***************************************************************
    * [ Ű �Է� ]
    * 
    * Ű �Է¿� ���� �ൿ ����
    ***************************************************************/

    public void OnControlKeyPressed()
    {
        OnMoveKeyPressed();
        OnSkillKeyPressed();

        if (Input.GetKeyDown(KeyCode.J))
        {
            KnockbackMonsters();
        }
    }

    private void OnMoveKeyPressed()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveVec = new Vector2(horizontalInput, verticalInput);
    }

    private void OnSkillKeyPressed()
    {
        if (Input.GetButtonDown("Skill"))
        {
            if (skill != null && skillCooldown <= 0)
            {
                skill.UseSkill(player);

                // ��ų ��� �� ��ٿ� ����
                skillCooldown = skill.Cooldown;
            }
        }
    }

    private void FixedUpdate()
    {
        // Ű �Է¿� ���� �÷��̾� ������
        Vector2 movement = moveVec.normalized * playerData.MoveSpeed * Time.deltaTime;

        rigid.MovePosition(rigid.position + movement);

        // �÷��̾� ��ǥ ����
        playerData.Position = transform.position;

        anim.SetFloat("Speed", movement.magnitude);
        if (movement.x != 0)
        {
            spriter.flipX = movement.x < 0;
        }
    }

    public void UpdatePlayerPos()
    {
        if (playerData.Player.IsLocal)
        {
            transform.position = playerData.Position;
        }
    }

    /***************************************************************
    * [ ��Ȱ ]
    * 
    * ��Ȱ �� �ֺ� ���� ��ġ��
    ***************************************************************/

    public void KnockbackMonsters()
    {
        PlayerResource resource = PlayerResource.Instance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, resource.KnockbackArea);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

                rb.AddForce(direction * resource.KnockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}