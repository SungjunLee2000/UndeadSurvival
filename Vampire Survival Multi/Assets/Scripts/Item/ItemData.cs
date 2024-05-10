using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    [Header("아이템 정보")]
    [SerializeField]
    private int _id;
    public int ID
    {
        get { return _id; }
    }

    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private Sprite _itemImage;
    public Sprite ItemImage
    {
        get { return _itemImage; }
    }

    [SerializeField]
    [TextArea]
    private string _description;
    public string Description
    {
        get { return _description; }
    }

    [Header("아이템 효과")]
    [SerializeField]
    private int _healthPoint;
    public int HP
    {
        get { return _healthPoint; }
    }
    [SerializeField]
    private float _percentHP;
    public float PercentHP
    {
        get { return _percentHP / 100; }
    }

    [SerializeField]
    private int _strength;
    public int STR
    {
        get { return _strength; }
    }
    [SerializeField]
    private float _percentSTR;
    public float PercentSTR
    {
        get { return _percentSTR / 100; }
    }

    [SerializeField]
    private int _defensive;
    public int DEF
    {
        get { return _defensive; }
    }
    [SerializeField]
    private float _percentDEF;
    public float PercentDEF
    {
        get { return _percentDEF / 100; }
    }

    [SerializeField]
    private float _moveSpeed;
    public float MoveSpeed
    {
        get { return _moveSpeed / 100; }
    }

    [SerializeField]
    private float _attackSpeed;
    public float AttackSpeed
    {
        get { return _attackSpeed; }
    }

    [SerializeField]
    private float _lifeSteal;
    public float ListSteal
    {
        get { return _lifeSteal / 100; }
    }
}