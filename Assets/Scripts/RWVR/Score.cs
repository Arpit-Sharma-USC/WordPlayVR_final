using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    // Use this for initialization
    public static Score scoreInstance=null;
    public static int currRoundScore;
    public static bool destroyPoint;
    public static int destroyPointTimer;
    public static AudioSource bonus;
    public static GameObject pointRefrence;

    private Score() {
    }
    public static Score getInstance()
    {
        if (scoreInstance == null)
        {
            scoreInstance = new Score();
        }
        return scoreInstance;
    }
    void Start () {
        pointRefrence = GameObject.Find("Planks");
        scoreInstance = getInstance();
        currRoundScore = 0;
        destroyPointTimer = 400;
        destroyPoint = false;
        bonus = GameObject.Find("Score").GetComponent<AudioSource>();
        //scoreInstance.pseudoTimer = 100;
    }
	
	// Update is called once per frame
	void Update () {
        if (destroyPoint)
        {
           destroyPointTimer -= 1;
           
        }

        //Debug.Log("Destroy Point Digit");

        if (destroyPointTimer == 0)
        {
            Debug.Log("Destroy Point Objects");
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("pointDigit");

            for (var i = 0; i < gameObjects.Length; i++)
            {
                Destroy(gameObjects[i]);
            }
            destroyPointTimer = 400;
            destroyPoint = false;
        }
    }
    public void displayPoints()
    {
        Debug.Log("In +");
        GameObject totalDigit = (Instantiate(Resources.Load("Digit_+")) as GameObject);
        totalDigit.transform.position = new Vector3(24.8f, pointRefrence.transform.position.y + 15.00f, 23f);
        Vector3 tempVector = totalDigit.transform.rotation.eulerAngles;
        tempVector.y = 87.78f;
        totalDigit.transform.rotation = Quaternion.Euler(tempVector);
        totalDigit.transform.localScale += new Vector3(3.3f, 2f, 2f);
        totalDigit.tag = "pointDigit";

        totalDigit = (Instantiate(Resources.Load("Digit_5")) as GameObject);
        totalDigit.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 15.00f, 20f);
        tempVector = totalDigit.transform.rotation.eulerAngles;
        tempVector.y = 87.78f;
        totalDigit.transform.rotation = Quaternion.Euler(tempVector);
        totalDigit.transform.localScale += new Vector3(3.3f, 2f, 2f);
        totalDigit.tag = "pointDigit";

        bonus.Play();

        destroyPoint = true;
        Debug.Log(destroyPoint);
    }
    public void incrementCurrRoundScore()
    {
        currRoundScore += 5;
        Debug.Log("Current RoundScore IS: ");
        Debug.Log(currRoundScore);
        paintRoundScoreXP(currRoundScore);
    }
    public void calcScoreFromTimer(int currTimer,int currRound)
    {
        Debug.Log("Current Timer Value in Score");
        Debug.Log(currTimer);
        double curr = currTimer;
        // Paint the score in the current scene
        //int score = (currTimer)
        double maxScore = (currRound == 1) ? 20 : ((currRound == 2) ? 30 : 50);
        double currTime = (currRound == 1) ? 300: (1.5 * Countdown.round1Timer);
        double currScore = (curr / currTime) * maxScore;
        int paintThisScore = (int)currScore;
        Debug.Log("Score to be painted");
        Debug.Log(paintThisScore);

        paintRoundScoreXP(currRoundScore);
        paintScore(paintThisScore);
    }
    public void paintRoundScoreXP(int score)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ScoreDigit");

        GameObject scoreChar, scoreDigit, totalDigit;

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        scoreChar = (Instantiate(Resources.Load("Letter_X")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 10.00f, 24f);
        Vector3 temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";

        scoreChar = (Instantiate(Resources.Load("Letter_P")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 10.00f, 22f);
        temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";

        scoreChar = (Instantiate(Resources.Load("Colon_")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 10.00f, 20f);
        temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";
        
        string scoreString = score.ToString();
        float increment = 0f;

        // Extract each digit and get its prefab to be displayed.
        for (int i = 0; i < scoreString.Length; i++)
        {
            int scoreDigitNum = (int)(scoreString[i] - '0');

            totalDigit = (Instantiate(Resources.Load("Digit_" + scoreDigitNum)) as GameObject);
            totalDigit.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 10.00f, 18f - increment);
            Vector3 tempVector = totalDigit.transform.rotation.eulerAngles;
            tempVector.y = 87.78f;
            totalDigit.transform.rotation = Quaternion.Euler(tempVector);
            totalDigit.transform.localScale += new Vector3(3f, 3f, 3f);
            totalDigit.tag = "ScoreDigit";

            increment += 1.5f;


        }

    }
    public void paintScore(int score)
    {
        GameObject scoreChar, scoreDigit, totalDigit;
        //GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ScoreDigit");
       // GameObject scoreChar,scoreDigit,totalDigit;

       // for (var i = 0; i < gameObjects.Length; i++)
       // {
       //     Destroy(gameObjects[i]);
       // }
        

        scoreChar = (Instantiate(Resources.Load("Letter_S")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 6.00f, 24f);
        Vector3 temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";

        scoreChar = (Instantiate(Resources.Load("Letter_C")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 6.00f, 22f);
        temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";

        scoreChar = (Instantiate(Resources.Load("Letter_O")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 6.00f, 20f);
        temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";

        scoreChar = (Instantiate(Resources.Load("Letter_R")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 6.00f, 17f);
        temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";

        scoreChar = (Instantiate(Resources.Load("Letter_E")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 6.00f, 15f);
        temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";

        scoreChar = (Instantiate(Resources.Load("Colon_")) as GameObject);
        scoreChar.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 6.00f, 12.2f);
        temp = scoreChar.transform.rotation.eulerAngles;
        temp.y = 87.78f;
        scoreChar.transform.rotation = Quaternion.Euler(temp);
        scoreChar.transform.localScale += new Vector3(3f, 3f, 3f);
        scoreChar.tag = "ScoreChar";
      
        string scoreString = score.ToString();
        float increment = 0f;

        // Extract each digit and get its prefab to be displayed.
        for (int i = 0; i < scoreString.Length; i++)
        {
            int scoreDigitNum = (int)(scoreString[i] - '0');

            totalDigit = (Instantiate(Resources.Load("Digit_" + scoreDigitNum)) as GameObject);
            totalDigit.transform.position = new Vector3(25f, pointRefrence.transform.position.y + 6.00f, 10f - increment);
            Vector3 tempVector = totalDigit.transform.rotation.eulerAngles;
            tempVector.y = 87.78f;
            totalDigit.transform.rotation = Quaternion.Euler(tempVector);
            totalDigit.transform.localScale += new Vector3(3f, 3f, 3f);
            totalDigit.tag = "ScoreDigit";

            increment += 2f;


        }
        
    }
    

}
