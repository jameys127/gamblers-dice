using System;
using System.Collections;
using System.Collections.Generic;

public class PointCombos
{
    public int points;
    public Dictionary<int, int> diceToSelect;
    public string descriptions;

    public int DiceUsed => diceToSelect.Count;

    public PointCombos(int points, Dictionary<int, int> diceToSelect, string descriptions){
        this.descriptions = descriptions;
        this.diceToSelect = diceToSelect;
        this.points = points;
    }

}