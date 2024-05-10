using UnityEngine;
using UnityEngine.UI;

public class PartyHealthUI : MonoBehaviour
{
    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData stat;

    [Header("참조 컴포넌트")]
    [SerializeField] private Image hpBar;

    // HP 구간 수치
    private float yellowLine = 0.5f;
    private float redLine = 0.1f;

    public void UpdateHP()
    {
        if (stat != null)
        {
            float percent = stat.HP / stat.MaxHP;

            hpBar.fillAmount = percent;
            SetHpColor(percent);
        }
    }

    private void SetHpColor(float percent)
    {
        if (percent <= redLine) hpBar.color = Color.red;
        else if (percent <= yellowLine) hpBar.color = Color.yellow;
        else hpBar.color = Color.green;
    }
}