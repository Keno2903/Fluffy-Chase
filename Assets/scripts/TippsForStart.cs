using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TippsForStart : MonoBehaviour
{
    [SerializeField]
    Text[] texts = new Text[1];

    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, texts.Length - 1);

        for(int i = 0; i < texts.Length; i++)
        {
            texts[i].enabled = false;  
        }

        texts[random].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
