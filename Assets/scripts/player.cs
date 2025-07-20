using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class player : MonoBehaviour
{

    Animator myAnimator;

    float spinSpeed = 3f;
    [SerializeField]
    float runSpeed = 5;

    float jumptimer = 0.5f;

    public bool joystickEnabled = true;

    public Joystick joyStick;

    public bool isSprinting = false;

    public bool isCaught = false;

    public List<farmer> farmersThatAreChasingPlayer = new List<farmer>();

    public float h2;
    public float v2;

    [Header("Energy")]

    Enegrybar Enegrybar;

    float MaxEnergy = 100;

    [SerializeField]
    public float currentEnergy = 100;

    [SerializeField]
    public PlayerStats animal;

    [Header("Potions")]

    public bool invisible;

    public bool boosted;

    float invisibleTimer = 10f;

    float boostedTimer = 15f;

    [Header("Camera")]

    public GameObject thirdPersonCamera;
    public GameObject topCamera;
    [SerializeField]
    public Camera FieldCamera;

    //public Camera currentCamera;

    [Header("Audio")]
    bool sprintSoundIsPlaying = false;
    bool walkSoundIsPlaying = false;
    enum walkingStatus
    {
        standing, walking, running, jumping
    }
    [SerializeField]
    walkingStatus status = walkingStatus.standing;
    [SerializeField]
    public AudioSource walkingSound;
    [SerializeField]
    public AudioSource sprintingSound;
    [SerializeField]
    public AudioSource animalSound;

    bool animalSoundPlaying;

    [Header("Field")]
    bool fieldSoundPlaying = false;

    public bool inspectingFarm;

    GameUI ui;

    private void Start()
    {

        foreach(Camera c in FindObjectsOfType<Camera>())
        {
            if(c.name == "FarmCamera")
            c.enabled = false;
        }
        ui = FindObjectOfType<GameUI>();
        currentEnergy = MaxEnergy;
        FieldCamera.enabled = false;
        AudioManager.instance.Play("Farm");
        if (SceneManager.GetActiveScene().name == "ChooseAnimal")
        {
            this.GetComponent<player>().enabled = false;
            return;
        }
        joyStick = GameObject.FindObjectOfType<Joystick>();
        myAnimator = GetComponentInChildren<Animator>();
        Enegrybar = FindObjectOfType<Enegrybar>();
        CheckCameras();
    }

  
    // Update is called once per frame
    void Update()
    {
        joyStick = GameObject.FindObjectOfType<Joystick>();
        if (SceneManager.GetActiveScene().name == "ChooseAnimal")
        {
            return;
        }

        if (!isCaught)
        {
            if (ui.active || ui.paused)
                return;

            if (CurrentData.instance.joystickFixed)
            {
                controlWithKeyboard();
            }else
            {
                controlWithKeyboard2();
            }
            
        }
        else
        {
            myAnimator.SetInteger("animation", 8);
        }

        if (invisible)
        {
            invisibleTimer -= Time.deltaTime;
            Text[] timerTexts = FindObjectsOfType<Text>();
            Text timerText = FindObjectOfType<Text>(); ;

            foreach (var text in timerTexts)
            {
                if(text.name == "InvisibleTimer")
                {
                    timerText = text;
                    timerText.enabled = true;
                    timerText.text = Mathf.RoundToInt(invisibleTimer).ToString();
                }
            }
            if(invisibleTimer <= 0)
            {
                if(!FindObjectOfType<farmer>().playerInField)
                FindObjectOfType<GameUI>().FieldVolume.SetActive(false);

                invisible = false;
                GetComponent<BoxCollider>().enabled = true;
                invisibleTimer = 10f;
                timerText.enabled = false;
            }
        }else if(boosted)
        {
            boostedTimer -= Time.deltaTime;
            Text[] Texts = FindObjectsOfType<Text>();
            Text timerText = FindObjectOfType<Text>(); ;

            foreach (var text in Texts)
            {
                if (text.name == "InvisibleTimer")
                {
                    timerText = text;
                    timerText.enabled = true;
                    timerText.text = Mathf.RoundToInt(boostedTimer).ToString();
                }
            }
            if (boostedTimer <= 0)
            {
                boostedTimer = 15;
                boosted = false;
                timerText.enabled = false;
            }
        }
       UpdateAudio();
    }

    public void lose()
    {
        status = walkingStatus.standing;

        if(CurrentData.instance.sound && !animalSoundPlaying)
        {
            animalSoundPlaying = true;
            animalSound.Play();
        }
    }
    void controlWithKeyboard()
    {
        // One hand controlls
        if (inspectingFarm)
            joyStick.enabled = false;
        else
            joyStick.enabled = true;

        h2 = joyStick.Horizontal;
        v2 = joyStick.Vertical;
        if (!joystickEnabled)
        {
            v2 = 0;
            h2 = 0;
        }

        if (h2 != 0)
        {
            myAnimator.SetInteger("animation", 1);
            transform.Rotate(new Vector3(0, h2 * CurrentData.instance.Sensitivity * 1.5f, 0));
            status = walkingStatus.walking;
            //currentCamera = thirdPersonCamera;
        }
        if (v2 < 0)
        {
            //running backwards
            v2 = -0.4f;
            myAnimator.SetInteger("animation", 1);
            status = walkingStatus.walking;
            // currentCamera = thirdPersonCamera;
        }
        else if (v2 > 0.9)
        {
            sprint();
        }
        else if (v2 > 0.9 && h2 != 0)
        {
            sprint();
        }
        else if (v2 > 0 && v2 < 0.9)
        {
            walk();
        }
        else if (v2 == 0)
        {
            //currentCamera = thirdPersonCamera;
            //standing
            if (currentEnergy < 100)
            {
                currentEnergy += Time.deltaTime * animal.endurance;
            }
            Enegrybar.setValue(currentEnergy);
            isSprinting = false;

            if (h2 != 0)
                return;

            myAnimator.SetInteger("animation", 0);
            status = walkingStatus.standing;
        }

        transform.localPosition += transform.forward * v2 * Time.deltaTime * runSpeed;

        Vector3 FORWARD = transform.TransformDirection(Vector3.forward);

        // transform.localPosition += FORWARD * v2 * Time.deltaTime * runSpeed;

    }

    void controlWithKeyboard2()
    {
       
        // Two hand Controls

        v2 = joyStick.Vertical;


        if (v2 != 0 && Input.touchCount == 2)
        {
            Touch touch;

            if(Vector2.Distance(Input.GetTouch(0).position, joyStick.transform.position) > Vector2.Distance(Input.GetTouch(1).position, joyStick.transform.position))
            {
                touch = Input.GetTouch(0);
            }else
            {
                touch = Input.GetTouch(1);
            }
            
            if (touch.phase == TouchPhase.Moved)
            {
                h2 = touch.deltaPosition.x / 30 * CurrentData.instance.Sensitivity / 4;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                h2 = 0;
            }
        }
        else if (v2 == 0 && Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                h2 = touch.deltaPosition.x / 30 * CurrentData.instance.Sensitivity / 4;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                h2 = 0;
            }
        }


        if (!joystickEnabled)
        {
            v2 = 0;
            h2 = 0;
        }

        if (h2 != 0 && !inspectingFarm && !GameManager.instance.tutorial)
        {
            myAnimator.SetInteger("animation", 1);
            transform.Rotate(new Vector3(0, h2 * CurrentData.instance.Sensitivity * 1.5f, 0));
            status = walkingStatus.walking;
            //currentCamera = thirdPersonCamera;
        }
        if (v2 < 0 && !inspectingFarm)
        {
            //running backwards
            v2 = -0.4f;
            myAnimator.SetInteger("animation", 1);
            status = walkingStatus.walking;
            // currentCamera = thirdPersonCamera;
        }
        else if (v2 > 0.9 && !inspectingFarm)
        {
            sprint();
        }
        else if (v2 > 0.9 && h2 != 0 && !inspectingFarm)
        {
            sprint();
        }
        else if (v2 > 0 && v2 < 0.9 && !inspectingFarm)
        {
            walk();
        }
        else if (v2 == 0)
        {
            //currentCamera = thirdPersonCamera;
            //standing
            if (currentEnergy < 100)
            {
                currentEnergy += Time.deltaTime * animal.endurance;
            }
            Enegrybar.setValue(currentEnergy);
            isSprinting = false;

            if (h2 != 0)
                return;

            myAnimator.SetInteger("animation", 0);
            status = walkingStatus.standing;
        }
        if (inspectingFarm || GameManager.instance.tutorial)
            return;

        transform.localPosition += transform.forward * v2 * Time.deltaTime * runSpeed;

        Vector3 FORWARD = transform.TransformDirection(Vector3.forward);

        // transform.localPosition += FORWARD * v2 * Time.deltaTime * runSpeed;

    }

    public void CheckCameras()
    {
        switch (CurrentData.instance.thirdPersonCamera)
        {
            case false:
                topCamera.SetActive(false);
                thirdPersonCamera.SetActive(true);
                //currentCamera = thirdPersonCamera;
                break;

            case true:
                topCamera.SetActive(true);
                thirdPersonCamera.SetActive(false);
                //currentCamera = topCamera;
                break;
        }

        if(SceneManager.GetActiveScene().name.Contains("ChooseAnimal"))
        {
            thirdPersonCamera.SetActive(false);
            thirdPersonCamera.SetActive(false);
            FieldCamera.enabled = false;
        }
    }

    void sprint()
    {
        //currentCamera = topCamera;
        if (boosted)
        {
            runSpeed =   currentEnergy / 100 * animal.speed * 1.4f;
        }
        else
        {
           runSpeed =   currentEnergy / 100 * animal.speed;
        }
        if (jumptimer == 0.5f)
        {
            status = walkingStatus.running;
            
            myAnimator.SetInteger("animation", 2);

            if(currentEnergy < 35)
                myAnimator.SetInteger("animation", 1);
        }
        else
        {
            status = walkingStatus.jumping;
            myAnimator.SetInteger("animation", 3);
            jumptimer -= Time.deltaTime;
        }

        if(jumptimer <= 0)
        {
            jumptimer = 0.5f;
        }
        isSprinting = true;
        currentEnergy -= Time.deltaTime/animal.endurance * 8;
        Enegrybar.setValue(currentEnergy);

        
    }

    void walk()
    {
        //currentCamera = thirdPersonCamera;
        isSprinting = false;
        if (jumptimer == 0.5f)
        {
            status = walkingStatus.walking;
            myAnimator.SetInteger("animation", 1);
        }
        else
        {
            status = walkingStatus.jumping;
            myAnimator.SetInteger("animation", 3);
            jumptimer -= Time.deltaTime;
        }

        if (jumptimer <= 0)
        {
            jumptimer = 0.5f;
        }
        if (boosted)
        {
            runSpeed = 3f;
        }else
        {
            runSpeed = 2;
        }
        
    }

    void UpdateAudio()
    {
        
        if (!CurrentData.instance.sound)
            return;
            
        switch(status)
        {
            case walkingStatus.standing:
                AudioManager.instance.Stop("Field");
                fieldSoundPlaying = false;
                walkingSound.Stop();
                sprintingSound.Stop();
                walkSoundIsPlaying = false;
                sprintSoundIsPlaying = false;
                break;
            case walkingStatus.walking:
                sprintingSound.Stop();
                if (walkSoundIsPlaying)
                    return;
                walkSoundIsPlaying = true;
                sprintSoundIsPlaying = false;
                walkingSound.Play();
                if (FindObjectOfType<farmer>() != null && FindObjectOfType<farmer>().playerInField && !fieldSoundPlaying)
                {
                    AudioManager.instance.Play("Field");
                    fieldSoundPlaying = true;
                }
                break;
            case walkingStatus.running:
                walkingSound.Stop();
                if (sprintSoundIsPlaying)
                    return;
                walkSoundIsPlaying = false;
                sprintSoundIsPlaying = true;
                sprintingSound.Play();
                if (FindObjectOfType<farmer>() != null && FindObjectOfType<farmer>().playerInField && !fieldSoundPlaying)
                {
                    AudioManager.instance.Play("Field");
                    fieldSoundPlaying = true;
                }
                break;
            case walkingStatus.jumping:
                walkingSound.Stop();
                sprintingSound.Stop();
                walkSoundIsPlaying = false;
                sprintSoundIsPlaying = false;

                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Field")
        {
            // Make field effect
            FindObjectOfType<GameUI>().FieldVolume.SetActive(true);
            // Enable the right Field Camera
            if(!CurrentData.instance.thirdPersonCamera)
            {
                FieldCamera.enabled = true;
            }
            else
            {
                if(v2 < 0.8)
                {
                    FieldCamera.enabled = true;
                }
                topCamera.GetComponent<CinemachineStateDrivenCamera>().enabled = false;
            }
            // Disable the other cameras

            fieldSoundPlaying = false;

            // Remove all the farmers from the farmers, that are chasing player array
            if(farmersThatAreChasingPlayer.Count != 0)
            {
                farmersThatAreChasingPlayer.RemoveRange(0, farmersThatAreChasingPlayer.Count);
            }
            // Enable the variable Farmer in field for each farmer
            foreach(farmer farmer in FindObjectsOfType<farmer>())
            {
                farmer.playerInField = true;
            }
            // Handle the music
            AudioManager.instance.Stop("Chase");
            AudioManager.instance.Play("Stealth");

            // HandleUI
            FindObjectOfType<GameUI>().chaseImage.enabled = false;

            FindObjectOfType<GameUI>().inspectFarmButton.gameObject.SetActive(true);

            if (!GameManager.instance.inLevel)
                FindObjectOfType<CoinSpawner>().playerInField = true;
        }
        else if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            AudioManager.instance.Play("Coin");
            myAnimator.SetInteger("animation", 3);
            jumptimer -= Time.deltaTime;

            if (GameManager.instance.inLevel)
            {
                LevelSceneManager.instance.collectedCoins++;
                CurrentData.instance.coins++;
                CurrentData.instance.safeData();
            }else
            {
                LevelSceneManager.instance.collectedCoins++;
                CurrentData.instance.coins++;
                CurrentData.instance.safeData();
                other.gameObject.GetComponentInParent<waypoint>().hasItem = false;
                // Highscore
                // Toggle off "hasItem" on Waypoint so that the coinspawner can spawn a coin again
            }
        }
        else if (other.gameObject.tag == "food")
        {
            LevelSceneManager.instance.collectedCarrots++;
            AudioManager.instance.Play("Eat");
            myAnimator.SetInteger("animation", 3);
            jumptimer -= Time.deltaTime;
            currentEnergy += 10;
            if(currentEnergy > 100)
            {
                currentEnergy = 100;
            }
            Destroy(other.gameObject);

            LevelSceneManager.instance.addedCarrotOrCoin_();
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Field")
        {
            FindObjectOfType<FieldCamera>().inField = false;
            FindObjectOfType<GameUI>().FieldVolume.SetActive(false);
            
            AudioManager.instance.Stop("Field");
            farmer[] Farmer = FindObjectsOfType<farmer>();

            foreach(var farmer in Farmer)
            {
                farmer.playerInField = false;
            }
            GetComponent<BoxCollider>().enabled = true;

            // Handle Cams
            CheckCameras();
            FieldCamera.enabled = false;
            if(!CurrentData.instance.thirdPersonCamera)
            {
                topCamera.GetComponent<CinemachineStateDrivenCamera>().enabled = true;
            }else
            {
                topCamera.GetComponent<CinemachineStateDrivenCamera>().enabled = true;
            }

            FindObjectOfType<GameUI>().inspectFarmButton.gameObject.SetActive(false);

            if(!GameManager.instance.inLevel)
            FindObjectOfType<CoinSpawner>().playerInField = false;
        }
    }

    public void Boost()
    {
        if (boosted)
            return;

        AudioManager.instance.Play("Button");
        currentEnergy = 100f;
        boosted = true;
    }

    public void Invisible()
    {
        if (invisible)
            return;
        FindObjectOfType<GameUI>().FieldVolume.SetActive(true);
        AudioManager.instance.Play("Button");
        AudioManager.instance.Play("Stealth");
        AudioManager.instance.Stop("Chase");

        // Remove all the farmers from the farmers, that are chasing player array
        if (farmersThatAreChasingPlayer.Count != 0)
        {
            farmersThatAreChasingPlayer.RemoveRange(0, farmersThatAreChasingPlayer.Count);
        }

        invisible = true;
        FindObjectOfType<GameUI>().chaseImage.enabled = false;
    }

    public void InspectFarm()
    {
        inspectingFarm = true;
        foreach (Camera c in FindObjectsOfType<Camera>())
        {
            if (c.name == "FarmCamera")
                c.enabled = true;
        }
        FindObjectOfType<FieldCamera>().inField = true;
        topCamera.SetActive(false);
        thirdPersonCamera.SetActive(false);
        FieldCamera.enabled = false;
    }

    public void StopInspectingFarm()
    {
        inspectingFarm = false;
        foreach (Camera c in FindObjectsOfType<Camera>())
        {
            if (c.name == "FarmCamera")
                c.enabled = false;
        }
        FindObjectOfType<FieldCamera>().inField = false;
        if (!CurrentData.instance.thirdPersonCamera)
        {
            FieldCamera.enabled = true;
        }else
        {
            CheckCameras();
        }
    }

}
