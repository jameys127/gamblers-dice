using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    //UI AND GLOBAL VARIABLES
    private int round = 1;
    private int nextAvailabelId = 0;
    public float leftMostPoint = -20;
    private int pointsToWin = 1000;
    private int hardScorePoints = 0;
    private int scorePoints = 0;
    private int oppScorePoints = 0;

    //Player UI
    public TextMeshProUGUI softScoreText;
    public TextMeshProUGUI hardScoreText;
    public TextMeshProUGUI scoreText;

    //Opponent UI
    public TextMeshProUGUI OppSoftScore;
    public TextMeshProUGUI OppHardScore;
    public TextMeshProUGUI OppScore;

    public GameObject switchingSides;
    public GameObject bustPanel;
    public List<GameObject> uiElements;
    private bool canBeSelected = false;
    public bool CurrentlyOpponentTurn = false;
    private bool busted = false;
    //END UI GLOBAL VARIABLES

    //GAME OBJECTS AND DICE COUNTERS
    public GameObject die;
    public GameObject Opponent;
    private List<GameObject> dice = new List<GameObject>();
    public int diceInPlay;
    private int rollingCounter;
    public GameObject gameOver;
    public GameObject youWon;
    public GameObject startButton;
    public GameObject passButton;
    public GameObject rollAgainButton;
    public GameObject RollDiceButton;
    //END GAME OBJECTS AND DICE COUNTERS
    
    //DICTIONARIES FOR DICE AND POINTS
    public Dictionary<int, int> diceSelectedForPoints = new Dictionary<int, int>();
    public Dictionary<int, int> threeOfAKindScores;
    private Dictionary<int, int> singlesPoints;
    public Dictionary<int, int> pointMultipliers;
    public Dictionary<int, int> fiveHundredComboWithStragglerOne;
    public Dictionary<int, int> fiveHundredComboWithStragglerFive;
    public Dictionary<int, int> sevenHundredComboWithStragglerFive;
    //END DICTIONARIES FOR DICE AND POINTS



    void Start()
    {
        threeOfAKindScores = new Dictionary<int, int> {
            {1, 1000},
            {2, 200},
            {3, 300},
            {4, 400},
            {5, 500},
            {6, 600},
        };
        singlesPoints = new Dictionary<int, int> {
            {1, 100},
            {2, 0},
            {3, 0},
            {4, 0},
            {5, 50},
            {6, 0},
        };
        pointMultipliers = new Dictionary<int, int> {
            {1, 1},
            {2, 2},
            {3, 1},
            {4, 2},
            {5, 4},
            {6, 8},
        };
        fiveHundredComboWithStragglerFive = new Dictionary<int, int> {
            {1, 1},
            {2, 1},
            {3, 1},
            {4, 1},
            {5, 2},
        };
        fiveHundredComboWithStragglerOne = new Dictionary<int, int> {
            {1, 2},
            {2, 1},
            {3, 1},
            {4, 1},
            {5, 1},
        };
        sevenHundredComboWithStragglerFive = new Dictionary<int, int> {
            {2, 1},
            {3, 1},
            {4, 1},
            {5, 2},
            {6, 1},
        };
        StartTheGame();

    } // void Start()







