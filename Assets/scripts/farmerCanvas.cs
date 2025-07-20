using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class farmerCanvas : MonoBehaviour
{
    public Canvas myCanvas;

    public float destroyTimer = 0.5f;

    GameObject cam;

    GameObject player;


    private void Awake()
    {

    }
    private void Update()
    {


        cam = GameObject.FindGameObjectWithTag("Cam");
        {
            myCanvas.transform.LookAt(cam.transform.position);
        }


        if (myCanvas != null)
        {
            if (myCanvas.enabled)
            {
                destroyTimer -= Time.deltaTime;
                if (destroyTimer <= 0)
                {
                    myCanvas.enabled = false;
                    destroyTimer = 0.5f;

                }


            }
        }
    }
}
