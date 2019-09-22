using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pausee : MonoBehaviour
{ 
    public Canvas PauseCanvas;
    public Button PauseButton;
    public Button JumpButton;
    private bool paused = false;

    private void Start()
    {
        PauseCanvas.enabled = false;
        PauseButton.enabled = true;
        JumpButton.enabled = true;

    }

    public void pauseed()
    {
        if (paused)
        {
            PauseCanvas.enabled = false;
            Time.timeScale = 1;
            paused = false;
            PauseButton.enabled = true;
            JumpButton.enabled = true;
        }
        else
        {
            PauseCanvas.enabled = true;
            Time.timeScale = 0;
            paused = true;
            PauseButton.enabled = false;
            JumpButton.enabled = false;
        }
        
    }

}
