using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    // 참조 컴포넌트
    private CircleCollider2D circleCollider;

    // 플레이어 감지
    private int playerCount = 0;
    public bool playerInArea
    {
        get { return playerCount > 0; }
    }

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        float distance = SpawnerResource.Instance.Distance;
        circleCollider.radius = distance;
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
}