using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour
{

    public Slider mySlider;

    public Dropdown myDropdown;

    public Toggle myToggle; //Sound

    public Toggle myToggle2; //Joystick

    public Toggle myToggle3; //Camera

    public Text myText; //Sound

    public Text myText2; //Joystick

    public Text myText3; //Camera

    private void Start()
    {
        Set();
    }


    public void Set()
    {
        myToggle.isOn = CurrentData.instance.sound;
        myToggle2.isOn = CurrentData.instance.joystickFixed;
        myToggle3.isOn = CurrentData.instance.thirdPersonCamera;
        mySlider.value = CurrentData.instance.Sensitivity;
        myDropdown.value = CurrentData.instance.QualityLevel;

        switch (CurrentData.instance.joystickFixed)
        {
            case true:
                myText2.text = "On";
                break;
            case false:
                myText2.text = "0ff";
                break;
        }
    }
    public void SetSound(bool sound_)
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        CurrentData.instance.sound = sound_;
       
        CurrentData.instance.safeData();

        switch(sound_)
        {
            case true:
                AudioManager.instance.Play("Theme");
                break;
            case false:
                foreach (Sound sound in AudioManager.instance.sounds)
                {
                    AudioManager.instance.Stop(sound.name);
                }  
                break;
        }
    }

    public void SetSensitivity(float number)
    {
        CurrentData.instance.Sensitivity = number;

        CurrentData.instance.safeData();
    }

    public void SetJoystick(bool bool_)
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        CurrentData.instance.joystickFixed = bool_;
        switch (CurrentData.instance.joystickFixed)
        {
            case true:
                myText2.text = "On";
                break;
            case false:
                myText2.text = "0ff";
                break;
        }
        CurrentData.instance.safeData();
    }

    public void SetCamera(bool bool_)
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        CurrentData.instance.thirdPersonCamera = bool_;
        switch (CurrentData.instance.thirdPersonCamera)
        {
            case true:
                myText3.text = "On";
                break;
            case false:
                myText3.text = "Off";
                break;
        }
        CurrentData.instance.safeData();
    }

    public void openFluffyChaseWebsite()
    {
        Application.OpenURL("https://fluffy-chase-2.jimdosite.com");
    }


    public void setQuality(int QualityIndex)
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        QualitySettings.SetQualityLevel(QualityIndex);
        CurrentData.instance.QualityLevel = QualityIndex;
        CurrentData.instance.safeData();
        QualityDropDown.instance.Set();
    }
}