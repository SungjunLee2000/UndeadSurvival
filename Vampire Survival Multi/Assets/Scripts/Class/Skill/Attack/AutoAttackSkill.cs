using UnityEngine;

public abstract class AutoAttackSkill : Skill
{
    [SerializeField]
    private bool _isPiercing;
    public bool IsPiercing
    {
        get { return _isPiercing; }
    }

    private float _addDistance;
    public float AddDistance
    {
        set { _addDistance = value; }
    }

    public override float Distance
    {
        get
        {
            float distance = base.Distance;

            return distance + (distance * _addDistance);
        }
    }
}