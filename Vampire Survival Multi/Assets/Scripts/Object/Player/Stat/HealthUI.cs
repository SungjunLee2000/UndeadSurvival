using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private Transform hpTranceform;
    [SerializeField] private SpriteRenderer hpBar;

    // HP 구간 수치
    private float yellowLine = 0.5f;
    private float redLine = 0.1f;

    public void UpdateHP(float currentHP, float maxHP)
    {
        float percent = currentHP / maxHP;

        hpTranceform.localScale = new Vector3(percent, 1, 1);
        SetHpColor(percent);
    }

    private void SetHpColor(float percent)
    {
        if (percent <= redLine) hpBar.color = Color.red;
        else if (percent <= yellowLine) hpBar.color = Color.yellow;
        else hpBar.color = Color.green;
    }
}