using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using GameBehavior= Popcorn.GameObjects.Elementies.GameBehavior;
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

    //lives object
    public List<Image> lives = new List<Image>(3);
    Text txt_score, txt_high, txt_level;

    //initialitation
    private void Start()
    {
        //Win, 22/09/2019, for pause
        PauseCanvas.enabled = false;
        DBcanvas.enabled = false;
        PauseButton.enabled = true;
        JumpButton.enabled = true;
         
        //Win, 23/09/2019, for showing live object
        txt_score = GetComponentsInChildren<Text>()[1];
        txt_high = GetComponentsInChildren<Text>()[0];
        txt_level = GetComponentsInChildren<Text>()[2];

               for (int i = 0; i < 3 - GameStatus.lives; i++)
               {
                   Destroy(lives[lives.Count - 1]);
                   lives.RemoveAt(lives.Count - 1);
               }
        
    }
    private void Update()
    {
        //Rayhan, 22/09/2019 for add data
        if(GameBehavior.GameState==GameBehavior.GameStates.Ended && GameStatus.lives==0)
        {
            DBcanvas.enabled = true;
            playerscore = GameStatus.score;
            Scoretext.text = "Your Score: " + playerscore;

        }
        //Win, 23/09/2019 for showing live object
        switch (GameStatus.lives)
        {
            case 1:
                Destroy(lives[1]);
                lives.RemoveAt(1);
                break;
            case 2:
                Destroy(lives[2]);
                lives.RemoveAt(2);
                break;
            case 0:
                Destroy(lives[0]);
                lives.RemoveAt(0);
                break;

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
        RestClient.Put(url:"https://cangone-0826273034.firebaseio.com/"+playername+".json", user);
    }

    //Rayhan & win, 22/09/2019, for pause
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
