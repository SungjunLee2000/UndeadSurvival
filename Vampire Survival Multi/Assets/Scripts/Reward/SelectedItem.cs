using UnityEngine;

[RequireComponent (typeof(SelectedItemUI))]
public class SelectedItem : MonoBehaviour
{
    // 참조 컴포넌트
    private SelectedItemUI ui;
    private RewardManager manager;

    private ItemData item;

    private void Awake()
    {
        ui = GetComponent<SelectedItemUI>();
        manager = GetComponentInParent<RewardManager>();
    }

    public void SetItem(ItemData item)
    {
        this.item = item;

        ui.SetItem(item);
    }

    public void SelectItem()
    {
        PlayerEquip.Instance.EquipItem(item);

        manager.CloseWindow();
    }

    public void HandOverItem()
    {
        manager.HandOverItem(item);
    }
}