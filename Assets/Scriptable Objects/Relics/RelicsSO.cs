using UnityEngine;

public enum RelicTrigger { OnEncounterStart, OnBoardStart }
public enum RelicCondition { Always, PlayerMovesFirst }
public enum RelicEffect { GrantDefense, HealPlayer }

[CreateAssetMenu(fileName = "Relic", menuName = "ScriptableObjects/RelicSO", order = 3)]
public class RelicSO : ScriptableObject
{
    public string _displayName;
    [TextArea] public string _description;
    public Sprite _icon;
    [Min(0)] public int _price = 20;

    public RelicTrigger _trigger = RelicTrigger.OnBoardStart;
    public RelicCondition _condition = RelicCondition.Always;
    public RelicEffect _effect = RelicEffect.GrantDefense;
    [Min(1)] public int _amount = 10;
}