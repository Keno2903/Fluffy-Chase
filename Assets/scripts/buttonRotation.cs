using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonRotation : MonoBehaviour
{
    bool swingingRight = true;
    float swingTimer = 0;
    float maxSwingTimer = 0.5f;

    bool go;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(set());
    }

    IEnumerator set()
    {
        yield return new WaitForSecondsRealtime(Random.Range(0, 1));
        go = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!go)
            return;

        swingTimer += Time.deltaTime;

        if (swingTimer > maxSwingTimer)
        {
            swingingRight = !swingingRight;
            swingTimer = 0;
            maxSwingTimer = 1;
        }

        if (swingingRight)
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + 0.05f);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - 0.05f);
        }
    }
}
