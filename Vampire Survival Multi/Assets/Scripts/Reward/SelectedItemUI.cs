using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemUI : MonoBehaviour
{
    [Header("관련 오브젝트")]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _image;

    public void SetItem(ItemData item)
    {
        _name.text = item.Name;
        _image.sprite = item.ItemImage;
    }
}