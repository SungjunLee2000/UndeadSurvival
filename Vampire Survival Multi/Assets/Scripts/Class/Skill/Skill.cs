using UnityEngine;

public abstract class Skill : ScriptableObject
{
    // 스킬 오브젝트 위치
    public const string SKILL_OBJECT_DIRECTORY = "Skills/Object/";

    // 플레이어 정보
    private PlayerData _casterData;
    protected PlayerData CasterData
    {
        get
        {
            if (_casterData == null)
                _casterData = LocalPlayerData.Instance.PlayerData;

            return _casterData;
        }
    }

    [Header("표시 정보")]
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon
    {
        get { return _icon; }
    }

    [SerializeField]
    [Multiline]
    private string _description;
    public string Description
    {
        get { return _description; }
    }

    [Header("스킬 상세 정보")]
    [SerializeField]
    private float _distance;
    public virtual float Distance
    {
        get { return _distance; }
    }

    [SerializeField]
    private float _cooldown;
    public float Cooldown
    {
        get { return _cooldown; }
    }

    public abstract void UseSkill(Player caster);
}