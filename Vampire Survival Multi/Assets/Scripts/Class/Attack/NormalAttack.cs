using UnityEngine;

public abstract class NormalAttack : ScriptableObject
{
    public float Cooldown
    {
         get
        {
            PlayerData stat = LocalPlayerData.Instance.PlayerData;

            return stat.AttackSpeed;
        }
    }

    public abstract void OnAction(Player attacker);
}