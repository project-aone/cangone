using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Popcorn.GameObjects.Persons;
public class pausee : MonoBehaviour
{ 
    public Canvas PauseCanvas;
    public Button PauseButton;
    public Button JumpButton;
    public Canvas BebasCanvas;
    private bool paused = false;

    private void Start()
    {
        BebasCanvas.enabled = true;
        PauseCanvas.enabled = false;
    }
    public void pauseed()
    {
        if (paused)
        {
            BebasCanvas.enabled = true;
            PauseCanvas.enabled = false;
            Time.timeScale = 1;
            paused = false;
            PauseButton.enabled = true;

        }
        else
        {
            BebasCanvas.enabled = false;
            PauseCanvas.enabled = true;
            Time.timeScale = 0;
            paused = true;
            PauseButton.enabled = false;
        }
    }
}
