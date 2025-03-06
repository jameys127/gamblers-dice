using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceManagerScript : MonoBehaviour
{
    public float centerXPoint = 5;
    public float leftMostPoint = -20;
    public float rightMostPoint = 30;
    public GameObject die;
    private List<GameObject> dice = new List<GameObject>();
    private int dicePoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int getDicePoitn(){
        return dicePoint;
    }
    public void setDicePoint(int point){
        dicePoint = point;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDiceSet(){
        for(int i = 0; i < 6; i++){
            GameObject dummydie = Instantiate(die, new Vector3(leftMostPoint, 0, 0), transform.rotation);
            dice.Add(dummydie);
            leftMostPoint = leftMostPoint + 10;
        }
    }
    public void ReturnDiceSide(){
        Debug.Log("First die side: " + dice[0].GetComponent<DiceBehavior>().GetSide());
        Debug.Log("Selected die number: " + dicePoint);
    }

}
