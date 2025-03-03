using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBehavior : MonoBehaviour
{
    public Sprite[] sprites;
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
        side = Random.Range(1, 7);
        Debug.Log(side);
        switch(side){
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
                break;
            case 5:
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[4];
                break;
            case 6:
                gameObject.GetComponent<SpriteRenderer>().sprite = sprites[5];
                break;
        }
    }
}
