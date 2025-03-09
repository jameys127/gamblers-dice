using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class DiceBehavior : MonoBehaviour
{
    private int id;
    public Sprite[] sprites;
    private new SpriteRenderer renderer;
    private SpriteRenderer selected;
    private int side;
    public float rollSeconds;
    public float timeBetweenRolls;
    private bool isSelected = false;
    public LogicScript LogicScript;

    // Start is called before the first frame update
    void Start()
    {
        LogicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        selected = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Roll(float seconds){
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
    public void setId(int uniqueId){
        id = uniqueId;
    }
    public int getId(){
        return id;
    }

    void OnMouseUpAsButton(){
        if(!isSelected){
            selected.enabled = true;
            isSelected = true;
            LogicScript.SetDiceSelectedForPoints(id, side);
            LogicScript.ShowPointsForCollecting();
        } else {
            selected.enabled = false;
            LogicScript.RemoveDiceSelectedForPoints(id);
            LogicScript.ShowPointsForCollecting();
            // Debug.Log("unselected");
            isSelected = false;
        }
    }


    public void RollDiceBehavior(){
        side = Random.Range(1, 7);
        switch(side){
            case 1:
                renderer.sprite = sprites[0];
                LogicScript.SetDiceSelectedForPoints(id, side);
                break;
            case 2:
                renderer.sprite = sprites[1];
                LogicScript.SetDiceSelectedForPoints(id, side);
                break;
            case 3:
                renderer.sprite = sprites[2];
                LogicScript.SetDiceSelectedForPoints(id, side);
                break;
            case 4:
                renderer.sprite = sprites[3];
                LogicScript.SetDiceSelectedForPoints(id, side);
                break;
            case 5:
                renderer.sprite = sprites[4];
                LogicScript.SetDiceSelectedForPoints(id, side);
                break;
            case 6:
                renderer.sprite = sprites[5];
                LogicScript.SetDiceSelectedForPoints(id, side);
                break;
        }
        LogicScript.NotifyRollComplete();
    }
}
