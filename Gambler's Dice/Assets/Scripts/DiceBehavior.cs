using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DiceBehavior : MonoBehaviour
{
    public Sprite[] sprites;
    public int side;
    public float rollSeconds;
    public float timeBetweenRolls;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Roll(float seconds){
        float timer = 0f;
        while(timer < seconds){
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0,6)];
            timer += Time.deltaTime;
            yield return null;
        }
        RollDiceBehavior();
    }

    public void StartRolling(){
        StartCoroutine(Roll(rollSeconds));
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
