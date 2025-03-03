using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBehavior : MonoBehaviour
{
    public Sprite Side1;
    public Sprite Side2;
    public Sprite Side3;
    public Sprite Side4;
    public Sprite Side5;
    public Sprite Side6;
    public int side;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollDiceBehavior(){
        side = Random.Range(1, 6);
        Debug.Log(side);
        switch(side){
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = Side1;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = Side2;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = Side3;
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().sprite = Side4;
                break;
            case 5:
                gameObject.GetComponent<SpriteRenderer>().sprite = Side5;
                break;
            case 6:
                gameObject.GetComponent<SpriteRenderer>().sprite = Side6;
                break;
        }
    }
}
