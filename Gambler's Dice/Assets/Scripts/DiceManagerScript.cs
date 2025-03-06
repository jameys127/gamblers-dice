using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceManagerScript : MonoBehaviour
{
    private int nextAvailabelId = 0;
    public float centerXPoint = 5;
    public float leftMostPoint = -20;
    public float rightMostPoint = 30;
    public GameObject die;
    private List<GameObject> dice = new List<GameObject>();
    private Dictionary<int, int> diceCombo = new Dictionary<int, int>();
    private Dictionary<int, int> threeOfAKindScores;
    private Dictionary<int, int> singlesPoints;
    private Dictionary<int, int> pointMultipliers;
    private Dictionary<int, int> fiveHundredComboWithStragglerOne;
    private Dictionary<int, int> fiveHundredComboWithStragglerFive;
    private Dictionary<int, int> sevenHundredComboWithStragglerFive;
    // Start is called before the first frame update
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
    }

    public Dictionary<int, int> GetDiceSelectedForPoints(){
        return diceCombo;
    }
    public void SetDiceSelectedForPoints(int id, int side){
        diceCombo.Add(id, side);
    }
    public void RemoveDiceSelectedForPoints(int id){
        diceCombo.Remove(id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDiceSet(){
        for(int i = 0; i < 6; i++){
            GameObject dummydie = Instantiate(die, new Vector3(leftMostPoint, 0, 0), transform.rotation);
            dice.Add(dummydie);
            dummydie.GetComponent<DiceBehavior>().setId(nextAvailabelId);
            nextAvailabelId++;
            leftMostPoint = leftMostPoint + 10;
        }
    }
    public void ReturnDiceSide(){
        // Debug.Log("First die side: " + dice[0].GetComponent<DiceBehavior>().GetSide());
        Debug.Log("Selected die number: " + string.Join(", ", diceCombo));
    }

    public int CollectPoints(){
        int invalidSelection = 0;
        int totalScore = 0;
        Dictionary<int, int> count = new Dictionary<int, int>();
        foreach (var value in diceCombo.Values){
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
        count.TryGetValue(2, out _) &&
        count.TryGetValue(3, out _) &&
        count.TryGetValue(4, out _) &&
        count.TryGetValue(5, out _)){
            return 500;
        }
        if(count.Count == 5 &&
        count.TryGetValue(2, out _) &&
        count.TryGetValue(3, out _) &&
        count.TryGetValue(4, out _) &&
        count.TryGetValue(5, out _) &&
        count.TryGetValue(6, out _)){
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
}