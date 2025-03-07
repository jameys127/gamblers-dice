using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    private int diceInPlay;
    private int rollingCounter;
    public GameObject gameOver;
    static private DiceManagerScript diceManager;
    public GameObject diceManagerObject;
    public GameObject startButton;
    public GameObject passButton;
    public GameObject rollAgainButton;
    public TextMeshProUGUI softScore;
    public TextMeshProUGUI hardScore;

    void Start()
    {
        diceManager = diceManagerObject.GetComponent<DiceManagerScript>();
    }
    public void RollDice(){
        var dice = GameObject.FindGameObjectsWithTag("Dice");
        foreach(var die in dice){
            die.GetComponent<DiceBehavior>().StartRolling();
        }
    }

    public void NotifyRollComplete(){
        rollingCounter++;
        if(rollingCounter == diceInPlay){
            Debug.Log("Initiating bust check becasue counter: " + rollingCounter);
            StartCoroutine(BustCheck());
            rollingCounter = 0;
        }
    }

    private IEnumerator BustCheck(){
        yield return new WaitForSeconds(0.1f);
        Dictionary<int, int> dummy = new Dictionary<int, int>(diceManager.GetDiceSelectedForPoints());
        Dictionary<int, int> diceCombo = new Dictionary<int, int>();
        foreach(var key in dummy.Keys){
            diceManager.RemoveDiceSelectedForPoints(key);
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
        if(diceCombo.Count >= 5){
            Debug.Log("broke cuz straight");
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

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetSelectedCount(int count){
        int points = diceManager.CollectPoints();
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
        int point = diceManager.CollectPoints();
        Debug.Log("Points: " + point);
    }

    public void StartTheGame(){
        diceManager.CreateDiceSet();
        startButton.SetActive(false);
        diceInPlay = 6;
    }
}
