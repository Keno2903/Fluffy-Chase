using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopSwipeButton : MonoBehaviour
{
    public Transform Rect;

    public static shopSwipeButton instance;


    private void Awake()
    {
        Rect = FindObjectOfType<shopManager>().Rect;
        instance = this;
    }
    public void SwipeRight()
    {
        AudioManager.instance.Play("Button");
        Rect.transform.localPosition = new Vector3(Rect.localPosition.x - 225, Rect.localPosition.y, Rect.localPosition.z);
     
    }
    public void SwipeLeft()
    {
        AudioManager.instance.Play("Button");
        Rect.transform.localPosition = new Vector3(Rect.localPosition.x + 225, Rect.localPosition.y, Rect.localPosition.z);
      
    }
}
