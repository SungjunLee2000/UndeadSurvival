using UnityEngine;

public class HUD : MonoBehaviour
{
    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData playerData;

    [Header("참조 스크립트")]
    [SerializeField] private HealthUI healthUI;

    private void Start()
    {
        if (playerData.Player.IsLocal == false)
        {
            // 본인의 플레이어블 캐릭터가 아니라면 HUD 비활성화
            gameObject.SetActive(false);
        }
    }

    public void UpdateHP()
    {
        float currentHP = playerData.HP;
        float maxHP = playerData.MaxHP;

        healthUI.UpdateHP(currentHP, maxHP);
    }
}