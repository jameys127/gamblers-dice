using System.Collections;

public class PointCombos{
    public readonly int singlePointTotal;
    public readonly ArrayList singlePointSides;
    public readonly int comboPointTotal;
    public readonly ArrayList comboPointSides;
    public readonly int fiveStraight;
    public readonly ArrayList fiveStraightSides;
    public readonly int sixStraight;
    public PointCombos(int singlePointTotal, ArrayList singlePointSides,
                       int comboPointTotal, ArrayList comboPointSides,
                       int fiveStraight, ArrayList fiveStraightSides,
                       int sixStraight){
        this.singlePointTotal = singlePointTotal;
        this.singlePointSides = singlePointSides;
        this.comboPointTotal = comboPointTotal;
        this.comboPointSides = comboPointSides;
        this.fiveStraight = fiveStraight;
        this.fiveStraightSides = fiveStraightSides;
        this.sixStraight = sixStraight;
    }
}