using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointersArray : MonoBehaviour
{
[SerializeField]
public GameObject[] pointersInScene = new GameObject[1];

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.tutorial)
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
