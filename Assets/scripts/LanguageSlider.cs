using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageSlider : MonoBehaviour
{
    public Transform Content;

    int offsetSettingScreen = 300;

    int offsetSetCountryScreen = 385;

    int offset;

    MenuManager mg;

    // Start is called before the first frame update

    public void Start()
    {
       
        mg = FindObjectOfType<MenuManager>();

        if (SceneManager.GetActiveScene().name == "SelectCountry")
        {
            CurrentData.instance.language = "English";
            mg.SetEnglish();
            offset = offsetSetCountryScreen;
            return;
        }else
        {
            offset = offsetSettingScreen;
        }

        if(CurrentData.instance.language == "English")
        {

        }else if(CurrentData.instance.language == "Deutsch")
        {
            SwipeRight();
        }else
        {
            SwipeLeft();
        }
    }

    public void SwipeRight()
    {

        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        if (Content.transform.localPosition.x == offset)
        {
            Content.localPosition = new Vector3(0 - offset, Content.localPosition.y, Content.localPosition.z);
        }else 
        Content.localPosition = new Vector3(Content.localPosition.x + offset, Content.localPosition.y, Content.localPosition.z);
        
        if (Content.localPosition.x == 0)
        {
            CurrentData.instance.language = "English";
            mg.SetEnglish();
        }
        else if(Content.localPosition.x == 0+offset)
        {
            CurrentData.instance.language = "Deutsch";
            mg.SetGerman();
        }else
        {
            CurrentData.instance.language = "French";
            mg.SetFrench();
        }

        CurrentData.instance.safeData();

        Notification.instance.CheckLangauge();

        if(SceneManager.GetActiveScene().name != "SelectCountry")
           QualityDropDown.instance.Set();
        
    }

    public void SwipeLeft()
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        if (Content.transform.localPosition.x == -offset)
        {
            Content.localPosition = new Vector3(0 + offset, Content.localPosition.y, Content.localPosition.z);
        }else
        Content.localPosition = new Vector3(Content.localPosition.x - offset, Content.localPosition.y, Content.localPosition.z);
        
        if (Content.localPosition.x == 0)
        {
            CurrentData.instance.language = "English";
            mg.SetEnglish();
        }else if(Content.localPosition.x ==  0-offset)
        {
            CurrentData.instance.language = "French";
            mg.SetFrench();
        }else
        {
            CurrentData.instance.language = "Deutsch";
            mg.SetGerman();
        }

        CurrentData.instance.safeData();

        Notification.instance.CheckLangauge();

        if (SceneManager.GetActiveScene().name != "SelectCountry")
            QualityDropDown.instance.Set();
        
    }
}
