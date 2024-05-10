using UnityEngine;

public class GameController : MonoBehaviour
{
    private ControlContext controller;

    private void Start()
    {
        controller = ControlContext.Instance;

        // Init Controller
        controller.NoKeyDown = false;
    }

    private void Update()
    {
        controller.OnKeyPressed();
    }
}