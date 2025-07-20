using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public int levelIndex;

    public int carrotsToCollect;

    public int coinsToCollect;

    public int enemiesInScene;

    public int howManyTimesYouCanGetCaught;

    public int toleranceToGetCaught;
}
