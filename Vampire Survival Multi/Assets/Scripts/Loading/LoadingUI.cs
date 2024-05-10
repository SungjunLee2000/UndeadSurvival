using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    [SerializeField] private TextMeshProUGUI percentage;
    [SerializeField] private TextMeshProUGUI loadingTxt;
    [SerializeField] private TextMeshProUGUI completeTxt;
    [SerializeField] private Image loadingBar;

    private string initLoadingTxt = "Loading";

    // 애니메이션 코루틴
    private Coroutine loadingCoroutine;

    public float FillAmount
    {
        get { return loadingBar.fillAmount; }
    }

    public void SetCompletePlayer(int completePlayer, int playerCount)
    {
        completeTxt.text = $"<{completePlayer}/{playerCount}> 준비 완료";
    }

    public void UpdateBar(float progress, float timer)
    {
        float percent = Mathf.Lerp(loadingBar.fillAmount, progress, timer);

        UpdateBar(percent);
    }

    public void UpdateBar(float percent)
    {
        percentage.text = (int)(percent * 100f) + " %";
        loadingBar.fillAmount = percent;
    }

    private void OnEnable()
    {
        loadingCoroutine = StartCoroutine(LoadingAnim());
    }

    private void OnDisable()
    {
        StopCoroutine(loadingCoroutine);
    }

    private IEnumerator LoadingAnim()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);

        while (true)
        {
            string loading = loadingTxt.text;

            if (loading.Equals("Loading . . ."))
            {
                loadingTxt.text = initLoadingTxt;
            }
            else
            {
                loadingTxt.text = loading + " .";
            }

            yield return delay;
        }
    }
}