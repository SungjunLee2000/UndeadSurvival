using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LoadingUI))]
public class SceneLoadManager : MonoBehaviourPunCallbacks
{
    public static string nextScene;

    [Header("참조 스크립트")]
    [SerializeField] private LoadingUI ui;

    // 로딩 변수
    private AsyncOperation op;
    private int playerCount;
    private int completePlayer;

    private void Start()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(SetNextScene), RpcTarget.All, nextScene);
        }
    }

    public static void LoadLevel(string sceneName)
    {
        nextScene = sceneName;

        PhotonNetwork.LoadLevel("Loading");
    }

    [PunRPC]
    private void SetNextScene(string sceneName)
    {
        nextScene = sceneName;

        // 씬 로딩 시작
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.6f);

        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        float percentage = 0f;
        while (!op.isDone)
        {
            timer += Time.deltaTime;
            percentage += Time.deltaTime * 0.7f;

            if (op.progress < 0.9f || percentage < 1.0f)
            {
                if (op.progress > percentage) ui.UpdateBar(percentage);
                else ui.UpdateBar(op.progress, timer);

                if (ui.FillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                ui.UpdateBar(1f, timer);

                if (ui.FillAmount == 1.0f)
                {
                    OnCompleted();

                    yield break;
                }
            }

            yield return null;
        }
    }

    private void OnCompleted()
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        photonView.RPC(nameof(LoadingCompleted), RpcTarget.MasterClient);
    }

    [PunRPC]
    private void LoadingCompleted()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(LoadingCompleted), RpcTarget.Others);
        }

        completePlayer++;
        ui.SetCompletePlayer(completePlayer, playerCount);

        if (completePlayer >= playerCount)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // 씬 로드
        op.allowSceneActivation = true;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        playerCount--;

        ui.SetCompletePlayer(completePlayer, playerCount);
    }
}