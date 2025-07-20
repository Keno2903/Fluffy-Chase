using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CowExplanation : MonoBehaviour
{
    [SerializeField]
    public NavMeshAgent agent;

    [SerializeField]
    public Transform waypoint;

    [SerializeField]
    public GameObject canvas;
    [SerializeField]
    public Animator myAnimator;

    [SerializeField]
    public GameObject particle;

    [SerializeField]
    public autoTyping[] texts;

    public bool textFinished;

    bool cowHasReachedPoint;

    [SerializeField]
    public AudioSource runningSound;

    bool runningSoundPlaying;

    [SerializeField]
    GameObject buttons;

    bool buttonActive;

    bool buttonPushed; 

    int state = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(waypoint.position);
        textFinished = true;
        //AudioManager.instance.Play("Farm");
        AudioManager.instance.Play("Theme");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, waypoint.position) > 1)
        {
            if(!runningSoundPlaying)
            {
                runningSound.Play();
                runningSoundPlaying = true;
            }
            myAnimator.SetInteger("animation", 2);
        }else
        {
            runningSound.Stop();
            myAnimator.SetInteger("animation", 0);
            canvas.SetActive(true);
            cowHasReachedPoint = true;
            
        }

        handleText();
    }


    void handleText()
    {
        if(!buttonActive && textFinished && cowHasReachedPoint && Input.GetMouseButtonDown(0) || cowHasReachedPoint && textFinished && state == 0 || buttonPushed)
        {
            if (state == texts.Length)
                return;
            AudioManager.instance.Stop("Text");
            textFinished = false;

            texts[state].Type();

            if (state != 0)
                texts[state - 1].GetComponent<Text>().enabled = false;

            if (state != texts.Length)
                state++;

            if (state == texts.Length - 1)
            {
                StartCoroutine(showButtons());
            }
        }

    }

    IEnumerator showButtons()
    {
        buttonActive = true;
        yield return new WaitForSecondsRealtime(3f);
        buttons.SetActive(true);
    }
    
    IEnumerator endScene()
    {
        yield return new WaitForSecondsRealtime(3);

        foreach(Text text in FindObjectsOfType<Text>())
        {
            text.enabled = false;
        }
        GameManager.instance.openSceneSlowly("Menu");
    }

    public void endTutorial()
    {
        GameManager.instance.tutorial = false;
        CurrentData.instance.safeData();
        buttons.SetActive(false);
        buttonActive = false;
        buttonPushed = true;
        StartCoroutine(endScene());
    }

    public void continueTutorial()
    {
        buttons.SetActive(false);
        buttonActive = false;
        buttonPushed = true;
        StartCoroutine(endScene());
    }

}
