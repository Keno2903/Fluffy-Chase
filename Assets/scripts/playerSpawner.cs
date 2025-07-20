using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpawner : MonoBehaviour
{


    public List<GameObject> list;

    [SerializeField]
    GameObject player;

    public static playerSpawner instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }else
        {
            instance = this;
        }

        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnPlayer()
    {
        
        int random = Random.Range(0, list.Count);
        
        
       // player = Instantiate(list[CurrentData.instance.currentAnimal], transform);
        player = Instantiate(list[CurrentData.instance.currentAnimal], transform.position, Quaternion.Euler(0, 270, 0), transform);
    }

    public void DestroyPlayer()
    {
        Destroy(player);
    }
}