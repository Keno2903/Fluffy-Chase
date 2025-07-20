using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;

    public GameObject enemy;

    public List<GameObject> enemiesInGame;

    public static enemySpawner instance;

    public int spawnedEnemies;

    private void Awake()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        yield return new WaitForSecondsRealtime(1);
        if (GameManager.instance.inLevel)
        {
            SpawnEnemiesInLevel();
        }
        else
        {
            SpawnEnemiesInHighScoreLevel();
        }
    }
    public void SpawnEnemiesInLevel()
    {
        int count = 0;

        foreach (var spawnPoint in spawnPoints)
        {
            if(count < LevelSceneManager.instance.enemiesInScene)
            {
                GameObject enemyToSpawn = Instantiate(enemy, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z), Quaternion.identity);
                
                enemiesInGame.Add(enemyToSpawn);
                count++;
            } 
        }
    }

    public void SpawnEnemiesInHighScoreLevel()
    {
        Instantiate(enemy, spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform);
        spawnedEnemies++;
        FindObjectOfType<GameUI>().FarmerSpawnedInHighscoreLevel.text = spawnedEnemies.ToString();
    }

    public void DestroyEnemies()
    {
        
        foreach (var enemy in enemiesInGame)
        {
            Destroy(enemy);
        }
    }
}
