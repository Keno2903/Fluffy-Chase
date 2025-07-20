using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    

    public static GameManager instance;

    public TimeSpan diff;

    bool isPaused;

    public Canvas LoadingScreen;

    public Text LoadingText;

    public Animator Fill;

    public bool MoreCoinsButtonClicked = false;

    public bool MorePotionsButtonClicked = false;

    public bool inLevel;

    public bool themeSongPlaying;

    public bool tutorial;

    public bool hadRewardToday;

    public int interstitialAdcounter = 0;

    #region singleton

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        CurrentData.instance.Load();
    }

    #endregion

    

    private void Start()
    {

        Fill.SetBool("isLoading", true);

        if (!tutorial)
            diff = (DateTime.Now - CurrentData.instance.lastDailyReward);

        if (tutorial)
        {
            StartCoroutine(openSelectCountry());
        }else
        {
            StartCoroutine(openMenu());
        }
    }

    IEnumerator openSelectCountry()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("SelectCountry");
        
    }
    IEnumerator openMenu()
    {
        yield return new WaitForSecondsRealtime(4);
        SceneManager.LoadScene("Menu");

    }
    public void openSceneSlowly(string name)
    {
        StartCoroutine(LoadAsynchronously(name));
    }

    public IEnumerator LoadAsynchronously(string name)
    {
        CancelInvoke();
        Time.timeScale = 1;
        LoadingScreen.enabled = true;
        yield return new WaitForSecondsRealtime(0.5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        operation.allowSceneActivation = true;
        while (!operation.isDone)
        {
            yield return null;
        }

        if (name.Contains("Level") && !name.Contains("Choose") && !name.Contains("Showcase") && !name.Contains("HighScoreLevel"))
        {
            inLevel = true;
            AudioManager.instance.Stop("Theme");
        }else if(name.Contains("Shop"))
        {
            MoreCoinsButtonClicked = false;
            MorePotionsButtonClicked = false;
        }else if (name.Contains("HighScoreLevel"))
        {
            AudioManager.instance.Stop("Theme");
            inLevel = false;
            StartCoroutine(StartHighScoreLevel());
        }
        
        LoadingScreen.enabled = false;
        
    }
    public void StartTutorial()
    {
        //Tutorial that starts when Player starts game for the first time
        tutorial = true;

    }

    public void openScene(string name)
    {
        SceneManager.LoadScene(name);
    }
   
    public void openLevel(int level)
    {
        LoadAsynchronously("level" + level);
        AudioManager.instance.Play("Farm");
    }

    public IEnumerator StartHighScoreLevel()
    {
        yield return new WaitForEndOfFrame();
        if (!GameManager.instance.inLevel)
        {
            InvokeRepeating("playScoreSoundEverySecondInHighScoreMode", 0, 1);
        }

    }

    void playScoreSoundEverySecondInHighScoreMode()
    {
        farmer farmer = FindObjectOfType<farmer>();
        player player = FindObjectOfType<player>();
        if(farmer != null)
        {
            if (farmer.playerInField || player.v2 < 0.5)
                return;
        }
        

        AudioManager.instance.Play("HighscoreTimer");
        LevelSceneManager.instance.Score += 1;
        if (LevelSceneManager.instance.Score > CurrentData.instance.HighScore)
        {
            CurrentData.instance.HighScore = Mathf.RoundToInt(LevelSceneManager.instance.Score);
            CurrentData.instance.safeData();
        }

        if(LevelSceneManager.instance.collectedCoins > CurrentData.instance.coinsInHighScore)
        {
            CurrentData.instance.coinsInHighScore = LevelSceneManager.instance.collectedCoins;
        }

        if(LevelSceneManager.instance.Score % 15 == 0)
        {
            FindObjectOfType<enemySpawner>().SpawnEnemiesInHighScoreLevel();
        }

        if(FindObjectOfType<enemySpawner>().spawnedEnemies > CurrentData.instance.enemiesInHighScore)
        {
            CurrentData.instance.enemiesInHighScore = FindObjectOfType<enemySpawner>().spawnedEnemies;
            CurrentData.instance.safeData();
        }
    }

    public void LoseLevel()
    {
        SceneManager.LoadScene("Fail");
    }

    public void WinLevel()
    {
        SceneManager.LoadScene("Win");
    }

    public void LoseHighScoreLevel()
    {
        CancelInvoke();
        AudioManager.instance.Stop("Chase");
        SceneManager.LoadScene("HighScore");
    }

    public void pauseOrPlay()
    {
        isPaused = !isPaused;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
}
