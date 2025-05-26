using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentScript : MonoBehaviour
{
    LogicScript logicScript;
    public float timeBetweenSelections;
    Dictionary<int, int> diceIDandSides;
    Boolean pressContinue;

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
        yield return new WaitForSecondsRealtime(1.5f);
        logicScript.RollDice();
        Debug.Log("rolling...");
        yield return new WaitUntil(() => logicScript.GetIfDiceCanBeSelected() && logicScript.CurrentlyOpponentTurn);
        if(!logicScript.CurrentlyOpponentTurn) yield break;
        foreach(var die in logicScript.GetDiceList()){
            logicScript.SetDiceSelectedForPoints(die.GetComponent<DiceBehavior>().getId(), die.GetComponent<DiceBehavior>().GetSide());
        }
        diceIDandSides = new Dictionary<int, int>();
        foreach(var die in logicScript.GetDiceList()){
            diceIDandSides.Add(die.GetComponent<DiceBehavior>().getId(), die.GetComponent<DiceBehavior>().GetSide());
        }
        Debug.Log("choosing now");
        yield return new WaitForSecondsRealtime(1f);
        Dictionary<int, int> diceToSelect = PickBestCombo(AnalyzeRoll());
        logicScript.diceSelectedForPoints.Clear();
        foreach(var pair in diceToSelect){
            logicScript.SetDiceSelectedForPoints(pair.Key, pair.Value);
            logicScript.ShowPointsForCollecting();
            foreach(var die in logicScript.GetDiceList()){
                if(pair.Key.Equals(die.GetComponent<DiceBehavior>().getId())){
                    die.GetComponent<DiceBehavior>().selectionTargetOn();
                }
            }
            yield return new WaitForSecondsRealtime(0.8f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        if(pressContinue){
            logicScript.ScoreAndContinue();
            yield return new WaitForSecondsRealtime(0.5f);
            StartCoroutine(StartYourTurn());
        }else{
            logicScript.ScoreAndPass();
        }
    }

    private PointCombos AnalyzeRoll(){

        //analyzing possible single die selection point totals
        int oneCounter = 0;
        int fiveCounter = 0;
        int singlePointTotal = 0;
        Dictionary<int, int> singlePointSides = new Dictionary<int, int>();
        foreach(var pair in diceIDandSides){
            if(pair.Value == 1 && oneCounter < 3){
                oneCounter++;
                singlePointTotal += 100;
                singlePointSides.Add(pair.Key, pair.Value);
            }else if(pair.Value == 5 && fiveCounter < 3){
                fiveCounter++;
                singlePointTotal += 50;
                singlePointSides.Add(pair.Key, pair.Value);
            }
        }

        //analyzing possible combo point totals
        int comboPointTotal = 0;
        Dictionary<int, int> comboPointSides = new Dictionary<int, int>();
        Dictionary<int, int> sidesAndFrequency = new Dictionary<int, int>();
        foreach(var pair in diceIDandSides){
            int increm;
            if(sidesAndFrequency.TryGetValue(pair.Value, out increm)){
                increm++;
                sidesAndFrequency[pair.Value] = increm;
            }else{
                sidesAndFrequency.Add(pair.Value, 1);
            }
        }
        foreach(var pair in sidesAndFrequency){
            int diceSide = pair.Key;
            int frequency = pair.Value;
            if(frequency >= 3){
                int baseScore = logicScript.threeOfAKindScores[diceSide];
                int multiplier = logicScript.pointMultipliers[frequency];
                comboPointTotal += baseScore * multiplier;
                // comboPointSides.Add(diceSide);
                foreach(int key in diceIDandSides.Keys){
                    if(diceIDandSides[key] == diceSide){
                        comboPointSides.Add(key, diceIDandSides[key]);
                    }
                }
            }
        }

        //analyzing possible straights 5 and 6
        int fiveStraight = 0;
        int sixStraight = 0;
        int straightWithStraggler = 0;
        Dictionary<int, int> fiveStraightSides = new Dictionary<int, int>();
        if(sidesAndFrequency.Count == 6){
            sixStraight = 1500;
        }else if(sidesAndFrequency.Count == 5){
            if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.fiveHundredComboWithStragglerFive)){
                straightWithStraggler = 550;
            }else if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.fiveHundredComboWithStragglerOne)){
                straightWithStraggler = 600;
            }else if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.sevenHundredComboWithStragglerFive)){
                straightWithStraggler = 800;
            }else if(sidesAndFrequency.TryGetValue(1, out _)){
                fiveStraight = 500;
                foreach(int key in diceIDandSides.Keys){
                    foreach(int side in sidesAndFrequency.Keys){
                        if(diceIDandSides[key] == side){
                            if(!fiveStraightSides.ContainsValue(side)){
                                fiveStraightSides.Add(key, side);
                            }
                        }
                    }
                }
            }else{
                fiveStraight = 750;
                foreach(int key in diceIDandSides.Keys){
                    foreach(int side in sidesAndFrequency.Keys){
                        if(diceIDandSides[key] == side){
                            if(!fiveStraightSides.ContainsValue(side)){
                                fiveStraightSides.Add(key, side);
                            }
                        }
                    }
                }
            }
        }

        PointCombos pointCombos = new PointCombos(singlePointTotal, singlePointSides,
                                                  comboPointTotal, comboPointSides, 
                                                  fiveStraight, fiveStraightSides, 
                                                  sixStraight, straightWithStraggler);
        return pointCombos;

    }

    private Dictionary<int, int> PickBestCombo(PointCombos pointCombo){
        // int highestPossibleScore = 0;

        if(pointCombo.sixStraight != 0 || pointCombo.straightWithStraggeler != 0){
            // highestPossibleScore = (pointCombo.sixStraight != 0) ? pointCombo.sixStraight : pointCombo.straightWithStraggeler;
            pressContinue = true;
            return diceIDandSides;
        }else{
            Dictionary<int, String> highestScore = pointCombo.returnHighestScore();
            if(highestScore.ContainsValue("single")){
                if(logicScript.diceInPlay - pointCombo.singlePointSides.Count >= 3){
                    pressContinue = true;
                }else{
                    pressContinue = false;
                }
                return pointCombo.singlePointSides;
            }else if(highestScore.ContainsValue("combo")){
                if(logicScript.diceInPlay - pointCombo.comboPointSides.Count >= 3){
                    pressContinue = true;
                }else{
                    pressContinue = false;
                }
                return pointCombo.comboPointSides;
            }else{
                if(logicScript.diceInPlay - pointCombo.singlePointSides.Count == 0){
                    pressContinue = true;
                }else{
                    pressContinue = false;
                }
                return pointCombo.fiveStraightSides;
            }
        }
    }
}
