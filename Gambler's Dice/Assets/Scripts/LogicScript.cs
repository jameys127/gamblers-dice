using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicScript : MonoBehaviour
{
    public GameObject diceManger;
    public GameObject startButton;
    public void RollDice(){
        var dice = GameObject.FindGameObjectsWithTag("Dice");
        foreach(var die in dice){
            die.GetComponent<DiceBehavior>().StartRolling();
        }
    }

    public void StartTheGame(){
        diceManger.GetComponent<DiceManagerScript>().CreateDiceSet();
        startButton.SetActive(false);
    }
}
