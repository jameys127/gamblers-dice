using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicScript : MonoBehaviour
{
    public GameObject die;
    public void RollDice(){
        die.GetComponent<DiceBehavior>().RollDiceBehavior();
    }
}
