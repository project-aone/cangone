using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//from Darengous Dave
public class GameStatus : MonoBehaviour
{

    public static int score = 0;
    public static int currentLevel = 1;
    public static int lives = 3;
    public static int kill = 0;

    public void NewGameButtonPressed()
    {
        ApplySettingsForLevel(1);
    }

    public void GoToLevel_2()
    {
        ApplySettingsForLevel(2);
    }

    public void GoToLevel_3()
    {
        ApplySettingsForLevel(3);
    }

    public void GoToLevel_4()
    {
        ApplySettingsForLevel(4);
    }

    ///apply settings for chosen level
    private void ApplySettingsForLevel(int level)
    {
        GameStatus.lives = 3;
        GameStatus.currentLevel = level;
        GameStatus.score = 0;
        GameStatus.kill = 0;
        SceneManager.LoadScene("level1b");
    }

    public void restart()
    {
        Time.timeScale = 1;
        GameStatus.lives = 3;
        GameStatus.score = 0;
        GameStatus.kill = 0;
        SceneManager.LoadScene(currentLevel);
    }

    public void backmenu()
    {
        SceneManager.LoadScene("menu");
        Time.timeScale = 1;
    }

    public void GoToLevelSelectionScreen()
    {
        SceneManager.LoadScene("LevelChoiceScreen");
    }
    /// Quit Game
    public void ExitGame()
    {
        Application.Quit();
    }
    public void info()
    {
        SceneManager.LoadScene("Info");
    }



}
