using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    // UI GLOBAL VARIABLES
    private int nextAvailabelId = 0;
    public float centerXPoint = 5;
    public float leftMostPoint = -20;
    public float rightMostPoint = 30;
    private int softScorePoints;
    private int hardScorePoints;
    public TextMeshProUGUI softScore;
    public TextMeshProUGUI hardScore;
    // END UI GLOBAL VARIABLES

    // GAME OBJECTS AND DICE COUNTERS
    public GameObject die;
    private List<GameObject> dice = new List<GameObject>();
    private int diceInPlay;
    private int rollingCounter;
    public GameObject gameOver;
    public GameObject startButton;
    public GameObject passButton;
    public GameObject rollAgainButton;
    //END GAME OBJECTS AND DICE COUNTERS
    
    //DICTIONARIES FOR DICE AND POINTS
    private Dictionary<int, int> diceSelectedForPoints = new Dictionary<int, int>();
    private Dictionary<int, int> threeOfAKindScores;
    private Dictionary<int, int> singlesPoints;
    private Dictionary<int, int> pointMultipliers;
    private Dictionary<int, int> fiveHundredComboWithStragglerOne;
    private Dictionary<int, int> fiveHundredComboWithStragglerFive;
    private Dictionary<int, int> sevenHundredComboWithStragglerFive;
    //DICTIONARIES FOR DICE AND POINTS


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

    } // void Start()







//***************************************
// ------DICE MANAGEMENT------

    public void CreateDiceSet(){
        for(int i = 0; i < 6; i++){
            GameObject dummydie = Instantiate(die, new Vector3(leftMostPoint, 0, 0), transform.rotation);
            dice.Add(dummydie);
            dummydie.GetComponent<DiceBehavior>().setId(nextAvailabelId);
            nextAvailabelId++;
            leftMostPoint = leftMostPoint + 10;
        }
    }

    public void RollDice(){
        var diceToRoll = GameObject.FindGameObjectsWithTag("Dice");
        foreach(var die in diceToRoll){
            die.GetComponent<DiceBehavior>().StartRolling();
        }
    }

    // Getters and Setters for dice
    public void SetDiceSelectedForPoints(int id, int side){
        diceSelectedForPoints.Add(id, side);
    }
    public void RemoveDiceSelectedForPoints(int id){
        diceSelectedForPoints.Remove(id);
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
        if(count == fiveHundredComboWithStragglerFive){
            return 550;
        }
        if(count == fiveHundredComboWithStragglerOne){
            return 600;
        }
        if(count == sevenHundredComboWithStragglerFive){
            return 800;
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
            yield break;
        }
        if(diceCombo.Count == 5 &&
        diceCombo.TryGetValue(1, out _) &&
        diceCombo.TryGetValue(2, out _) &&
        diceCombo.TryGetValue(3, out _) &&
        diceCombo.TryGetValue(4, out _) &&
        diceCombo.TryGetValue(5, out _)){
            Debug.Log("broke cuz straight 500");
            yield break;
        }
        if(diceCombo.Count == 5 &&
        diceCombo.TryGetValue(2, out _) &&
        diceCombo.TryGetValue(3, out _) &&
        diceCombo.TryGetValue(4, out _) &&
        diceCombo.TryGetValue(5, out _) &&
        diceCombo.TryGetValue(6, out _)){
            Debug.Log("broke cuz straight 750");
            yield break;
        }
        foreach(var key in diceCombo.Keys){
            if(key == 1 || key == 5){
                Debug.Log("broke cuz 1 or 5");
                yield break;
            }else if(diceCombo[key] >= 3){
                Debug.Log("broke cuz frequency >= 3");
                yield break;
            }
        }
        gameOver.SetActive(true);
    }
    //---END BUST CHECK---

// ------END DICE MANAGEMENT------
//***************************************




//***************************************
// ------GAME LOGIC------
    public void StartTheGame(){
        CreateDiceSet();
        startButton.SetActive(false);
        diceInPlay = 6;
    }
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowPointsForCollecting(){
        int points = CollectPoints();
        if(points > 0){
            softScore.text = points.ToString();
            passButton.SetActive(true);
            rollAgainButton.SetActive(true);
        }
        if(points == 0){
            softScore.text = points.ToString();
            passButton.SetActive(false);
            rollAgainButton.SetActive(false);
        }
    }

    public void DisplayPoints(){
        int point = CollectPoints();
        Debug.Log("Points: " + point);
    }



    public void ScoreAndContinue(){
        String score = softScore.text.ToString();
        Debug.Log("score: " + score);
        int points = int.Parse(score);
        if(points != 0){
            hardScore.text += points.ToString();
            hardScorePoints += points;
            foreach(var keys in diceSelectedForPoints.Keys){
                
            }
        }
    }
}
