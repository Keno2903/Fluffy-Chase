using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelShowcaser : MonoBehaviour
{
    public Text LevelName;

    public Text carrotsToFind;

    public Text coinsToCollect;

    public Text enemiesInScene;

    public Text TimesYouCanbeSeen;

    public Image[] starsImage = new Image[3];

    // Start is called before the first frame update
    void Start()
    {
        LevelName.text = "Level" + " " + LevelSceneManager.instance.LevelIndex.ToString();
        carrotsToFind.text = LevelSceneManager.instance.CarrotsToCollectInLevel.ToString();
        coinsToCollect.text = LevelSceneManager.instance.CoinsToCollectInLevel.ToString();
        enemiesInScene.text = LevelSceneManager.instance.enemiesInScene.ToString();
        TimesYouCanbeSeen.text = LevelSceneManager.instance.HowManyTimesYouCanGetCaugth.ToString();



        int starsAlreardyCollectedInLevel = CurrentData.instance.levels[LevelSceneManager.instance.LevelIndex - 1];
        for (int i = 0; i < starsAlreardyCollectedInLevel; i++)
        {
            
            starsImage[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
