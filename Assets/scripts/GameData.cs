using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GameData
{
    public int[] animalsBought;

    public int currentAnimal;

    public int QualityLevel;

    public string language;

    public int coins;

    public int boostPotion;

    public int inivisiblePotion;

    public DateTime lastDailyReward;

    public int currentLevel;

    public int[] levels = new int[52];

    public int HighScore;

    public float sensitivity;

    public bool joystickFixed;

    public bool thirdPersonCamera;

    public bool sound;

    public int coinsInHighScore;

    public int enemiesInHighScore;


    public GameData(CurrentData currentData_)
    {
        enemiesInHighScore = currentData_.enemiesInHighScore;
        coinsInHighScore = currentData_.coinsInHighScore;
        HighScore = currentData_.HighScore;
        animalsBought = currentData_.animalsBought;
        currentAnimal = currentData_.currentAnimal;
        QualityLevel = currentData_.QualityLevel;
        language = currentData_.language;
        coins = currentData_.coins;
        boostPotion = currentData_.boostPotion;
        inivisiblePotion = currentData_.inivisiblePotion;
        lastDailyReward = currentData_.lastDailyReward;
        sensitivity = currentData_.Sensitivity;
        joystickFixed = currentData_.joystickFixed;
        thirdPersonCamera = currentData_.thirdPersonCamera;
        sound = currentData_.sound;
        for (int i = 0; i < currentData_.levels.Length; i++)
        {
            levels[i] = currentData_.levels[i];
        }
        currentLevel = currentData_.currentLevel;
    }

}
