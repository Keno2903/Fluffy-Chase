using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionButton : MonoBehaviour
{


    public GameManager gameManager;

    public string Action;


    public Button myButton;

    // Start is called before the first frame update
    void Start()
    {

        gameManager = FindObjectOfType<GameManager>();
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(openScene);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openScene()
    {

        if (CurrentData.instance.sound)
        {
            AudioManager.instance.Play("Button");
        }
        gameManager.openSceneSlowly(Action);
        if (!GameManager.instance.tutorial)
            return;
        foreach (autoTyping text in FindObjectsOfType<autoTyping>())
        {
            text.myText.enabled = false;
        }
    }




}
