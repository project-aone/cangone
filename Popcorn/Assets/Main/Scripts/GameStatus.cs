using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//from Darengous Dave
public class GameStatus :  MonoBehaviour
{
    public static int score = 0;
    //for enabled canvas;
    public Canvas canvasSignin;
    public Canvas canvasMenu;

    //for sign in to google;
    public Text infoText;
    public string webClientId = "<Your client id here>";

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    public void Start()
    {
        canvasSignin.enabled = true;
        canvasMenu.enabled = false;
    }
    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };

    }
    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    auth = FirebaseAuth.DefaultInstance;
                }
                else
                {
                    AddToInformation("Could not resolve all firebase dependencies" + task.Result.ToString());
                }
            }
            else
            {
                AddToInformation("Dependency check was not complete. Error :" + task.Exception.Message);
            }
        });
    }
    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthentificationFinished);
    }

    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }
    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthentificationFinished(Task<GoogleSignInUser> task)
    {
        if(task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator=task.Exception.InnerExceptions.GetEnumerator())
            {
                if(enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if(task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Email= " + task.Result.Email);
            AddToInformation("Google ID Token= " + task.Result.IdToken);
            AddToInformation("Email= "+task.Result.Email);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                {
                    AddToInformation("\nError Code= " + inner.ErrorCode + "Massage" + inner.Message);
                }
            }
            else
            {
                AddToInformation("SignIn Successful.");
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthentificationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthentificationFinished);
    }

    private void AddToInformation(string str) { infoText.text += "\n" + str; }
    //Game version 4.2 
    //Last changes 10/28/2016

    public void NewGameButtonPressed()
    {
        ApplySettingsForLevel(1);
    }
    public void Leaderboard()
    {
        SceneManager.LoadScene("leaderboard");
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

    public void GoToMainMenuScreen()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    ///apply settings for chosen level
    private void ApplySettingsForLevel(int level)
    {

        SceneManager.LoadScene("level1b");
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



}
