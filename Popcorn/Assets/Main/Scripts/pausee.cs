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
    public Canvas OverCanvas;
    public Canvas DieCanvas;
    public Button PauseButton;

    private bool paused = false;
    public static bool overd;
    public static bool die;

    //lives object
    public List<Image> lives = new List<Image>(3);
    public List<Image> stars = new List<Image>(3);

    //show point and kill at the end of the game
    [SerializeField] Text pointShow;
    [SerializeField] Text killShow;

    //initialitation
    private void Start()
    {
        //Win, 22/09/2019, for pause
        PauseCanvas.enabled = false;
        OverCanvas.enabled=false;
        PauseButton.enabled = true;
        DieCanvas.enabled = false;
        overd = false;
        die = false;

        over();
    }
    void Update()
    {
        over();
        
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

    void over()
    {
        if (overd || die)
        {
            StartCoroutine("waitForSec");
        }
    }

    IEnumerator waitForSec()
    {
        yield return new WaitForSeconds(4);
        if (overd)
        {
            OverCanvas.enabled = true;
            countStars();
            showScoreKill();
        }
        else if(die)
        {
            DieCanvas.enabled = true;
        }

        PauseCanvas.enabled = false;
        PauseButton.enabled = false;
        
    }

    void countStars()
    {
        if(GameStatus.kill < 4 && GameStatus.score < 5)
        {
            Destroy(stars[2]);
            stars.RemoveAt(2);
        }
        if(GameStatus.kill < 2 && GameStatus.score<2)
        {
            Destroy(stars[1]);
            stars.RemoveAt(1);
        }
    }
    
    public void showScoreKill()
    {
        pointShow.text = GameStatus.score.ToString();
        killShow.text = GameStatus.kill.ToString();
    }
}
