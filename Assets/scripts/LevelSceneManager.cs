using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSceneManager : MonoBehaviour
{
    // This script keeps the information about the level the player is playing and persists between the scenes in order to feed the information to all scenes e.g the lost or won seen


    // Level Requirements
    public int LevelIndex;

    public int CarrotsToCollectInLevel;

    public int CoinsToCollectInLevel;

    public int HowManyTimesYouCanGetCaugth;

    public int enemiesInScene;

    public int toleranceToGetCaught;

    // What the Player archived

    public int collectedCoins;

    public int collectedCarrots;

    public int timesCaught;

    public static LevelSceneManager instance;

    public event Action addedCarrotOrCoin;

    public bool alreadyWonLevel;

    public int LevelJustPlayed;

    // Array of all Levels

    public Level[] levels = new Level[8];

    // Stats for Highscore

    public float Score;

    #region singleton
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);

        addedCarrotOrCoin += checkIfWon;
    }
    #endregion

    public void addedCarrotOrCoin_()
    {
        if(addedCarrotOrCoin != null)
        {
            addedCarrotOrCoin();
        }
    }

    public void selectLevel(int level)
    {

        collectedCarrots = 0;
        collectedCoins = 0;
        timesCaught = 0;
        CarrotsToCollectInLevel = levels[level - 1].carrotsToCollect;
        CoinsToCollectInLevel = levels[level - 1].coinsToCollect;
        HowManyTimesYouCanGetCaugth = levels[level - 1].howManyTimesYouCanGetCaught;
        enemiesInScene = levels[level - 1].enemiesInScene;
        LevelIndex = levels[level - 1].levelIndex;
        toleranceToGetCaught = levels[level - 1].toleranceToGetCaught;
        LevelJustPlayed = levels[level - 1].levelIndex;

        if(CurrentData.instance.levels[level - 1] > 0)
        {
            alreadyWonLevel = true;
        }else
        {
            alreadyWonLevel = false;
        }
        SceneManager.LoadScene("Showcase Level");

    }

    void checkIfWon()
    {
        if(collectedCarrots == CarrotsToCollectInLevel)
        {
            // Level Won
            player myPlayer = FindObjectOfType<player>();
            GameManager.instance.WinLevel();
            // Check how many stars have been earned

            // If the player has reached three stars once, you cannot have a lower amount of stars when you play the level again.
            if (CurrentData.instance.levels[LevelIndex - 1] == 3)
                return;



            if(collectedCoins == CoinsToCollectInLevel && timesCaught <=  HowManyTimesYouCanGetCaugth)
            {
                CurrentData.instance.levels[LevelIndex - 1] = 3;
            }else if(collectedCoins < CoinsToCollectInLevel && timesCaught <= HowManyTimesYouCanGetCaugth)
            {
                CurrentData.instance.levels[LevelIndex - 1] = 2;
            }
            else if (collectedCoins == CoinsToCollectInLevel && timesCaught > HowManyTimesYouCanGetCaugth)
            {
                CurrentData.instance.levels[LevelIndex - 1] = 2;
            }else
            {
                if (CurrentData.instance.levels[LevelIndex - 1] == 2)
                    return;
                CurrentData.instance.levels[LevelIndex - 1] = 1;
            }


            CurrentData.instance.safeData();
        }
    }

    private void Update()
    {

    }

}
