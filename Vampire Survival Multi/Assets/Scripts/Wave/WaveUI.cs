using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [Header("UI 오브젝트")]
    [SerializeField] private TextMeshProUGUI tmpUI;

    // 임시 참조 데이터
    private WaveData waveData;

    private void Start()
    {
        waveData = WaveData.Instance;
    }

    private void Update()
    {
        int time = (int)waveData.RemainTime;

        UpdateTimer(time);
    }

    private void UpdateTimer(int time)
    {
        int min = time / 60;
        int sec = time % 60;

        string timer = min.ToString("00") + ":" + sec.ToString("00");

        tmpUI.text = "<Wave>" +
            "\r\nWave : " + waveData.WaveLevel +
            "\r\nTimer : " + timer +
            "\r\nMob Count : " + waveData.MobCount;
    }
}