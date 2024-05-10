using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("스포너")]
    [SerializeField] private List<Spawner> spawners;
    [SerializeField] private PlayerChecker checker;

    // 스폰 가능 여부
    public bool IsSpawnable
    {
        get
        {
            if (checker.playerInArea == false) return true;

            foreach (Spawner spawner in spawners)
            {
                if (spawner.IsSpawnable)
                    return true;
            }

            return false;
        }
    }

    public void SpawnMob(GameObject mob)
    {
        bool isSpawnable = false;
        Spawner spawner = null;

        // 스폰 가능한 스포너가 나올 때까지 반복
        while(isSpawnable == false)
        {
            int num = Random.Range(0, spawners.Count);

            spawner = spawners[num];
            isSpawnable = checker.playerInArea == false || spawner.IsSpawnable;
        }

        spawner.SpawnMob(mob);
    }

    private void OnDrawGizmos()
    {
        SpawnerResource resource = SpawnerResource.Instance;

        // 플레이어 탐지 범위
        float distance = resource.Distance;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distance);

        // 스폰 가능한 스포너
        foreach(Spawner spawner in spawners)
        {
            if (checker.playerInArea == false || spawner.IsSpawnable)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.blue;
            }

            Gizmos.DrawWireCube(spawner.Pivot, spawner.SpawnArea);
        }
    }
}