//***************************************
// ------DICE MANAGEMENT------

    public void CreateDiceSet(){
        for(int i = 0; i < 6; i++){
            GameObject dummydie = Instantiate(die, new Vector3(leftMostPoint, 0, 0), transform.rotation);
            dummydie.GetComponent<DiceBehavior>().setId(nextAvailabelId);
            dice.Add(dummydie);
            nextAvailabelId++;
            leftMostPoint = leftMostPoint + 10;
        }
        leftMostPoint = -20;
        diceInPlay = 6;
    }

    public void MoveReducedDiceSet(){
        if(dice.Count == 1){
            dice[0].transform.position = new Vector3(5, 0, 0);
        }
        if(dice.Count == 2){
            dice[0].transform.position = new Vector3(0, 0, 0);
            dice[1].transform.position = new Vector3(10, 0, 0);
        }
        if(dice.Count == 3){
            dice[0].transform.position = new Vector3(-5, 0, 0);
            dice[1].transform.position = new Vector3(5, 0, 0);
            dice[2].transform.position = new Vector3(15, 0, 0);
        }
        if(dice.Count == 4){
            dice[0].transform.position = new Vector3(-10, 0, 0);
            dice[1].transform.position = new Vector3(0, 0, 0);
            dice[2].transform.position = new Vector3(10, 0, 0);
            dice[3].transform.position = new Vector3(20, 0, 0);
        }
        if(dice.Count == 5){
            dice[0].transform.position = new Vector3(-15, 0, 0);
            dice[1].transform.position = new Vector3(-5, 0, 0);
            dice[2].transform.position = new Vector3(5, 0, 0);
            dice[3].transform.position = new Vector3(15, 0, 0);
            dice[4].transform.position = new Vector3(25, 0, 0);
        }
    }

    public void RollDice(){
        Debug.Log($"OpponentTurn() called, CurrentlyOpponentTurn = {CurrentlyOpponentTurn}");
        diceSelectedForPoints.Clear();
        var diceToRoll = GameObject.FindGameObjectsWithTag("Dice");
        foreach(var die in diceToRoll){
            die.GetComponent<DiceBehavior>().StartRolling();
        }
        RollDiceButton.SetActive(false);
    }

    // Getters and Setters for dice
    public List<GameObject> GetDiceList(){
        return dice;
    }
    public int GetDieWithId(int id){
        for(int i = 0; i < dice.Count; i++){
            GameObject die = dice[i];
            if(die.GetComponent<DiceBehavior>().getId() == id){
                return i;
            }
        }
        throw new Exception("this error was reached somehow");
    }
    public void SetDiceSelectedForPoints(int id, int side){
        diceSelectedForPoints.Add(id, side);
    }
    public void RemoveDiceSelectedForPoints(int id){
        diceSelectedForPoints.Remove(id);
    }
    public bool GetIfDiceCanBeSelected(){
        return canBeSelected;
    }

    // Collecting points for scoring
    public int CollectPoints(){
        int invalidSelection = 0;
        int totalScore = 0;
        Dictionary<int, int> count = new Dictionary<int, int>();
        foreach (var value in diceSelectedForPoints.Values){
            int increm;
            if(count.TryGetValue(value, out increm)){
                increm++;
                count[value] = increm;
            }else {
                count.Add(value, 1);
            }
        }
        if(count.Count == 6){
            return 1500;
        }
        if(count.Count == 5){
            if(CheckStragglersForPoints(count, fiveHundredComboWithStragglerFive)){
                return 550;
            }
            if(CheckStragglersForPoints(count, fiveHundredComboWithStragglerOne)){
                return 600;
            }
            if(CheckStragglersForPoints(count, sevenHundredComboWithStragglerFive)){
                return 800;
            }
        }
        if(count.Count == 5 &&
        count.TryGetValue(1, out _) &&
        count[1] == 1 &&
        count.TryGetValue(2, out _) &&
        count[2] == 1 &&
        count.TryGetValue(3, out _) &&
        count[3] == 1 &&
        count.TryGetValue(4, out _) &&
        count[4] == 1 &&
        count.TryGetValue(5, out _) &&
        count[5] == 1){
            return 500;
        }
        if(count.Count == 5 &&
        count.TryGetValue(2, out _) &&
        count[2] == 1 &&
        count.TryGetValue(3, out _) &&
        count[3] == 1 &&
        count.TryGetValue(4, out _) &&
        count[4] == 1 &&
        count.TryGetValue(5, out _) &&
        count[5] == 1 &&
        count.TryGetValue(6, out _) &&
        count[6] == 1){
            return 750;
        }

        foreach (var pair in count){
            int diceValue = pair.Key;
            int frequency = pair.Value;
            if (frequency >= 3){
                int baseScore = threeOfAKindScores[diceValue];
                int multScore = pointMultipliers[frequency];
                totalScore += baseScore * multScore;
            }
            if (frequency == 1 || frequency == 2){
                int baseScore = singlesPoints[diceValue];
                int multScore = pointMultipliers[frequency];
                totalScore += baseScore * multScore;
                if(baseScore == 0){
                    invalidSelection++;
                }
            }
        }
        if(invalidSelection == 0){
            return totalScore;
        }else{
            return 0;
        }
    }

    public bool CheckStragglersForPoints(Dictionary<int, int> count, Dictionary<int, int> pointTotal){
        int correctCounter = 0;
        foreach(var key in count.Keys){
            int valueCount;
            int valuePoint;
            count.TryGetValue(key, out valueCount);
            pointTotal.TryGetValue(key, out valuePoint);
            if(valueCount == valuePoint){
                correctCounter++;
            }
        }
        if(correctCounter == 5){
            return true;
        }else {
            return false;
        }
    }


    //---BUSTING CHECK---
    public void NotifyRollComplete(){
        rollingCounter++;
        if(rollingCounter == diceInPlay){
            Debug.Log("Initiating bust check becasue counter: " + rollingCounter);
            StartCoroutine(BustCheck());
            rollingCounter = 0;
        }
    }

    private IEnumerator BustCheck(){
        yield return new WaitForSeconds(0.3f);
        Dictionary<int, int> dummy = new Dictionary<int, int>(diceSelectedForPoints);
        Dictionary<int, int> diceCombo = new Dictionary<int, int>();
        foreach(var key in dummy.Keys){
            RemoveDiceSelectedForPoints(key);
        }
        foreach (var value in dummy.Values){
            int increm;
            if(diceCombo.TryGetValue(value, out increm)){
                increm++;
                diceCombo[value] = increm;
            }else {
                diceCombo.Add(value, 1);
            }
        }
        if(diceCombo.Count == 6){
            Debug.Log("broke cuz straight");
            canBeSelected = true;
            yield break;
        }
        if(diceCombo.Count == 5 &&
        diceCombo.TryGetValue(1, out _) &&
        diceCombo.TryGetValue(2, out _) &&
        diceCombo.TryGetValue(3, out _) &&
        diceCombo.TryGetValue(4, out _) &&
        diceCombo.TryGetValue(5, out _)){
            Debug.Log("broke cuz straight 500");
            canBeSelected = true;
            yield break;
        }
        if(diceCombo.Count == 5 &&
        diceCombo.TryGetValue(2, out _) &&
        diceCombo.TryGetValue(3, out _) &&
        diceCombo.TryGetValue(4, out _) &&
        diceCombo.TryGetValue(5, out _) &&
        diceCombo.TryGetValue(6, out _)){
            Debug.Log("broke cuz straight 750");
            canBeSelected = true;
            yield break;
        }
        foreach(var key in diceCombo.Keys){
            if(key == 1 || key == 5){
                Debug.Log("broke cuz 1 or 5");
            canBeSelected = true;
                yield break;
            }else if(diceCombo[key] >= 3){
                Debug.Log("broke cuz frequency >= 3");
            canBeSelected = true;
                yield break;
            }
        }
        busted = true;
        StartCoroutine(OpponentTurn());
    }
    //---END BUST CHECK---

