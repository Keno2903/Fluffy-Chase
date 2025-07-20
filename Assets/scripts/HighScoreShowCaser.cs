using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreShowCaser : MonoBehaviour
{

    public Text HighScoretext;
    public Text CoinsHighScoreText;
    public Text enemiesInHighScore;

    [SerializeField]
    public GameObject highscoreSign;
    bool swingingRight = true;
    float swingTimer = 0;
    float maxSwingTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        LevelSceneManager.instance.collectedCoins = 0;
        LevelSceneManager.instance.Score = 0;
        HighScoretext.text = CurrentData.instance.HighScore.ToString();
        CoinsHighScoreText.text = CurrentData.instance.coinsInHighScore.ToString();
        enemiesInHighScore.text = CurrentData.instance.enemiesInHighScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        swingTimer += Time.deltaTime;

        if (swingTimer > maxSwingTimer)
        {
            swingingRight = !swingingRight;
            swingTimer = 0;
            maxSwingTimer = 1;
        }

        if(swingingRight)
        {
            highscoreSign.transform.localEulerAngles = new Vector3(0, 0, highscoreSign.transform.localEulerAngles.z + 0.1f);
        }else
        {
            highscoreSign.transform.localEulerAngles = new Vector3(0, 0, highscoreSign.transform.localEulerAngles.z - 0.1f);
        }
    }
}
