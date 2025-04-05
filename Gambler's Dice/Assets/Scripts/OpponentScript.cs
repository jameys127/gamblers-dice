using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentScript : MonoBehaviour
{
    public GameObject logicManager;
    public float timeBetweenSelections;

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
        yield return new WaitForSecondsRealtime(1.5f);
        logic.RollDice();
        Debug.Log("rolling...");
        yield return new WaitUntil(() => logic.GetIfDiceCanBeSelected());
        Debug.Log("choosing now");
        yield return new WaitForSecondsRealtime(1f);
        List<GameObject> dice = logic.GetDiceList();
        foreach(var die in dice){
            DiceBehavior dieScript = die.GetComponent<DiceBehavior>();
            if(dieScript.GetSide() == 1){
                logic.SetDiceSelectedForPoints(dieScript.getId(), dieScript.GetSide());
                logic.ShowPointsForCollecting();
                dieScript.selectionTargetOn();
                yield return new WaitForSecondsRealtime(timeBetweenSelections);
                continue;
            }
            if(dieScript.GetSide() == 5){
                logic.SetDiceSelectedForPoints(dieScript.getId(), dieScript.GetSide());
                logic.ShowPointsForCollecting();
                dieScript.selectionTargetOn();
                yield return new WaitForSecondsRealtime(timeBetweenSelections);
                continue;
            }
    
        }
        
    }








}