// ------END DICE MANAGEMENT------
//***************************************




//***************************************
// ------GAME LOGIC------
    public void StartTheGame(){
        CreateDiceSet();
        startButton.SetActive(false);
        foreach(var ui in uiElements){
            ui.SetActive(true);
        }
    }
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowPointsForCollecting(){
        int points = CollectPoints();
        if(points > 0 && CurrentlyOpponentTurn == false){
            softScoreText.text = points.ToString();
            passButton.SetActive(true);
            rollAgainButton.SetActive(true);
        }else{
            OppSoftScore.text = points.ToString();
            passButton.SetActive(true);
            rollAgainButton.SetActive(true);
        }
        if(points == 0){
            softScoreText.text = points.ToString();
            OppSoftScore.text = points.ToString();
            passButton.SetActive(false);
            rollAgainButton.SetActive(false);
        }
    }

    public void DisplayPoints(){
        int point = CollectPoints();
        Debug.Log("Points: " + point);
    }



    public void ScoreAndContinue(){
        int score = CollectPoints();
        Debug.Log("score: " + score);
        int points = score;
        if(points != 0 && CurrentlyOpponentTurn == false){
            hardScorePoints += points;
            hardScoreText.text = hardScorePoints.ToString();
            softScoreText.text = "0";
            foreach(var keys in diceSelectedForPoints.Keys){
                int index = GetDieWithId(keys);
                Destroy(dice[index]);
                dice.RemoveAt(index);
                diceInPlay--;
            }
            passButton.SetActive(false);
            rollAgainButton.SetActive(false);
            canBeSelected = false;
            MoveReducedDiceSet();
            RollDiceButton.SetActive(true);
        }
        if(points != 0 && CurrentlyOpponentTurn){
            hardScorePoints += points;
            OppHardScore.text = hardScorePoints.ToString();
            OppSoftScore.text = "0";
            foreach(var keys in diceSelectedForPoints.Keys){
                int index = GetDieWithId(keys);
                Destroy(dice[index]);
                dice.RemoveAt(index);
                diceInPlay--;
            }
            passButton.SetActive(false);
            rollAgainButton.SetActive(false);
            canBeSelected = false;
            MoveReducedDiceSet();
            RollDiceButton.SetActive(true);
        }
        if(diceInPlay == 0){
            CreateDiceSet();
        }
        diceSelectedForPoints.Clear();
    }

    public void ScoreAndPass(){
        int score = CollectPoints();
        int points = score;
        if(points != 0 && CurrentlyOpponentTurn == false){
            hardScorePoints += points;
            hardScoreText.text = "0";
            scorePoints += hardScorePoints;
            scoreText.text = scorePoints.ToString();
            softScoreText.text = "0";
            hardScorePoints = 0;
        } else {
            hardScorePoints += points;
            OppHardScore.text = "0";
            oppScorePoints += hardScorePoints;
            OppScore.text = oppScorePoints.ToString();
            OppSoftScore.text = "0";
            hardScorePoints = 0;
        }
        if(CheckIfSomeoneWon()){
            if(CurrentlyOpponentTurn){
                gameOver.SetActive(true);
                foreach(var ui in uiElements){
                    ui.SetActive(false);
                }
                passButton.SetActive(false);
                rollAgainButton.SetActive(false);
                foreach(var x in dice){
                    Destroy(x);
                }
                dice.Clear();
                return;
            }else{
                youWon.SetActive(true);
                foreach(var ui in uiElements){
                    ui.SetActive(false);
                }
                passButton.SetActive(false);
                rollAgainButton.SetActive(false);
                foreach(var x in dice){
                    Destroy(x);
                }
                dice.Clear();
                return;
            }
        }
        diceSelectedForPoints.Clear();
        StartCoroutine(OpponentTurn());
    }
    public bool CheckIfSomeoneWon(){
        if(scorePoints >= pointsToWin || oppScorePoints >= pointsToWin){
            return true;
        } else {
            return false;
        }
    }

    private IEnumerator OpponentTurn(){
        float seconds = 2f;
        float timer = 0f;
        float something = 0.1f;

        hardScorePoints = 0;
        hardScoreText.text = "0";
        OppHardScore.text = "0";

        // Changing text to opponent
        if(CurrentlyOpponentTurn){
            uiElements[5].GetComponent<TextMeshProUGUI>().text = "Your Turn";
            switchingSides.GetComponentInChildren<TextMeshProUGUI>().text = "Your Turn\nOpponent's Score: " + oppScorePoints;
            round++;
            uiElements[2].GetComponent<TextMeshProUGUI>().text = round.ToString();
        } else {
            uiElements[5].GetComponent<TextMeshProUGUI>().text = "Opponent's Turn";
            switchingSides.GetComponentInChildren<TextMeshProUGUI>().text = "Opponent's Turn\nYour Score: " + scorePoints;
        }

        // Removing UI elements
        foreach(var ui in uiElements){
            ui.SetActive(false);
        }
        passButton.SetActive(false);
        rollAgainButton.SetActive(false);

        if(busted){
            bustPanel.SetActive(true);
            while(timer < seconds){
                timer += something + Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
            }
            busted = false;
        }

        // Destroying previous dice to start anew
        foreach(var x in dice){
            Destroy(x);
        }
        dice.Clear();

        

        // Switching sides screen for 2 seconds
        if(!busted){
            switchingSides.SetActive(true);
            while (timer < seconds){
                timer += something + Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
            }
        }

        // UI elements are activated again
        foreach(var ui in uiElements){
            ui.SetActive(true);
        }
        canBeSelected = false;

        // Create new set of dice
        CreateDiceSet();
        // RollDiceButton.SetActive(false);
        bustPanel.SetActive(false);
        switchingSides.SetActive(false);
        if(!CurrentlyOpponentTurn){
            CurrentlyOpponentTurn = true;
            Opponent.GetComponent<OpponentScript>().OpponentTurn(this);
        }else{
            CurrentlyOpponentTurn = false;
        }
        // Opponent.GetComponent<OpponentScript>().OpponentTurn();
    }



//*************************************************************************
//------TESTING METHODS------
    public void TestingStragglers(){
        diceSelectedForPoints.Clear();
        var diceToRoll = GameObject.FindGameObjectsWithTag("Dice");
        int counter = 1;
        foreach(var die in diceToRoll){
            if(counter == 1){
                die.GetComponent<DiceBehavior>().SetDiceSide(5);
            }
            if(counter >= 2){
                die.GetComponent<DiceBehavior>().SetDiceSide(counter);
            }
            counter++;

        }
    }
}
