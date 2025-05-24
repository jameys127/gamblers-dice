using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentScript : MonoBehaviour
{
    LogicScript logicScript;
    public float timeBetweenSelections;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpponentTurn(LogicScript logic){
        logicScript = logic;
        StartCoroutine(StartYourTurn());
    }

    private IEnumerator StartYourTurn(){
        // yield return new WaitForSecondsRealtime(1.5f);
        // logic.RollDice();
        // Debug.Log("rolling...");
        // yield return new WaitUntil(() => logic.GetIfDiceCanBeSelected());
        // Debug.Log("choosing now");
        // yield return new WaitForSecondsRealtime(1f);
        // List<GameObject> dice = logic.GetDiceList();
        // foreach(var die in dice){
        //     DiceBehavior dieScript = die.GetComponent<DiceBehavior>();
        //     if(dieScript.GetSide() == 1){
        //         logic.SetDiceSelectedForPoints(dieScript.getId(), dieScript.GetSide());
        //         logic.ShowPointsForCollecting();
        //         dieScript.selectionTargetOn();
        //         yield return new WaitForSecondsRealtime(timeBetweenSelections);
        //         continue;
        //     }
        //     if(dieScript.GetSide() == 5){
        //         logic.SetDiceSelectedForPoints(dieScript.getId(), dieScript.GetSide());
        //         logic.ShowPointsForCollecting();
        //         dieScript.selectionTargetOn();
        //         yield return new WaitForSecondsRealtime(timeBetweenSelections);
        //         continue;
        //     }
    
        // } 
    }

    private void AnalyzeRoll(){
        Dictionary<int, int> diceIDandSides = new Dictionary<int, int>(logicScript.diceSelectedForPoints);

        int oneCounter = 0;
        int fiveCounter = 0;
        int singlePointTotal = 0;
        ArrayList singlePointSides = new ArrayList();
        foreach(int side in diceIDandSides.Values){
            if(side == 1 && oneCounter < 3){
                oneCounter++;
                singlePointTotal += 100;
                singlePointSides.Add(1);
            }else if(side == 5 && fiveCounter < 3){
                fiveCounter++;
                singlePointTotal += 50;
                singlePointSides.Add(5);
            }
        }


    }

    private void PickBestCombo(){

    }

    private void RiskAssessment(){
        
    }
}
