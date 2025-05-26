using System;
using System.Collections;
using System.Collections.Generic;

public class PointCombos{
    public readonly int singlePointTotal;
    public readonly Dictionary<int, int> singlePointSides;
    public readonly int comboPointTotal;
    public readonly Dictionary<int, int> comboPointSides;
    public readonly int fiveStraight;
    public readonly Dictionary<int, int> fiveStraightSides;
    public readonly int sixStraight;
    public readonly int straightWithStraggeler;
    public PointCombos(int singlePointTotal, Dictionary<int, int> singlePointSides,
                       int comboPointTotal, Dictionary<int, int> comboPointSides,
                       int fiveStraight, Dictionary<int, int> fiveStraightSides,
                       int sixStraight, int straightWithStraggeler){
        this.singlePointTotal = singlePointTotal;
        this.singlePointSides = singlePointSides;
        this.comboPointTotal = comboPointTotal;
        this.comboPointSides = comboPointSides;
        this.fiveStraight = fiveStraight;
        this.fiveStraightSides = fiveStraightSides;
        this.sixStraight = sixStraight;
        this.straightWithStraggeler = straightWithStraggeler;
    }


    public Dictionary<int, String> returnHighestScore(){
        if(singlePointTotal > comboPointTotal && singlePointTotal > fiveStraight){
            return new Dictionary<int, string> {{singlePointTotal, "single"}};
        }else if(comboPointTotal > singlePointTotal && comboPointTotal > fiveStraight){
            return new Dictionary<int, string> {{comboPointTotal, "combo"}};
        }else{
            return new Dictionary<int, string> {{fiveStraight, "five"}};
        }
    }

}