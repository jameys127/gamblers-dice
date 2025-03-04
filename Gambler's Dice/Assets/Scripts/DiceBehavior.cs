using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class DiceBehavior : MonoBehaviour
{
    public Sprite[] sprites;
    private new SpriteRenderer renderer;
    private SpriteRenderer selected;
    private int side;
    public float rollSeconds;
    public float timeBetweenRolls;
    private bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        selected = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Roll(float seconds){
        float timer = 0f;
        while(timer < seconds){
            renderer.sprite = sprites[Random.Range(0,6)];
            timer += timeBetweenRolls + Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        RollDiceBehavior();
    }

    public void StartRolling(){
        StartCoroutine(Roll(rollSeconds));
    }
    public int GetSide(){
        return side;
    }
    void OnMouseUpAsButton(){
        if(!isSelected){
            selected.enabled = true;
            Debug.Log("selected");
            isSelected = true;
        } else {
            selected.enabled = false;
            Debug.Log("unselected");
            isSelected = false;
        }
    }


    public void RollDiceBehavior(){
        side = Random.Range(1, 7);
        Debug.Log(side);
        switch(side){
            case 1:
                renderer.sprite = sprites[0];
                break;
            case 2:
                renderer.sprite = sprites[1];
                break;
            case 3:
                renderer.sprite = sprites[2];
                break;
            case 4:
                renderer.sprite = sprites[3];
                break;
            case 5:
                renderer.sprite = sprites[4];
                break;
            case 6:
                renderer.sprite = sprites[5];
                break;
        }
    }
}
