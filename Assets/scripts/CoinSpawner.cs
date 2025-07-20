using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject Coin;

    public int spawnRate;

    float timer = 0f;

    public List<waypoint> spawnPoints;

    public static CoinSpawner instance;

    public bool playerInField;

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

    // Start is called before the first frame update
    void Start()
    {
        foreach (var point in FindObjectsOfType<waypoint>())
        {
            
            spawnPoints.Add(point);
        }
    }


    public void spawnCoins()
    {
        
        timer += Time.deltaTime;

        if (timer > spawnRate)
        {
            timer = 0;
            int randomPoint = Random.Range(0, spawnPoints.Count - 1);

            if (spawnPoints[randomPoint].hasItem)
               return;

            
            Vector3 point = spawnPoints[randomPoint].gameObject.transform.position;
            GameObject CoinToSpawn = Instantiate(Coin, point, Quaternion.identity);
            
            CoinToSpawn.transform.parent = spawnPoints[randomPoint].gameObject.transform;
            spawnPoints[randomPoint].hasItem = true;

        }
    }

    private void Update()
    {
        if (playerInField)
            return;
        spawnCoins();
    }
}
