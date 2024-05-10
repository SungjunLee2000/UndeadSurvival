using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    [Header("보상 선택창")]
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject selectPrefab;

    [Header("보상 양도창")]
    [SerializeField] private GameObject handOverContainer;
    [SerializeField] private List<GameObject> selectPlayers;

    [Header("상태 패널")]
    [SerializeField] private GameObject statePanel;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI playerCountText;

    // 로딩 텍스트
    private string initLoadingTxt = "플레이어가 보상을 고르고 있습니다";
    private int frame = 4;

    // 애니메이션 코루틴
    private Coroutine loadingCoroutine;

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
            for (int i = 0; i < frame; i++)
            {
                loadingText.text += ".";

                yield return delay;
            }

            loadingText.text = initLoadingTxt;

            yield return delay;
        }
    }

    public void SetContainer(bool isActive)
    {
        container.SetActive(isActive);
    }

    public bool IsActiveContainer()
    {
        return container.activeSelf;
    }

    public void SetStatePanel(bool isActive)
    {
        statePanel.SetActive(isActive);
    }    

    public void SetPlayerCount(int count)
    {
        playerCountText.text = $"남은 플레이어 수 : {count}명";
    }

    public GameObject SetItem(ItemData item)
    {
        // 아이템 선택창 생성
        GameObject itemObj = Instantiate(selectPrefab, container.transform);

        // 선택된 아이템 띄우기
        SelectedItem selectedItem = itemObj.GetComponent<SelectedItem>();
        selectedItem.SetItem(item);

        return itemObj;
    }

    public void SetHandOverContainer(bool isActive)
    {
        handOverContainer.SetActive(isActive);
    }

    public void SetActivePlayer(int index, bool isActive)
    {
        selectPlayers[index].SetActive(isActive);
    }
}