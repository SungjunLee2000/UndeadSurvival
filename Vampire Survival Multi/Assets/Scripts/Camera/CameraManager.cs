using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("카메라 종류")]
    [SerializeField] 
    private GameObject playerCamera;
    private PlayerTracker playerTracker;
    
    [SerializeField] 
    private GameObject observerCamera;
    private PlayerTracker observerTracker;

    private void Start()
    {
        playerTracker = playerCamera.GetComponent<PlayerTracker>();
        observerTracker = observerCamera.GetComponent<PlayerTracker>();
    }

    public void InitPlayer(GameObject player)
    {
        playerTracker.SetPlayer(player.transform);
    }

    public void SetObservePlayer(GameObject other)
    {
        observerTracker.SetPlayer(other.transform);
    }

    public void SwitchPlayerCam()
    {
        playerCamera.SetActive(true);
        observerCamera.SetActive(false);
    }

    public void SwitchObserverCam()
    {
        playerCamera.SetActive(false);
        observerCamera.SetActive(true);
    }
}