using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinOrFailScreen : MonoBehaviour
{

    public Image[] starsImage = new Image[3];

    public Text carrotsToFind;

    public Text coinsToCollect;

    public bool win;


    // Start is called before the first frame update

    void Start()
    {
        GameManager.instance.interstitialAdcounter++;
        AudioManager.instance.Stop("Chase");
        carrotsToFind.text = LevelSceneManager.instance.collectedCarrots.ToString() + "/" + LevelSceneManager.instance.CarrotsToCollectInLevel.ToString();
        coinsToCollect.text = LevelSceneManager.instance.collectedCoins.ToString() + "/" + LevelSceneManager.instance.CoinsToCollectInLevel.ToString();

        if (win)
        {
            if (CurrentData.instance.currentLevel == 4 || CurrentData.instance.currentLevel == 25)
                //iOSReviewRequest.Request();

            AudioManager.instance.Play("GameWon");

            if (!LevelSceneManager.instance.alreadyWonLevel)
            {
                CurrentData.instance.currentLevel++;
                CurrentData.instance.safeData();
            }

            int starsAlreardyCollectedInLevel = CurrentData.instance.levels[LevelSceneManager.instance.LevelIndex - 1];
            for (int i = 0; i < starsAlreardyCollectedInLevel; i++)
            {
                starsImage[i].enabled = true;
            }
            // Play Loose sound
        }


        if(GameManager.instance.interstitialAdcounter % 2 == 0 && CurrentData.instance.currentLevel != 4 && CurrentData.instance.currentLevel != 25)
        {
            //AdManager.instane.showInterstitial();
        }
    }
}
