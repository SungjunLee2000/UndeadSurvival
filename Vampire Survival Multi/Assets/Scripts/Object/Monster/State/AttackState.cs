using UnityEngine;

public class AttackState : IMonsterState
{
    private Monster _monster;
    private GameObject _target;

    public AttackState(Monster monster, GameObject player)
    {
        _monster = monster;
        _target = player;
    }

    public void OnUpdate(FSM fsm)
    {
        // 선 공격
        OnAttack(_target);

        // 해당 타겟이 여전히 공격 가능한 범위 내에 있는지 판별
        if (_target.activeSelf == true)
        {
            Vector2 mobPos = _monster.transform.position;
            Vector2 playerPos = _target.transform.position;

            // 몬스터와 플레이어 간의 거리 계산
            float distance = Vector2.Distance(playerPos, mobPos);
            if (distance > _monster.MonsterData.AttackDistance)
            {
                // 거리가 멀어지면 다시 추적
                fsm.SetState(new ChaseState(_monster));
            }
        }
        else
        {
            // 해당 타겟이 죽은 상태면 추적 상태로 전환
            fsm.SetState(new ChaseState(_monster));
        }
    }

    private void OnAttack(GameObject target)
    {
        // 플레이어 공격
        _monster.OnAttack(target);
    }

    public void OnEnterState() { }

    public void OnExitState(FSM fsm) { }
}