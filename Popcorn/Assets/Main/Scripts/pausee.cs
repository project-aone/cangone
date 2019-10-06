using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using GameBehavior = Popcorn.GameObjects.Elementies.GameBehavior;
public class pausee : MonoBehaviour
{
    //pause object
    public Canvas PauseCanvas;
    public Canvas DBcanvas;
    public Button PauseButton;
    public Button JumpButton;
    private bool paused = false;

    //score object
    public InputField playertext;
    public Text Scoretext;
    public static int playerscore;
    public static string playername;
    public int i = 0;

    private void Start()
    {
        PauseCanvas.enabled = false;
        DBcanvas.enabled = false;
        PauseButton.enabled = true;
        JumpButton.enabled = true;
    }
    private void Update()
    {
        if (GameBehavior.GameState == GameBehavior.GameStates.Ended)
        {
            DBcanvas.enabled = true;
            playerscore = GameStatus.score;
            Scoretext.text = "Your Score: " + playerscore;

        }
    }

    public void OnSumbit()
    {
        playername = playertext.text;
        PostToDatabase();
    }

    private void PostToDatabase()
    {
        Users user = new Users();
        i += 1;
        RestClient.Put(url: "https://cangone-0826273034.firebaseio.com/" + playertext + ".json", user);
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
