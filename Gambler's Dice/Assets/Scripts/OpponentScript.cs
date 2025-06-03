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
        yield return new WaitForSeconds(1.5f);
        logicScript.RollDice();
        Debug.Log("rolling...");
        yield return new WaitForSeconds(0.1f);
        if (!logicScript.CurrentlyOpponentTurn){
            Debug.Log("breaking out of opponent coroutine");
            yield break;
        }
        yield return new WaitUntil(() => logicScript.GetIfDiceCanBeSelected() && logicScript.CurrentlyOpponentTurn);
        // foreach(var die in logicScript.GetDiceList()){
        //     logicScript.SetDiceSelectedForPoints(die.GetComponent<DiceBehavior>().getId(), die.GetComponent<DiceBehavior>().GetSide());
        // }
        diceIDandSides = new Dictionary<int, int>();
        foreach(var die in logicScript.GetDiceList()){
            diceIDandSides.Add(die.GetComponent<DiceBehavior>().getId(), die.GetComponent<DiceBehavior>().GetSide());
        }
        Debug.Log("choosing now");
        yield return new WaitForSeconds(1f);
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
            yield return new WaitForSeconds(0.8f);
        }
        yield return new WaitForSeconds(0.5f);
        if(pressContinue){
            logicScript.ScoreAndContinue();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(StartYourTurn());
        }else{
            logicScript.ScoreAndPass();
        }
    }

    private List<PointCombos> AnalyzeRoll(){
        List<PointCombos> listOfCombos = new List<PointCombos>();
        // checking for straights
        Dictionary<int, int> sidesAndFrequency = new Dictionary<int, int>();
        foreach (var pair in diceIDandSides){
            int increm;
            if(sidesAndFrequency.TryGetValue(pair.Value, out increm)){
                increm++;
                sidesAndFrequency[pair.Value] = increm;
            }else{
                sidesAndFrequency.Add(pair.Value, 1);
            }
        }
        if(sidesAndFrequency.Count == 6){
            listOfCombos.Add(new PointCombos(1500, diceIDandSides, "six straight"));
        }
        if(sidesAndFrequency.Count == 5){
            if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.fiveHundredComboWithStragglerFive)){
                listOfCombos.Add(new PointCombos(550, diceIDandSides, "5 straight with straggler 5"));
            }
            if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.fiveHundredComboWithStragglerOne)){
                listOfCombos.Add(new PointCombos(600, diceIDandSides, "5 straight with straggler 1"));
            }
            if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.sevenHundredComboWithStragglerFive)){
                listOfCombos.Add(new PointCombos(800, diceIDandSides, "2-6 straight with straggler 5"));
            }
            if(sidesAndFrequency.TryGetValue(1, out _) &&
                sidesAndFrequency.TryGetValue(2, out _) &&
                sidesAndFrequency.TryGetValue(3, out _) &&
                sidesAndFrequency.TryGetValue(4, out _) &&
                sidesAndFrequency.TryGetValue(5, out _)){
                    int count = 1;
                    Dictionary<int, int> selectedIdsAndSides = new Dictionary<int, int>();
                    foreach(int key in diceIDandSides.Keys){
                        if(diceIDandSides[key] == count && count < 6){
                            selectedIdsAndSides.Add(key, diceIDandSides[key]);
                            count++;
                        }
                    }
                    listOfCombos.Add(new PointCombos(500, selectedIdsAndSides, "normal 1-5 straight"));
            }
            if(sidesAndFrequency.TryGetValue(2, out _) &&
                sidesAndFrequency.TryGetValue(3, out _) &&
                sidesAndFrequency.TryGetValue(4, out _) &&
                sidesAndFrequency.TryGetValue(5, out _) &&
                sidesAndFrequency.TryGetValue(6, out _)){
                    int count = 2;
                    Dictionary<int, int> selectedIdsAndSides = new Dictionary<int, int>();
                    foreach(int key in diceIDandSides.Keys){
                        if(diceIDandSides[key] == count && count < 7){
                            selectedIdsAndSides.Add(key, diceIDandSides[key]);
                            count++;
                        }
                    }
                    listOfCombos.Add(new PointCombos(750, selectedIdsAndSides, "normal 2-6 straight"));
            }

        }
        foreach(var pair in sidesAndFrequency){
            if(pair.Value >= 3){
                int baseScore = logicScript.threeOfAKindScores[pair.Key];
                int multScore = logicScript.pointMultipliers[pair.Value];
                int totalScore = baseScore * multScore;
                Dictionary<int, int> threeOfAKind = new Dictionary<int, int>();
                foreach(int key in diceIDandSides.Keys){
                    if(diceIDandSides[key] == pair.Key){
                        threeOfAKind.Add(key, diceIDandSides[key]);
                    }
                }
                listOfCombos.Add(new PointCombos(totalScore, threeOfAKind, $"{pair.Value} of a kind, ({pair.Key} appears {pair.Value} times)"));
            }
        }
        
        List<int> oneIDs = new List<int>();
        List<int> fiveIDs = new List<int>();

        foreach(var pair in diceIDandSides){
            if(pair.Value == 1){
                oneIDs.Add(pair.Key);
            }else if(pair.Value == 5){
                fiveIDs.Add(pair.Key);
            }
        }

        for(int numOnes = 0; numOnes <= oneIDs.Count; numOnes++){
            for(int numFives = 0; numFives <= fiveIDs.Count; numFives++){
                if(numOnes > 0 || numFives > 0){
                    Dictionary<int, int> selectedSingles = new Dictionary<int, int>();

                    for(int i = 0; i < numOnes; i++){
                        selectedSingles.Add(oneIDs[i], 1);
                    }
                    for(int i = 0; i < numFives; i++){
                        selectedSingles.Add(fiveIDs[i], 5);
                    }
                    int points = (numOnes * 100) + (numFives * 50);
                    listOfCombos.Add(new PointCombos(points, selectedSingles, $"{numOnes} ones + {numFives} fives"));
                }
            }
        }
        return listOfCombos;













        // //analyzing possible single die selection point totals
        // int oneCounter = 0;
        // int fiveCounter = 0;
        // int singlePointTotal = 0;
        // Dictionary<int, int> singlePointSides = new Dictionary<int, int>();
        // foreach(var pair in diceIDandSides){
        //     if(pair.Value == 1 && oneCounter < 3){
        //         oneCounter++;
        //         singlePointTotal += 100;
        //         singlePointSides.Add(pair.Key, pair.Value);
        //     }else if(pair.Value == 5 && fiveCounter < 3){
        //         fiveCounter++;
        //         singlePointTotal += 50;
        //         singlePointSides.Add(pair.Key, pair.Value);
        //     }
        // }

        // //analyzing possible combo point totals
        // int comboPointTotal = 0;
        // Dictionary<int, int> comboPointSides = new Dictionary<int, int>();
        // Dictionary<int, int> sidesAndFrequency = new Dictionary<int, int>();
        // foreach(var pair in diceIDandSides){
        //     int increm;
        //     if(sidesAndFrequency.TryGetValue(pair.Value, out increm)){
        //         increm++;
        //         sidesAndFrequency[pair.Value] = increm;
        //     }else{
        //         sidesAndFrequency.Add(pair.Value, 1);
        //     }
        // }
        // foreach(var pair in sidesAndFrequency){
        //     int diceSide = pair.Key;
        //     int frequency = pair.Value;
        //     if(frequency >= 3){
        //         int baseScore = logicScript.threeOfAKindScores[diceSide];
        //         int multiplier = logicScript.pointMultipliers[frequency];
        //         comboPointTotal += baseScore * multiplier;
        //         // comboPointSides.Add(diceSide);
        //         foreach(int key in diceIDandSides.Keys){
        //             if(diceIDandSides[key] == diceSide){
        //                 comboPointSides.Add(key, diceIDandSides[key]);
        //             }
        //         }
        //     }
        // }

        // //analyzing possible straights 5 and 6
        // int fiveStraight = 0;
        // int sixStraight = 0;
        // int straightWithStraggler = 0;
        // Dictionary<int, int> fiveStraightSides = new Dictionary<int, int>();
        // if(sidesAndFrequency.Count == 6){
        //     sixStraight = 1500;
        // }else if(sidesAndFrequency.Count == 5){
        //     if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.fiveHundredComboWithStragglerFive)){
        //         straightWithStraggler = 550;
        //     }else if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.fiveHundredComboWithStragglerOne)){
        //         straightWithStraggler = 600;
        //     }else if(logicScript.CheckStragglersForPoints(sidesAndFrequency, logicScript.sevenHundredComboWithStragglerFive)){
        //         straightWithStraggler = 800;
        //     }else if(sidesAndFrequency.TryGetValue(1, out _) && !sidesAndFrequency.ContainsKey(6)){
        //         fiveStraight = 500;
        //         foreach(int key in diceIDandSides.Keys){
        //             foreach(int side in sidesAndFrequency.Keys){
        //                 if(diceIDandSides[key] == side){
        //                     if(!fiveStraightSides.ContainsValue(side)){
        //                         fiveStraightSides.Add(key, side);
        //                     }
        //                 }
        //             }
        //         }
        //     }else{
        //         fiveStraight = 750;
        //         foreach(int key in diceIDandSides.Keys){
        //             foreach(int side in sidesAndFrequency.Keys){
        //                 if(diceIDandSides[key] == side){
        //                     if(!fiveStraightSides.ContainsValue(side)){
        //                         fiveStraightSides.Add(key, side);
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }

        // PointCombos pointCombos = new PointCombos(singlePointTotal, singlePointSides,
        //                                           comboPointTotal, comboPointSides, 
        //                                           fiveStraight, fiveStraightSides, 
        //                                           sixStraight, straightWithStraggler);
        // return pointCombos;

    }

    private Dictionary<int, int> PickBestCombo(PointCombos pointCombo){

        if(pointCombo.sixStraight != 0 || pointCombo.straightWithStraggeler != 0){
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
