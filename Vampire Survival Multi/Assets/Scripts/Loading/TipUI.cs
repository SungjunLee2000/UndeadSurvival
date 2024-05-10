using TMPro;
using UnityEngine;

public class TipUI : MonoBehaviour
{
    private TipResource resource;

    [Header("참조 컴포넌트")]
    [SerializeField] private TextMeshProUGUI tipText;

    private void Start()
    {
        resource = TipResource.Instance;

        // 랜덤 팁 띄우기
        PostTip();
    }

    private void PostTip()
    {
        tipText.text = resource.GetRandomTip();
    }
}