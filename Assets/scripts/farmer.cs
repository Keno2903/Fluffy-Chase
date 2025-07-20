using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class farmer : MonoBehaviour
{
    [Header("AI")]
    public NavMeshAgent agent;

    public List<waypoint> waypoints;

    public float catchRadius = 2;

    public bool hasTarget;

    public float waitTime = 3;

    float HeadRotation = 0f;

    float runSpeed = 6;

    public Transform Head;

    int lastPoint;

    bool looksRight = false;

    public bool isChasing = false;

    Animator myAnimator;

    bool seen = false;

    public bool playerInField = false;

    [Header("Canvas")]
    public Canvas myCanvas;

    public float canvasTimer = 1f;

    player myPlayer; 

    [Header("Audio")]
    [SerializeField]
    public AudioSource runningSound;
    [SerializeField]
    public AudioSource walkingSound;
    bool sprintSoundIsPlaying = false;
    bool walkSoundIsPlaying = false;
    enum walkingState
    {
        standing, walking, running
    }
    [SerializeField]
    walkingState state = walkingState.standing;

    void Start()
    {
        myPlayer = FindObjectOfType<player>();
        myAnimator = GetComponentInChildren<Animator>();
        myAnimator.SetBool("Crouch_b", false);
        // get all waypoints
        foreach (var waypoint in FindObjectsOfType<waypoint>())
        {
            waypoints.Add(waypoint);
        }
    }

    void Update()
    {
        if (isChasing)
        {
            state = walkingState.running;
            chasePlayer();
        }else
        {
            state = walkingState.walking;
            patrol();
            LookAround();
        }
        if(agent.speed > 0)
        {
            // ensure that farmer is not crouching during patrol
            myAnimator.SetBool("Crouch_b", false);
        }
        // Exlemation Mark when Farmer sees Player
        if(myCanvas.enabled)
        {
            canvasTimer -= Time.deltaTime;
            if(canvasTimer <= 0)
            {
                myCanvas.enabled = false;
                canvasTimer = 1;
            }
        }
        HandleAudio();
    }


    void HandleAudio()
    {
        if (!CurrentData.instance.sound)
            return;

        switch (state)
        {
            case walkingState.standing:
                runningSound.Stop();
                walkingSound.Stop();
                break;
            case walkingState.walking:
                if (walkSoundIsPlaying)
                    return;
                walkSoundIsPlaying = true;
                sprintSoundIsPlaying = false;
                runningSound.Stop();
                walkingSound.Play();
                break;
            case walkingState.running:
                if (sprintSoundIsPlaying)
                    return;
                walkSoundIsPlaying = false;
                sprintSoundIsPlaying = true;
                walkingSound.Stop();
                runningSound.Play();
                break;
        }
    }

    public void patrol()
    {
        runSpeed = 6;
        seen = false;
        agent.speed = 2f;
        myAnimator.SetFloat("Speed_f", 0.3f, 0, Time.deltaTime);
        
        if (!hasTarget)
        {
            int randomPoint = Random.Range(0, waypoints.Count - 1);

            if(randomPoint == lastPoint || waypoints[randomPoint].isTargetedByAFarmer)
            {
                return;
            }
            agent.SetDestination(waypoints[randomPoint].transform.position);
            waypoints[randomPoint].isTargetedByAFarmer = true;
            lastPoint = randomPoint;
            hasTarget = true;
        }
        else if (agent.remainingDistance <= 4 && hasTarget)
        {
            // if enemy has reached a point
            // stand here and wait
            agent.speed = 0f;
            state = walkingState.standing;
            myAnimator.SetFloat("Speed_f", 0f, 0, Time.deltaTime);
            if (Random.Range(0, 2) == 1 && waitTime == 3)
            {
                myAnimator.SetBool("Crouch_b", true);
            }
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                waypoints[lastPoint].isTargetedByAFarmer = false;
                waitTime = 3;
                hasTarget = false;
                myAnimator.SetInteger("Animation_int", 0);
                myAnimator.SetBool("Crouch_b", false);
            }
        }
    }

    public void LookAround()
    {
        if (!looksRight)
        {
            HeadRotation += Time.deltaTime;
            if (HeadRotation > 0.99)
            {
                looksRight = true;
            }

        }
        else if (looksRight)
        {
            HeadRotation -= Time.deltaTime;
            if (HeadRotation < -0.99)
            {
                looksRight = false;
            }
        }
        myAnimator.SetFloat("Head_Horizontal_f", HeadRotation, 0f, Time.deltaTime);
    }
    public void chasePlayer()
    { 
        if (!seen)
        {
            //Enable The Arrow that shows the direction of the farmer

            if (CurrentData.instance.sound)
            myPlayer.animalSound.Play();

            AudioManager.instance.Play("Farmer");

            if (!myPlayer.farmersThatAreChasingPlayer.Contains(this))
                myPlayer.farmersThatAreChasingPlayer.Add(this);

            if (myPlayer.farmersThatAreChasingPlayer.Count == 1)
            {
                AudioManager.instance.Play("Chase");
                LevelSceneManager.instance.timesCaught++;
            }
            FindObjectOfType<GameUI>().chaseImage.enabled = true;
            myCanvas.enabled = true;
            seen = true;
        }
        if (!myPlayer.boosted && Vector3.Distance(transform.position, myPlayer.transform.position) < 8 && myPlayer.currentEnergy < 60)
            runSpeed += Time.deltaTime;
        
        if (!myPlayer.isSprinting || myPlayer.boosted || myPlayer.invisible)
            runSpeed = 6;


        hasTarget = false;
        isChasing = true;
        myAnimator.SetFloat("Head_Horizontal_f", 0, 0f, Time.deltaTime);
        agent.speed = runSpeed;
        myAnimator.SetBool("Crouch_b", false);
        agent.SetDestination(myPlayer.transform.position);
        myAnimator.SetFloat("Speed_f", 1, 0, Time.deltaTime);

        myCanvas.transform.LookAt(Camera.main.transform.position);

        float DistanceToPlayer = Vector3.Distance(transform.position, myPlayer.transform.position);
        if (DistanceToPlayer <= catchRadius && !myPlayer.GetComponent<player>().invisible && !FindObjectOfType<GameUI>().active & !playerInField)
        {
            myPlayer.isCaught = true;
            myAnimator.SetFloat("Speed_f", 0, 0, Time.deltaTime);
            myAnimator.SetInteger("Animation_int", 1);
            StartCoroutine(loseGame());
        }
    }


    IEnumerator loseGame()
    {
        state = walkingState.standing;
        myPlayer.lose();
        yield return new WaitForSecondsRealtime(0.5f);
        AudioManager.instance.Play("GameLost");
        if (GameManager.instance.inLevel)
        {
            GameManager.instance.LoseLevel();
        }else
        {
            if (GameManager.instance.tutorial)
            {
                GameManager.instance.LoseLevel();
            }else
            {
                GameManager.instance.LoseHighScoreLevel();

            }
        }
    }
}

