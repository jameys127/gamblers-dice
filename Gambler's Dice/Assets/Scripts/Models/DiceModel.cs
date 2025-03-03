using UnityEngine;

[CreateAssetMenu(fileName = "Die", menuName = "Dice")]
public class DiceModel : ScriptableObject
{
    public Sprite dieSide;
    public int number;
}
