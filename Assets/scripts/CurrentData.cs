using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CurrentData : MonoBehaviour
{
    
    public int[] animalsBought;

   // public float Volume;

    public int currentAnimal;

    public int QualityLevel;

    public string language;

    public int coins;

    public int currentLevel;

    public int[] levels;

    public int boostPotion;

    public int inivisiblePotion;

    public int HighScore;

    public DateTime lastDailyReward;

    public float Sensitivity = 2.5f;

    public bool joystickFixed = false;

    public bool thirdPersonCamera;

    public bool sound;

    public int coinsInHighScore;

    public int enemiesInHighScore;

    public static CurrentData instance;

    private void Awake()
    {   
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
   public void safeData()
    {
        SaveSystem.saveData(this);
    }

   public void Load()
   {
        GameData data = SaveSystem.LoadGameData();
        if(data == null)
        {
            animalsBought = new int[23];
            levels = new int[52];
            animalsBought[6] = 7;
            currentAnimal = 6;
            Sensitivity = 5;
            joystickFixed = false;
            thirdPersonCamera = true;
            sound = true;
            language = "English";
            lastDailyReward = DateTime.Now;
            GameManager.instance.StartTutorial();
            QualityLevel = 2;
            return;

        }
        //GameManager.instance.StartTutorial();
        enemiesInHighScore = data.enemiesInHighScore;
        coinsInHighScore = data.coinsInHighScore;
        HighScore = data.HighScore;
        levels = data.levels;
        currentLevel = data.currentLevel;
        currentAnimal = data.currentAnimal;
        animalsBought = data.animalsBought;
        lastDailyReward = data.lastDailyReward;
        QualityLevel = data.QualityLevel;
        boostPotion = data.boostPotion;
        inivisiblePotion = data.inivisiblePotion;
        language = data.language;
        coins = data.coins;
        Sensitivity = data.sensitivity;
        joystickFixed = data.joystickFixed;
        thirdPersonCamera = data.thirdPersonCamera;
        sound = data.sound;

        for (int i = 0; i < data.levels.Length; i++)
        {
            levels[i] = data.levels[i];
        }
    }
}
