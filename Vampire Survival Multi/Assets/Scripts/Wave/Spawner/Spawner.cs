using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("스폰 범위")]
    [SerializeField] 
    private Vector2 _spawnArea;
    public Vector2 SpawnArea
    {
        get { return _spawnArea; }
    }
    public Vector2 Pivot
    {
        get { return transform.position; }
    }
    private Vector2 minPos // 소환될 수 있는 최소 범위
    {
        get
        {
            Vector2 minVec = Pivot - SpawnArea / 2;

            return minVec;
        }
    }
    private Vector2 maxPos // 소환될 수 있는 최대 범위
    {
        get
        {
            Vector2 maxVec = Pivot + SpawnArea / 2;

            return maxVec;
        }
    }

    // 스폰 가능 조건
    [ReadOnly]
    [SerializeField]
    private int playerCount = 0;

    public bool IsSpawnable
    {
        get
        {
            // 범위 안에 플레이어가 없고
            if (playerCount <= 0)
            {
                MapData mapData = MapData.Instance;

                Vector2 maxVec = maxPos;
                Vector2 minVec = minPos;
                Vector2 mapMaxPos = mapData.MaxPos;
                Vector2 mapMinPos = mapData.MinPos;

                // 소환 범위 일부가 경계선 안인 경우
                return (maxVec.x > mapMinPos.x && maxVec.y > mapMinPos.y)
                    && (minVec.x < mapMaxPos.x && minVec.y < mapMaxPos.y);
            }

            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCount--;
        }
    }

    public void SpawnMob(GameObject mob)
    {
        int horizontalRange = (int)_spawnArea.x / 2;
        int verticalRange = (int)_spawnArea.y / 2;

        int randomX = Random.Range(-horizontalRange, horizontalRange);
        int randomY = Random.Range(-verticalRange, verticalRange);

        Vector2 pivot = transform.position;
        Vector2 spawnPos = pivot + new Vector2(randomX, randomY);

        PhotonNetwork.Instantiate(mob.name, spawnPos, Quaternion.identity);
    }
}