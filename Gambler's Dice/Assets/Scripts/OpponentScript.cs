using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentScript : MonoBehaviour
{
    public GameObject logicManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpponentTurn(){
        StartCoroutine(StartYourTurn());
    }

    private IEnumerator StartYourTurn(){
        LogicScript logic = logicManager.GetComponent<LogicScript>();
        yield return new WaitForSeconds(1.5f);
        logic.RollDice();
        Debug.Log("rolling...");
        yield return new WaitUntil(() => logic.GetIfDiceCanBeSelected());
        Debug.Log("choosing now");
        
    }








}
