using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public LevelButton[] levelButtons;

    // Start is called before the first frame update
    void Awake()
    {
        levelButtons = FindObjectsOfType<LevelButton>();

       foreach (var button in levelButtons)
        {
            button.stars = CurrentData.instance.levels[button.index - 1];
            if (button.index - 1 <= CurrentData.instance.currentLevel)
            {
                // More than Zero Stars == Level unlocked
                // Or the Level you are on
                // Or first Level

                button.levelUnlocked = true;
                button.GetComponent<Button>().interactable = true;
                button.GetComponentInParent<Image>().color = new Color(1, 1, 1, 1);
                Image[] stars = button.GetComponentsInChildren<Image>();
                foreach (var star in stars)
                {
                    star.color = new Color(1, 1, 1, 1);
                }

            }
            else
            {
                Image[] stars = button.GetComponentsInChildren<Image>();
                foreach (var star in stars)
                {
                    star.color = new Color(1, 1, 1, 0.5f);
                }
                button.GetComponent<Button>().interactable = false;
                button.GetComponentInParent<Image>().color = new Color(1, 1, 1, 0.75f);
            }

            
        }
        
    }

    private void Start()
    {
        StartCoroutine(PlayTheme());
    }

    IEnumerator PlayTheme()
    {
        yield return new WaitForSecondsRealtime(1);
        foreach (AudioSource audio in AudioManager.instance.GetComponents<AudioSource>())
        {
            if (audio.clip.name == "Cartoon" && !audio.isPlaying)
                AudioManager.instance.Play("Theme");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
