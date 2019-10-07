using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using GameBehavior= Popcorn.GameObjects.Elementies.GameBehavior;
using Popcorn.GameObjects.Objects;
public class pausee : MonoBehaviour
{
    //for showing score and kill at the end of the game
    [SerializeField] Text pointEnd;
    [SerializeField] Text killEnd;

//    public AudioSource

    //pause object
    public Canvas PauseCanvas;
    public Canvas DBcanvas;
    public Canvas OverCanvas;
    public Button PauseButton;

    private bool paused = false;
    public static bool overd;

    //score object
    public InputField playertext;
    public Text Scoretext;
    public static int playerscore;
    public static string playername;

    //lives object
    public List<Image> lives = new List<Image>(3);
    public List<Image> stars = new List<Image>(3);


    //initialitation
    private void Start()
    {
        //Win, 22/09/2019, for pause
        PauseCanvas.enabled = false;
        DBcanvas.enabled = false;
        OverCanvas.enabled=false;
        PauseButton.enabled = true;
        overd = false;

        over();
    }
    void Update()
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

        over();
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
        }
        else
        {
            PauseCanvas.enabled = true;
            Time.timeScale = 0;
            paused = true;
            PauseButton.enabled = false;
        }
        
    }

    public void over()
    {
        if (overd)
        {
            StartCoroutine("waitForSec");
        }
    }

    IEnumerator waitForSec()
    {
        yield return new WaitForSeconds(4);
        OverCanvas.enabled = true;
        PauseCanvas.enabled = false;
        PauseButton.enabled = false;
        countStars();
    }

    void countStars()
    {
     if(GameStatus.kill < 4 && GameStatus.score < 5)
        {
            Destroy(stars[2]);
            stars.RemoveAt(2);
        }  
    }
    
}
