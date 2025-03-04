using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManagerScript : MonoBehaviour
{
    public float centerXPoint = 5;
    public float leftMostPoint = -20;
    public float rightMostPoint = 30;
    public GameObject die;
    private List<GameObject> dice = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
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
    }

}
