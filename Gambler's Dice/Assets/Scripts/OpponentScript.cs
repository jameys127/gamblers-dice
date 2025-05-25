using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentScript : MonoBehaviour
{
    LogicScript logicScript;
    public float timeBetweenSelections;
    Dictionary<int, int> diceIDandSides;

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
        diceIDandSides = new Dictionary<int, int>(logicScript.diceSelectedForPoints);
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

    private PointCombos AnalyzeRoll(){

        //analyzing possible single die selection point totals
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

        //analyzing possible combo point totals
        int comboPointTotal = 0;
        ArrayList comboPointSides = new ArrayList();
        Dictionary<int, int> sidesAndFrequency = new Dictionary<int, int>();
        foreach(var value in diceIDandSides.Values){
            int increm;
            if(sidesAndFrequency.TryGetValue(value, out increm)){
                increm++;
                sidesAndFrequency[value] = increm;
            }else{
                sidesAndFrequency.Add(value, 1);
            }
        }
        foreach(var pair in sidesAndFrequency){
            int diceSide = pair.Key;
            int frequency = pair.Value;
            if(frequency >= 3){
                int baseScore = logicScript.threeOfAKindScores[diceSide];
                int multiplier = logicScript.pointMultipliers[frequency];
                comboPointTotal += baseScore * multiplier;
                comboPointSides.Add(diceSide);
            }
        }

        //analyzing possible straights 5 and 6
        int fiveStraight = 0;
        int sixStraight = 0;
        ArrayList fiveStraightSides = new ArrayList();
        if(sidesAndFrequency.Count == 6){
            sixStraight = 1500;
        }else if(sidesAndFrequency.Count == 5){
            if(sidesAndFrequency.TryGetValue(1, out _)){
                fiveStraight = 500;
                fiveStraightSides.Add(1);
                fiveStraightSides.Add(2);
                fiveStraightSides.Add(3);
                fiveStraightSides.Add(4);
                fiveStraightSides.Add(5);
            }else{
                fiveStraight = 750;
                fiveStraightSides.Add(2);
                fiveStraightSides.Add(3);
                fiveStraightSides.Add(4);
                fiveStraightSides.Add(5);
                fiveStraightSides.Add(6);
            }
        }

        PointCombos pointCombos = new PointCombos(singlePointTotal, singlePointSides,
                                                  comboPointTotal, comboPointSides, 
                                                  fiveStraight, fiveStraightSides, 
                                                  sixStraight);
        return pointCombos;

    }

    private void PickBestCombo(){

    }

    private void RiskAssessment(){
        
    }
}
