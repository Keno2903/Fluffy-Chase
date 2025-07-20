using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{

    public GameManager gameManager;

    public int index;

    public bool levelUnlocked;

    public Button myButton;

    public int stars;

    public Image[] starsImages = new Image[3];



    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < stars; i++)
        {

            starsImages[i].enabled = true;
        }
        StartCoroutine(Set());
    }

    public IEnumerator Set()
    {
        yield return new WaitForEndOfFrame();
        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < stars; i++)
        {

            starsImages[i].enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openLevel()
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        gameManager.openSceneSlowly("Level" + LevelSceneManager.instance.LevelIndex);
        if (!GameManager.instance.tutorial)
            return;
        foreach (autoTyping text in FindObjectsOfType<autoTyping>())
        {
            text.myText.enabled = false;
        }
    }

    public void openNextLevel()
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }

        if (LevelSceneManager.instance.LevelJustPlayed == 42)
            return;

        LevelSceneManager.instance.selectLevel(LevelSceneManager.instance.LevelJustPlayed + 1);
    }

    public void Retry()
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        LevelSceneManager.instance.selectLevel(LevelSceneManager.instance.LevelJustPlayed);
    }

    public void SelectLevel()
    {
        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        LevelSceneManager.instance.selectLevel(index);
        if (!GameManager.instance.tutorial)
            return;
        foreach (autoTyping text in FindObjectsOfType<autoTyping>())
        {
            text.myText.enabled = false;
        }
    }




}
