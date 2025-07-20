using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameUI : MonoBehaviour
{
    // This script handles all the UI during the game e.g the collected carrots or coins, pausing the game, or the potions

    public Text CoinText;

    public Text CarrotText;

    public Text timesSeenText;

    public RawImage chaseImage;

    public Text farmerCount;

    public Text boostPotionText;

    public Text InvisiblePotionText;

    public bool active = false;

    [SerializeField]
    public GameObject pausePanel;

    public bool paused = false;

    public Image pauseButton;

    public Image arrowForFarmerDirection;

    public GameObject FieldVolume;

    public Button inspectFarmButton;

    bool inspectingFarm;

    public Text FarmerSpawnedInHighscoreLevel;

    player player;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.tutorial)
            FindObjectOfType<FloatingJoystick>().enabled = false;

        player = FindObjectOfType<player>();
        boostPotionText.text = CurrentData.instance.boostPotion.ToString();
        InvisiblePotionText.text = CurrentData.instance.inivisiblePotion.ToString();

        if (SceneManager.GetActiveScene().name == "HighScoreLevel")
            FarmerSpawnedInHighscoreLevel.text = 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.inLevel)
        {
            CarrotText.text = LevelSceneManager.instance.collectedCarrots.ToString() + "/" + LevelSceneManager.instance.CarrotsToCollectInLevel.ToString();
            CoinText.text = LevelSceneManager.instance.collectedCoins.ToString() + "/" + LevelSceneManager.instance.CoinsToCollectInLevel.ToString();
            timesSeenText.text = LevelSceneManager.instance.timesCaught.ToString();

        }
        else
        {
            CarrotText.text = Mathf.RoundToInt(LevelSceneManager.instance.Score).ToString();
            CoinText.text = LevelSceneManager.instance.collectedCoins.ToString();
        }

        if (player.farmersThatAreChasingPlayer.Count == 0)
        {
            farmerCount.enabled = false;
        }
        else
        {
            farmerCount.enabled = true;
            farmerCount.text = player.farmersThatAreChasingPlayer.Count.ToString();
        }
    }

public void BoostPotion()
    {
        if (player.invisible || player.boosted)
            return;
        // Give Player a Speed Boost
        if(CurrentData.instance.boostPotion >= 1)
        {
            CurrentData.instance.boostPotion--;
            player.Boost();
            boostPotionText.text = CurrentData.instance.boostPotion.ToString();
        }
        CurrentData.instance.safeData();
    }

    public void InvisiblePotion()
    {
        if (player.boosted || player.invisible)
            return;
        // Make Player Invnisble
        if (CurrentData.instance.inivisiblePotion >= 1)
        {
            CurrentData.instance.inivisiblePotion--;
            player.Invisible();
            InvisiblePotionText.text = CurrentData.instance.inivisiblePotion.ToString();
        }
        CurrentData.instance.safeData();
    }

    public void PauseGame()
    {
        paused = !paused;
        pausePanel.SetActive(paused);
        FindObjectOfType<Joystick>().enabled = !paused;
        pauseButton.enabled = !paused;

        if(paused)
        {
            foreach(AudioSource s in FindObjectsOfType<AudioSource>())
            {
                if (s.isPlaying)
                    s.Stop();
            }

            AudioManager.instance.Stop("Chase");
            Time.timeScale = 0;
        }else
        {
            AudioManager.instance.Play("Farm");

            foreach (farmer f in FindObjectsOfType<farmer>())
            {
                if(f.isChasing)
                {
                    AudioManager.instance.Play("Chase");
                }
            }
            Time.timeScale = 1;
        }
    }

    public void PointArrow(farmer farmer_, player player_)
    {
        Vector3 dirToTarget = (farmer_.transform.position - player_.transform.position).normalized;

        if (Vector3.Angle(transform.forward, dirToTarget) < 180 / 2)
        {
            Debug.Log("1");
        // Get the position of the object in screen space
        Vector3 objScreenPos = Camera.main.WorldToScreenPoint(farmer_.transform.position);

        // Get the directional vector between your arrow and the object
        Vector3 dir = (objScreenPos - arrowForFarmerDirection.transform.position).normalized;

        // Calculate the angle 
        // We assume the default arrow position at 0° is "up"
        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(dir, Vector3.up));

        // Use the cross product to determine if the angle is clockwise
        // or anticlockwise
        Vector3 cross = Vector3.Cross(dir, Vector3.up);
        angle = -Mathf.Sign(cross.z) * angle;

        // Update the rotation of your arrow
        arrowForFarmerDirection.transform.localEulerAngles = new Vector3(arrowForFarmerDirection.transform.localEulerAngles.x, arrowForFarmerDirection.transform.localEulerAngles.y, angle);
        }
        else
        {
            Debug.Log("2");
            var targetPosLocal = Camera.main.WorldToScreenPoint(farmer_.transform.position);
            var targetAngle = -Mathf.Atan2(-targetPosLocal.x, -targetPosLocal.y) * Mathf.Rad2Deg - 90;
            arrowForFarmerDirection.transform.eulerAngles = new Vector3(0, 0, targetAngle);
        }
    }

    public void inspectFarm()
    {
        if(!inspectingFarm)
        {
            FindObjectOfType<player>().InspectFarm();
            inspectingFarm = true;
        }else
        {
            FindObjectOfType<player>().StopInspectingFarm();
            inspectingFarm = false;
        }
        
    }
}
