using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SendInputString : MonoBehaviour {

    public GameObject Parent;
    public Text PuzzleText;
    public Text PuzzleText1;
    public Text PuzzleText2;

    public List<string> input = new List<string>();

    public Text HintText;
    public string inputForHint = "";
    public List<int[]> bitMapSubString = new List<int[]>();

    public int[] No_of_blanks;

    public Toggle Spin ;
    public Toggle MissingLettersOnly ;
    public bool valueOfSpinToggle = false;
    public bool valueOfMissingLettersOnlyToggle = false;

    public List<Text> No_Of_Blank_Text = new List<Text>();

    public float spawnIntervalFromUI;
    public Text SpawnIntervalText;


    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        ContainerGenerator obj = new ContainerGenerator();
    }

  
    public void ButtonPress() {
        input.Add(PuzzleText.text);
        input.Add(PuzzleText1.text);
        input.Add(PuzzleText2.text);


        inputForHint = HintText.text;

        for (int j = 0; j < 3; j++)
        {
            No_of_blanks[j] = int.Parse(No_Of_Blank_Text[j].text);
            
            int[] temp = new int[input[j].Length];
            bitMapSubString.Add(temp);
            System.Random rnd = new System.Random();
            for (int i = 0; i < bitMapSubString[j].Length; i++)
            {
                bitMapSubString[j][i] = 1;

            }



            int rand_id;
            if (No_of_blanks[j] >= input[j].Length - 2)
                No_of_blanks[j] = input[j].Length / 2;

            Debug.Log("No Blanks:" + No_of_blanks[j]);
            ContainerCollider.setBlanks(No_of_blanks[j]);

            while (No_of_blanks[j] != 0)
            {
                rand_id = rnd.Next(1, input[j].Length - 1);
                if (bitMapSubString[j][rand_id] != 0)
                {
                    bitMapSubString[j][rand_id] = 0;
                    No_of_blanks[j]--;

                }
            }
        }//end of for j
        valueOfSpinToggle = Spin.isOn;
        valueOfMissingLettersOnlyToggle = MissingLettersOnly.isOn;
        spawnIntervalFromUI = float.Parse(SpawnIntervalText.text);


        SceneManager.LoadScene("Game");       
    }
}
