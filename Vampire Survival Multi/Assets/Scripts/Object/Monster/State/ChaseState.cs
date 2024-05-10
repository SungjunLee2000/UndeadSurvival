using System;
using UnityEngine;

public class ChaseState : IMonsterState
{
    private Monster _monster;
    private SpriteRenderer spriter;

    public ChaseState(Monster monster)
    {
        _monster = monster;
        spriter = _monster.GetComponent<SpriteRenderer>();
    }

    public void OnUpdate(FSM fsm)
    {
        GameObject target = DetectedPlayer();
        
        
        if (target != null)
        {
            Vector2 pos = target.transform.position;

            // 몬스터와 플레이어 간의 거리 계산
            float distance = Vector2.Distance(pos, _monster.transform.position);
            if (distance <= _monster.MonsterData.AttackDistance)
            {
                // 몬스터가 공격 가능한 범위까지 접근하면 공격
                fsm.SetState(new AttackState(_monster, target));
            }
            else
            {
                // 특정 플레이어를 향해 이동
                MoveToPlayer(pos);
            }
        }
    }

    private GameObject DetectedPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 가장 가까운 플레이어 위치 리턴
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(player.transform.position, _monster.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    private void MoveToPlayer(Vector2 playerPos)
    {
        float speed = _monster.MonsterData.MoveSpeed * Time.deltaTime;

        _monster.transform.position = Vector2.MoveTowards(_monster.transform.position, playerPos, speed);
        spriter.flipX = _monster.transform.position.x < 0;
    }

    public void OnEnterState() { }

    public void OnExitState(FSM fsm) { }
}